using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Services.UserService
{
    public interface IUserService
    {
        bool IsExists(string value);
    }
}
