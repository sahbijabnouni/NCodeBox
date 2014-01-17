using System.Collections.Generic;

namespace NCodeBox.Domain
{
    public interface IValidator<T>
    {
        ICollection<ValidationMessage> Errors { get; set; }
        T Entity{ get; set; }
        void Init(T t);
        void Validate();
        bool IsValid { get;}
        void AddError(string error,string message);
    }
}
