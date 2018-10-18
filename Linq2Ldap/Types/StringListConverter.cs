using System;
using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.Proxies;

namespace Linq2Ldap.Types {
    public class StringListConverter : IConverter<List<string>>
    {
        public List<string> Convert(ResultPropertyValueCollectionProxy values)
        {
            return values == null
                ? null
                : values.Select(e => e is Byte[] b
                    ? System.Text.Encoding.UTF8.GetString(b, 0, b.Length)
                    : e.ToString())
                .ToList();
        }
    }
}