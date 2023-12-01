using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Ara3D.Domo
{
    public sealed class Model<TValue> : DynamicObject, IModel<TValue>
    {
        public Model(Guid id, IRepository<TValue> repo)
        {
            (Id, Repository) = (id, repo);
        }

        static Model()
        {
            _cloneMethod = typeof(TValue).GetMethod("MemberwiseClone", 
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        }

        private static readonly MethodInfo _cloneMethod;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            PropertyChanged = null;
        }

        public Guid Id { get; }

        public TValue Value
        {
            get => Repository.GetValue(Id);
            set => Repository.Update(Id, _ => value);
        }

        public object CloneValue()
            => _cloneMethod.Invoke(Value, Array.Empty<object>());

        public bool HasProperty(string name)
            => ValueType.GetProperty(name) != null;

        public void SetPropertyValue(string name, object value)
        {
            var newState = CloneValue();
            var prop = ValueType.GetProperty(name);
            if (prop == null)
            {
                throw new ArgumentException(name);
            }
            prop.SetValue(newState, value);
            Value = (TValue)newState;
        }

        public object GetPropertyValue(string name)
        {
            var prop = ValueType.GetProperty(name);
            if (prop == null)
            {
                throw new ArgumentException(name);
            }

            return prop.GetValue(Value);
        }

        object IModel.Value
        {
            get => Value;
            set => Value = (TValue)value;
        }

        public Type ValueType
            => typeof(TValue);

        public IRepository<TValue> Repository { get; }

        IRepository IModel.Repository
            => Repository;

        public void TriggerChangeNotification()
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

        public AttributeCollection GetAttributes()
            => null;

        public string GetClassName()
            => TypeDescriptor.GetClassName(ValueType);

        public string GetComponentName()
            => TypeDescriptor.GetComponentName(ValueType);

        public TypeConverter GetConverter()
            => TypeDescriptor.GetConverter(ValueType);

        public EventDescriptor GetDefaultEvent()
            => null;

        public PropertyDescriptor GetDefaultProperty()
            => null;

        public object GetEditor(Type editorBaseType)
            => TypeDescriptor.GetEditor(ValueType, editorBaseType);

        public EventDescriptorCollection GetEvents()
            => null;

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
            => null;

        public PropertyDescriptorCollection GetProperties()
            // ReSharper disable once CoVariantArrayConversion
            => new PropertyDescriptorCollection(ValueType.GetProperties().Select(pi =>
                new ModelPropertyDescriptor(pi)).ToArray());

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            => GetProperties();

        public object GetPropertyOwner(PropertyDescriptor pd)
            => this;

        public override IEnumerable<string> GetDynamicMemberNames()
            => GetProperties().OfType<PropertyDescriptor>().Select(pi => pi.Name);

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == ValueType)
            {
                result = Value;
                return true;
            }

            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!HasProperty(binder.Name))
                return false;
            SetPropertyValue(binder.Name, value);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!HasProperty(binder.Name))
            {
                result = null;
                return false;
            }

            result = GetPropertyValue(binder.Name);
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder.Name == "ToString" && args.Length == 0)
            {
                result = Value.ToString();
                return true;
            }

            if (binder.Name == "Equals" && args.Length == 1)
            {
                result = Value.Equals(args[0]);
                return true;
            }

            if (binder.Name == "GetHashCode" && args.Length == 0)
            {
                result = Value.GetHashCode();
                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }
        
        public class ModelPropertyDescriptor : PropertyDescriptor
        {
            public PropertyInfo PropertyInfo;

            public ModelPropertyDescriptor(PropertyInfo pi)
                : base(pi.Name, Array.Empty<Attribute>())
                => PropertyInfo = pi;

            public override bool CanResetValue(object component)
                => false;

            public override object GetValue(object component)
                => PropertyInfo.GetValue(((Model<TValue>)component).Value);

            public override void ResetValue(object component)
                => throw new NotImplementedException();

            public override void SetValue(object component, object value)
                => ((Model<TValue>)component).SetPropertyValue(PropertyInfo.Name, value);

            public override bool ShouldSerializeValue(object component)
                => true;

            public override Type ComponentType
                => typeof(Model<TValue>);

            public override bool IsReadOnly
                => !PropertyInfo.CanWrite;

            public override Type PropertyType
                => PropertyInfo.PropertyType;
        }
    }
}