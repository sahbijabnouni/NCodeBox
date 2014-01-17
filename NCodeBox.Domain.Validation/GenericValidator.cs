using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NCodeBox.Domain.Validation
{
    public class GenericValidator<T> : IValidator<T>
    {

        public ICollection<ValidationMessage> Errors { get; set; }
        public T Entity { get; set; }
        public void Init(T t){}
        public void Validate(){}
        private PropertyInfo _propertyInfo;
        private object _value;
        private bool _isValid;
        public bool IsValid { get { return Errors.Count == 0; }  }
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
                _isValid = _value.ToString().Length > nbr;
            }
            return this;
        }
        public GenericValidator<T> MinLength(int nbr)
        {
            if (_value is string)
            {
                _isValid = _value.ToString().Length < nbr;
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
    }
}
