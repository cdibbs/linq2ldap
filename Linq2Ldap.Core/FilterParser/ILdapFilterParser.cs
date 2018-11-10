using System;
using System.Linq.Expressions;
using Linq2Ldap.Core.Models;

namespace Linq2Ldap.Core.FilterParser {
    public interface ILdapFilterParser {
        Expression<Func<T, bool>> Parse<T>(string filter)
            where T: IEntry;
    }
}