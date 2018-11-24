using System.DirectoryServices;

namespace Linq2Ldap.Examples.Repository
{
    public class LdapRepositoryFactory
    {
        public LdapRepositoryFactory() {}

        public LdapRepository Build()
        {
            return new LdapRepository();
        }

        public LdapRepository Build(DirectoryEntry entry)
        {
            return new LdapRepository(entry);
        }
    }
}
