using Linq2Ldap.Attributes;
using Linq2Ldap.Models;

namespace Linq2Ldap.IntegrationTest.RemoteFilterEvaluation {
    public class MyMapperProfile<T>: MapperProfile<T>
        where T: class, IEntry
    {

        public MyMapperProfile(): base() {
            // Your overrides

        }
    }
}