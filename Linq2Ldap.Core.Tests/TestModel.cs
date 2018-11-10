using System;
using System.Collections.Generic;
using System.Text;

namespace Linq2Ldap.Core.Tests
{
    public class TestModel
    {
        public int Id { get; set; }

        public bool TestMethod(int input) => input % 2 == 0;
    }
}