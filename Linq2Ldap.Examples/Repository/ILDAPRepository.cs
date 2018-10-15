using Linq2Ldap.Models;
using Specifications;

namespace Linq2Ldap.Examples.Repository
{
    public interface ILDAPRepository
    {
        T FindOne<T>(Specification<T> spec) where T: class, IEntry;
        T[] FindAll<T>(Specification<T> spec) where T: class, IEntry;
        void Add<T>(T entity) where T: class, IEntry;
        void Update<T>(T entity) where T: class, IEntry;
    }
}