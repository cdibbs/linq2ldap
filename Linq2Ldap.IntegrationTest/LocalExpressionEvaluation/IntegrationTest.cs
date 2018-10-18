using System;
using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.IntegrationTest.Models;
using Xunit;

namespace Linq2Ldap.IntegrationTest.LocalExpressionEvaluation
{
    public class IntegrationTest
    {
        // From the readme
        public MyBizSpecifications Specs;
        public IntegrationTest() {
            Specs = new MyBizSpecifications();
        }

        [Fact]
        public void PropValues_Null_Accepts() {
            // Setup
            var users = new List<User>() {
                new User() { Id = 314, Status = "active", Suspended = "some reason" },
                new User() { Id = 444, Status = "active", Suspended = null }
            }.AsQueryable();

            // Run
            var subset = users.Where(u => u.Suspended == null).ToList();

            // Inspect
            Assert.Equal(users.ElementAt(1), subset.FirstOrDefault());
        }

        [Fact]
        public void PropValues_SearchInts() {
            // Setup
            var users = new List<User>() {
                new User() { Id = 314, Status = "active", Suspended = "some reason" },
                new User() { Id = 444, Status = "active", Suspended = null }
            }.AsQueryable();

            // Run
            var subset = users.Where(u => u.Id == 314).ToList();

            // Inspect
            Assert.Equal(users.FirstOrDefault(), subset.Single());
        }
    }
}