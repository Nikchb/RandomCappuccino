using RandomCappuccino.Server.Services.IdentityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCappuccino.Tests.InterfaceImplementations
{
    class TestIdentityManager : IIdentityManager
    {
        public string UserId { get; private set; }

        public TestIdentityManager(string userId)
        {
            UserId = userId;
        }
    }
}
