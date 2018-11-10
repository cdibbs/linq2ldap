using System;
using System.Collections.Generic;
using System.Linq;
using Linq2Ldap.Core.Proxies;

namespace Linq2Ldap.Core.Types {
    public class StringListConverter : IConverter<List<string>>
    {
        public List<string> Convert(PropertyValueCollection values)
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