using Linq2Ldap.Core.Proxies;

namespace Linq2Ldap.Core.Types {
    public interface IConverter<T> {
        T Convert(PropertyValueCollection values);
    }
}