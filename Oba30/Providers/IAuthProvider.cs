using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oba30.Providers
{
    public interface IAuthProvider
    {
        bool IsLoggiedIn { get; }
        bool Login(string username, string password);
        void Logout();
    }
}
