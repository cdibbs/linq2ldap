using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using Linq2Ldap.Attributes;
using Linq2Ldap.Models;
using Linq2Ldap.Proxies;
using Linq2Ldap.Types;

namespace Linq2Ldap
{
    public class MapperProfile<T>: Profile
        where T: IEntry
    {
        public MapperProfile()
        {
            var bsa = CreateMap<SearchResultProxy, T>();
            bsa.ForMember(b => b.DN, o => o.MapFrom(s => s.Path));
            bsa.ForMember(b => b.Properties, o => o.MapFrom(s => s.Properties));
            bsa.AfterMap(this.Convert);
        }

        internal void Convert(SearchResultProxy srp, T model)
        {
            //model = srp.Properties["samaccountname"]?[0] as string;
            var t = typeof(T);
            var props = t.GetProperties();
            foreach (var prop in props)
            {
                SetPropFromProperties(srp, model, prop);
            }
        }

        private void SetPropFromProperties(SearchResultProxy srp, T model, PropertyInfo prop)
        {
            var attr = prop.GetCustomAttribute<LDAPFieldAttribute>();
            if (attr == null)
            {
                return;
            }

            var ldapName = attr?.Name ?? prop.Name;
            var val = srp.Properties[ldapName];
            if (prop.CanWrite)
            {
                ValidateTypeConvertAndSet(model, val, prop, ldapName);
            }
            else
            {
                throw new ArgumentException(
                    $"Column attribute {attr.Name} applied to property {prop.Name}, "
                    + $"but {prop.Name} is not writable.");
            }
        }

        private void ValidateTypeConvertAndSet(
            T model,
            ResultPropertyValueCollectionProxy ldapData,
            PropertyInfo prop,
            string ldapName)
        {
            var ptype = prop.PropertyType;
            if (ptype == typeof(LDAPStringList))
            {
                var converted = ldapData
                    .Select(e => e is Byte[] b
                        ? System.Text.Encoding.UTF8.GetString(b, 0, b.Length)
                        : e.ToString())
                    .ToArray();
                prop.SetValue(model, new LDAPStringList(converted));
                return;
            }

            if (ldapData.Count == 1)
            {
                //AssertValidCast(ptype, ldapData[0], ldapName, prop.Name);
                prop.SetValue(model, ldapData[0]);
                return;
            }

            throw new FormatException(
                $"Mapping to non-array type, but LDAP data is array: {ldapName} -> {prop.Name}.");
        }
    }
}
