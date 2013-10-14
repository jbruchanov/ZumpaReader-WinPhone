using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZumpaReader.Model
{
    public interface Credentials
    {
        string Login { get; }
        string Password { get; set; }
        bool IsLoggedIn { get; set; }
    }
}
