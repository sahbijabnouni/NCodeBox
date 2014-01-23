using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NCodeBox.Domain.Validation
{
    public class GenericValidator<T> : IValidator<T>
    {

        public ICollection<ValidationMessage> Errors { get; set; }
        public T Entity { get; set; }
        public void Init(T t) { }
        public void Validate() { }
        private PropertyInfo _propertyInfo;
        private object _value;
        private bool _isValid;
        public bool IsValid { get { return Errors.Count == 0; } }
        public GenericValidator<T> For(Expression<Func<T, object>> property)
        {
            _propertyInfo = ExpressionExtensions<T>.GetProperty(property);
            _value = _propertyInfo.GetValue(Entity);
            return this;
        }
        public GenericValidator<T> Required()
        {
            if (_value is string)
            {
                _isValid = !string.IsNullOrEmpty(_value.ToString());
            }
            else
            {
                _isValid = _value != null;
            }
            return this;
        }
        public GenericValidator<T> MaxLength(int nbr)
        {
            if (_value is string)
            {
                _isValid = _value.ToString().Length <= nbr;
            }
            return this;
        }
        public GenericValidator<T> MinLength(int nbr)
        {
            if (_value is string)
            {
                _isValid = _value.ToString().Length >= nbr;
            }
            return this;
        }
        public GenericValidator<T> Range(dynamic min, dynamic max)
        {
            if (IsNumericType(_value.GetType()) && _value.GetType() == min.GetType() && _value.GetType() == max.GetType())
            {
                _isValid = _value >= min && _value <= max;
            }
            return this;
        }
        public GenericValidator<T> Email(dynamic min, dynamic max)
        {
            if (_value is string)
            {
                _isValid = IsValidEmail(_value.ToString());
            }
            return this;
        }
        public GenericValidator<T> Url(dynamic min, dynamic max)
        {
            if (IsNumericType(_value.GetType()) && _value.GetType() == min.GetType() && _value.GetType() == max.GetType())
            {
                _isValid = _value >= min && _value <= max;
            }
            return this;
        }
        public GenericValidator<T> AddMessage(string message)
        {
            if (!_isValid)
            {
                AddError(_propertyInfo.Name, message);
            }
            return this;
        }
        public void AddError(string key, string errorMessage)
        {
            var error = Errors.FirstOrDefault(s => s.Key == key);
            if (error != null)
            {
                if (error.Message == null) error.Message = new List<string>();
                error.Message.Add(errorMessage);
            }
            else
            {
                Errors.Add(new ValidationMessage() { Key = key, Message = new List<string>() { errorMessage } });
            }
        }
        public GenericValidator<T> Should<TP>(Func<TP, bool> func)
        {
            _isValid = func((TP)_value);
            return this;
        }
        private bool IsNumericType(Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);
            return (int)typeCode >= 5 && (int)typeCode <= 15;
        }
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
