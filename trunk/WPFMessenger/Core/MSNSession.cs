using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFMessenger.Core
{
    public static class MSNSession
    {

        public static MSNUser User { get; set; }

        public static void CreateUser()
        {
            User = new MSNUser();
        }

    }
}
