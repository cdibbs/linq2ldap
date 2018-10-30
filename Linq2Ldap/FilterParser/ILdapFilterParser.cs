using System;
using System.Linq.Expressions;
using Linq2Ldap.Models;

namespace Linq2Ldap.FilterParser {
    public interface ILdapFilterParser {
        Expression<Func<T, bool>> Parse<T>(string filter)
            where T: IEntry;
    }
}