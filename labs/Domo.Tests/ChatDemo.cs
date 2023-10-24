using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Ara3D.Domo.Tests
{
    // These specifically are classes that represent value and entities in the 
    // problem (aka business) domain. 
    #region Models

    public record struct PhoneNumber(
        string Number);

    public record struct Message(
        Guid ChatId,
        PhoneNumber Sender, 
        string Text, 
        DateTimeOffset Composed,
        DateTimeOffset Sent, 
        DateTimeOffset Recieved);

    public record struct Contact(
        string DisplayName, 
        IReadOnlyList<PhoneNumber> Numbers);

    public record struct Chat(
        IReadOnlyList<PhoneNumber> Participants);

    #endregion

    // These are helper extension functions that add functionality to various models and repositories

    #region Model Extension Functions

    public static class PhoneNumberExtensions
    {
        public static string ToValidPhoneNumberChars(string input)
            => Regex.Replace(input, "^[0-9-]", "");

        public static PhoneNumber ToPhoneNumber(this string input)
            => new(ToValidPhoneNumberChars(input));
    }

    public static class ContactExtensions
    {
        public static bool UpdateDisplayName(this IModel<Contact> contact, string displayName = null)
            => contact.Update(x => x with { DisplayName = displayName });

        public static PhoneNumber FirstNumber(this IModel<Contact> contact)
            => contact.Value.Numbers[0];

        public static bool AddNumber(this IModel<Contact> contact, PhoneNumber number)
            => contact.Update(x => x with { Numbers = x.Numbers.Append(number).ToArray() });

        public static bool AddNumbers(this IModel<Contact> contact, IEnumerable<PhoneNumber> numbers)
            => contact.Update(x => x with { Numbers = x.Numbers.Concat(numbers).ToArray() });

        public static bool AddNumbers(this IModel<Contact> contact, params string[] number)
            => contact.AddNumbers(number.Select(n => n.ToPhoneNumber()));

        public static IModel<Contact> FindContact(this IRepository<Contact> repo, PhoneNumber number)
            => repo.GetModels().FirstOrDefault(x => x.Value.Numbers.Contains(number));

        public static IModel<Contact> AddContact(this IRepository<Contact> repo)
            => repo.Add(new Contact("", Array.Empty<PhoneNumber>()));

        public static IModel<Contact> AddContact(this IRepository<Contact> repo, string name,
            params string[] phoneNumbers)
        {
            var contact = repo.AddContact();
            contact.UpdateDisplayName(name);
            contact.AddNumbers(phoneNumbers);
            return contact;
        }

        public static void DeleteContact(this IModel<Contact> model)
            => model.Repository.Delete(model.Id);
    }

    public static class ChatExtensions
    {
        public static IModel<Chat> StartChat(this IRepository<Chat> repo, params PhoneNumber[] participants)
            => repo.Add(new Chat(participants));

        public static IModel<Chat> StartChat(this IRepository<Chat> repo, params IModel<Contact>[] participants)
            => repo.StartChat(participants.Select(p => p.FirstNumber()).ToArray());

        public static IModel<Message> CreateMessage(this IRepository<Message> repo, 
            PhoneNumber sender, string text)
            => repo.Add(new Message(Guid.Empty, sender, text, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now));

        public static bool SendMessage(this IModel<Chat> chat, IModel<Message> message)
            => message.Update(x => x with { Sent = DateTimeOffset.Now, ChatId = chat.Id });
    }

    #endregion

    public class ChatDemo
    {
        public IRepositoryManager Store = new RepositoryManager();

        public IAggregateRepository<Message> Messages { get; }
        public IAggregateRepository<Chat> Chats { get; }
        public IAggregateRepository<Contact> Contacts { get; }

        public ChatDemo()
        {
            Messages = Store.AddAggregateRepository<Message>();
            Chats = Store.AddAggregateRepository<Chat>();
            Contacts = Store.AddAggregateRepository<Contact>();
        }

        [Test]
        public void ChatDemoTest()
        {
            var john = Contacts.AddContact("John", "555-1234");
            var paul = Contacts.AddContact("Paul", "555-2345");
            var ringo = Contacts.AddContact("Ringo", "555-4321");
            var george = Contacts.AddContact("George", "555-6789");

            var chat = Chats.StartChat(john, paul, george);
            chat.SendMessage(Messages.CreateMessage(george.FirstNumber(), "Hey guys should we tell Ringo?"));
            chat.SendMessage(Messages.CreateMessage(paul.FirstNumber(), "Nah, he'll just ruin it"));
        }
    }
}
