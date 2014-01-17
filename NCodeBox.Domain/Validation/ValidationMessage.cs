using System.Collections.Generic;

namespace NCodeBox.Domain
{
    public class ValidationMessage
    {
        public string Key { get; set; }
        public ICollection<string> Message { get; set; }
    }
}
