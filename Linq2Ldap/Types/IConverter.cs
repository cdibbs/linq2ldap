using Linq2Ldap.Proxies;

public interface IConverter<T> {
    T Convert(ResultPropertyValueCollectionProxy values);
}