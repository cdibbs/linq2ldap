using Linq2Ldap.IntegrationTest.Models;
using Linq2Ldap.ExtensionMethods;
using Linq2Ldap.Specifications;

namespace Linq2Ldap.IntegrationTest.LocalExpressionEvaluation {
    // The example from the readme
    public class MyBizSpecifications {
        public virtual Specification<User> ActiveUsers() {
            return Specification<User>.Start(
                u =>     u.Status == "active"
                    && ! u.Suspended.Matches("*") /* not exists */
            );
        }

        public virtual Specification<User> UsersInCountry(string country) {
            return Specification<User>.Start(u => u.Country == country);
        }

        public virtual Specification<User> ActiveUsersInCountry(string country) {
            return ActiveUsers().And(UsersInCountry(country));
        }

        // ...
    }
}