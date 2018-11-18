using Linq2Ldap.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linq2Ldap.Protocols
{
    public interface ILinq2LdapConnection
    {
        LinqSearchResponse<T> SendRequest<T>(LinqSearchRequest<T> request)
            where T : Entry, new();
    }
}
