using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.IntegrationTest.Models;
using Xunit;

namespace Linq2Ldap.IntegrationTest.LocalExpressionEvaluation
{
    public class ReadmeIntegrationTest
    {
        // From the readme
        public MyBizSpecifications Specs;
        public ReadmeIntegrationTest() {
            Specs = new MyBizSpecifications();
        }

        [Fact]
        public void ActiveUsers_ExcludesSuspended() {
            // Setup
            var users = new List<User>() {
                new User() { Id = 314, Status = "active", Suspended = "some reason" },
                new User() { Id = 444, Status = "active", Suspended = null }
            }.AsQueryable();
            var spec = Specs.ActiveUsers();

            // Run
            var subset = users.Where(spec).ToList();

            // Inspect
            Assert.Equal(users.ElementAt(1), subset.FirstOrDefault());
        }
    }
}