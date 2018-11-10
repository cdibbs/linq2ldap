using Linq2Ldap.Core.Models;
using Linq2Ldap.Core.Specifications;

namespace Linq2Ldap.Examples.Repository
{
    public interface ILDAPRepository
    {
        T FindOne<T>(Specification<T> spec) where T: IEntry, new();
        T[] FindAll<T>(Specification<T> spec) where T: IEntry, new();
        void Add<T>(T entity) where T: IEntry, new();
        void Update<T>(T entity) where T: IEntry, new();
    }
}