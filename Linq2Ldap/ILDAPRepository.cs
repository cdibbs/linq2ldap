using Specifications;

namespace Linq2Ldap
{
    public interface ILDAPRepository
    {
        T FindOne<T>(ISpecification<T> spec);
        T[] FindAll<T>(ISpecification<T> spec);
        void Add<T>(T entity);
        void Update<T>(T entity);
    }
}