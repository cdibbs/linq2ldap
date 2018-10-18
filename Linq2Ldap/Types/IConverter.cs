using Linq2Ldap.Proxies;

namespace Linq2Ldap.Types {
    public interface IConverter<T> {
        T Convert(ResultPropertyValueCollectionProxy values);
    }
}