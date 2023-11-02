#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Text.Json;

namespace Ara3D.Domo.Tests
{
    public readonly record struct TestRecord(int X, int Y);



    public static class Tests
    {
        [Test]
        public static void Test1()
        {
            var rec = new TestRecord(1, 2);
            Console.WriteLine($"{rec}");
            var jsonString = JsonSerializer.Serialize(rec);
            Console.WriteLine(jsonString);
            var rec2 = JsonSerializer.Deserialize<TestRecord>(jsonString);
            Console.WriteLine(rec2);
            var jsonString2 = JsonSerializer.Serialize(rec2);
            Assert.AreEqual(jsonString, jsonString2);
        }

        public static void OutputRepo(IRepository r)
        {
            Console.WriteLine($"{r.GetTypeName()} {r.ValueType.Name}");
            var models = string.Join(", ", r.GetModels().Select(m => m.ToDebugString()));
            Console.WriteLine($"[{models}]");
        }

        public record ModelRecord
        (
            Guid Id,
            string Type,
            object Value
        );

        public static Utf8JsonWriter Write(this Utf8JsonWriter writer, IModel m)
        {
            JsonSerializer.Serialize(writer, new ModelRecord(m.Id, m.ValueType.FullName, m.Value));
            return writer;
        }

        public static Utf8JsonWriter Write(this Utf8JsonWriter writer, IRepository r)
        {
            writer.WriteStartObject();
            writer.WriteString("RepoType", r.GetTypeName());
            writer.WriteString("ValueType", r.ValueType.Name);
            writer.WritePropertyName("Models");
            JsonSerializer.Serialize(writer, r.GetModelDictionary());
            writer.WriteEndObject();
            writer.Flush();
            return writer;
        }

        public static string ToStringWithStream(this Action<Stream> action)
        {
            using var stream = new MemoryStream();
            action(stream);
            return System.Text.Encoding.UTF8.GetString(stream.ToArray());
        }

        public static string ToStringWithJsonWriter(this Action<Utf8JsonWriter> action, JsonWriterOptions options = default)
            => ToStringWithStream(stream => action(new Utf8JsonWriter(stream, options)));

        public static void Read(ref Utf8JsonReader reader, JsonTokenType type)
        {
            if (!reader.Read())
                throw new Exception("Reading failed");
            if (reader.TokenType != type)
                throw new Exception($"Expected token type {type} but instead got {reader.TokenType}");
        }

        public static void Deserialize<T>(this Utf8JsonReader reader, IRepository<T> repo)
        {
            repo.Clear();
            Read(ref reader, JsonTokenType.StartObject);

            var repoType = "";
            var valueType = "";
            var models = new List<IModel>();

            reader.Read();
            while (reader.TokenType == JsonTokenType.PropertyName)
            {
                var name = reader.GetString();
                if (name == "RepoType")
                {
                    reader.Read();
                    repoType = reader.GetString();
                }
                else 
                if (name == "ValueType")
                {
                    reader.Read();
                    valueType = reader.GetString();
                }
                else if (name == "Models")
                {
                    reader.Read();
                    var d = JsonSerializer.Deserialize<IDictionary<Guid, T>>(ref reader);
                    foreach (var kv in d)
                    {
                        // TODO: Serialization is not properly implemented in Domo anymore. 
                        throw new NotImplementedException();
                        // repo.Add(kv.Key, kv.Value);
                    }
                }

                reader.Read();
            }
            Console.WriteLine($"RepoType = {repoType}, ValueType =  {valueType}");
        }

        public static string ToJson(this IRepository repo)
            => ToStringWithJsonWriter(writer => writer.Write(repo), new() { Indented = true });
        

        [Test]
        public static void SerializeRepo()
        {
            var mgr = new RepositoryManager();
            var repo = CreateAggregateRepository(mgr);
            var text = repo.ToJson();
            Console.WriteLine(text);
            //var repo2 = JsonSerializer.Deserialize<AggregateRepository<TestRecord>>(text);
            var bytes = Encoding.UTF8.GetBytes(text);
            var reader = new Utf8JsonReader(bytes);
            var repo2 = new AggregateRepository<TestRecord>();
            Deserialize(reader, repo2);
            var text2 = repo2.ToJson();
            Assert.AreEqual(text, text2);
        }

        public static IAggregateRepository<TestRecord> CreateAggregateRepository(RepositoryManager mgr)
        {
            var repo = mgr.AddAggregateRepository<TestRecord>();
            repo.Add(new TestRecord(1, 2));
            repo.Add(new TestRecord(1, 3));
            repo.Add(new TestRecord(2, 4));
            repo.Add(new TestRecord(2, 5));
            return repo;
        }

        [Test]
        public static void TestSingletonRepo()
        {
            var rec = new TestRecord(1, 2);
            var mgr = new RepositoryManager();
            var repo = mgr.AddSingletonRepository(rec);
            var modelId = repo.Model.Id;
            OutputRepo(repo);
            repo.Model.Value = new TestRecord(1, 3);
            Assert.AreEqual(modelId, repo.Model.Id);
            Assert.AreEqual(3, repo.Model.Value.Y);
            OutputRepo(repo);
            repo.Model.Value = repo.Model.Value with { Y = 4 };
            Assert.AreEqual(modelId, repo.Model.Id);
            Assert.AreEqual(4, repo.Model.Value.Y);
            OutputRepo(repo);
            repo.Model.Update(x => x with { Y = 5 });
            Assert.AreEqual(modelId, repo.Model.Id);
            Assert.AreEqual(5, repo.Model.Value.Y);
            OutputRepo(repo);
            repo.Update(repo.Model.Id, x => x with { Y = 6 });
            Assert.AreEqual(modelId, repo.Model.Id);
            Assert.AreEqual(6, repo.Model.Value.Y);
            OutputRepo(repo);
        }

        [Test]
        public static void TestAggregateRepo()
        {
            var store = new RepositoryManager();
            var repo = store.AddAggregateRepository<TestRecord>();

            Assert.AreEqual(0, repo.GetModels().Count);
            var rec1 = new TestRecord { X = 1, Y = 2 };
            var model = repo.Add(rec1);
            var modelId = model.Id;
            Assert.AreEqual(modelId, repo.GetModels()[0].Id);
            Assert.AreEqual(rec1, repo.GetModels()[0].Value);
            Assert.AreEqual(1, repo.GetModels().Count);
            OutputRepo(repo);

            var rec2 = new TestRecord { X = 3, Y = 4 };
            var model2 = repo.Add(rec2);
            var modelId2= model2.Id;
            Assert.AreNotEqual(modelId2, modelId);
            Assert.AreEqual(2, repo.GetModels().Count);

            Assert.AreEqual(rec1, repo.GetValue(modelId));
            Assert.AreEqual(rec2, repo.GetValue(modelId2));

            Assert.AreEqual(modelId, repo.GetModel(modelId).Id);
            Assert.AreEqual(modelId2, repo.GetModel(modelId2).Id);
        }

        [Test]
        public static void TestModel()
        {
            var rec1 = new TestRecord { X = 1, Y = 2 };
            var repo = new SingletonRepository<TestRecord>(rec1);
            var model = repo.Model;
            model.PropertyChanged += OnPropertyChanged;
            model.Repository.CollectionChanged += RepositoryOnCollectionChanged;
            model.Repository.RepositoryChanged += Repository_RepositoryChanged;
            dynamic dynModel = model;
            Console.WriteLine($"Original Record {rec1.X} {rec1.Y}");
            Console.WriteLine($"Model {model.Value.X} {model.Value.Y}");
            Console.WriteLine($"Dynamic model {dynModel.X} {dynModel.Y}");
            dynModel.X = 56;
            dynModel.Y = 67;

            Console.WriteLine($"Original Record {rec1.X} {rec1.Y}");
            Console.WriteLine($"Model {model.Value.X} {model.Value.Y}");
            Console.WriteLine($"Dynamic model {dynModel.X} {dynModel.Y}");
            Assert.AreEqual(new TestRecord { X = 1, Y = 2 }, rec1);
            Assert.AreEqual(new TestRecord { X = 56, Y = 67}, model.Value);
            Assert.AreEqual(56, dynModel.X);
            Assert.AreEqual(new TestRecord { X = 56, Y = 67 }, repo.Value);

            model.Value = model.Value with { X = 8, Y = 9 };
            Console.WriteLine($"Original Record {rec1.X} {rec1.Y}");
            Console.WriteLine($"Model {model.Value.X} {model.Value.Y}");
            Console.WriteLine($"Dynamic model {dynModel.X} {dynModel.Y}");
            Assert.AreEqual(new TestRecord { X = 1, Y = 2 }, rec1);
            Assert.AreEqual(new TestRecord { X = 8, Y = 9 }, model.Value);
            Assert.AreEqual(8, dynModel.X);
            Assert.AreEqual(new TestRecord { X = 8, Y = 9 }, repo.Value);

            model.Update(r => r with { X = 10, Y = 11 });
            Console.WriteLine($"Original Record {rec1.X} {rec1.Y}");
            Console.WriteLine($"Model {model.Value.X} {model.Value.Y}");
            Console.WriteLine($"Dynamic model {dynModel.X} {dynModel.Y}");
            Assert.AreEqual(new TestRecord { X = 1, Y = 2 }, rec1);
            Assert.AreEqual(new TestRecord { X = 10, Y = 11 }, model.Value);
            Assert.AreEqual(10, dynModel.X);
            Assert.AreEqual(new TestRecord { X = 10, Y = 11 }, repo.Value);


            // Console.WriteLine($"Dynamic model {dynModel.X} {dynModel.Y}");
        }

        [Test]
        public static void TestModelModified()
        {
            var rec1 = new TestRecord { X = 1, Y = 2 };
            var repo = new SingletonRepository<TestRecord>(rec1);
            var model = repo.Model;
            var propChangedCalled = false;
            model.PropertyChanged += (sender, args) =>
            {
                propChangedCalled = true;
            };
            Assert.AreEqual(1, model.Value.X);
            Assert.AreEqual(false, propChangedCalled);
            model.Value = model.Value with { X = 3 };
            Assert.AreEqual(3, model.Value.X);
            Assert.AreEqual(true, propChangedCalled);
            model.Dispose();
        }

        private static void Repository_RepositoryChanged(object? sender, RepositoryChangeArgs e)
        {
            Console.WriteLine($"Repository changed {sender}, {e.ChangeType} {e.ModelId} {e.NewValue} {e.OldValue}");
        }

        private static void RepositoryOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine($"Repository collection changed {sender}, {e?.Action}");
        }

        private static void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine($"Property changed {sender}, {e?.PropertyName}");
        }
    }
}