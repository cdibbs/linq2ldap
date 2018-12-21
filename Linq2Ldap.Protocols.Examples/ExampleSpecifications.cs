using Linq2Ldap.Examples.Models;
using Linq2Ldap.Core.ExtensionMethods;
using Linq2Ldap.ExampleCommon.Specifications;

namespace Linq2Ldap.Examples
{
    public class ExampleSpecifications {
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
    }
}