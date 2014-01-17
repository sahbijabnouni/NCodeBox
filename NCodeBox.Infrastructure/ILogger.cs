using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLess.Infrastructure
{
    public interface ILogger
    {
        void AddError(string message);
    }
}
