﻿using Linq2Ldap.Core.Models;
using Linq2Ldap.ExampleCommon.Specifications;
using System.Collections.Generic;
using System.DirectoryServices;

namespace Linq2Ldap.Examples.Repository
{
    public interface ILdapRepository
    {
        T FindOne<T>(Specification<T> spec) where T: IEntry, new();
        T[] FindAll<T>(Specification<T> spec) where T: IEntry, new();
        IEnumerable<T> Page<T>(
            Specification<T> spec,
            int offsetPage = 0, int pageSize = 10,
            SortOption sortOpt = null)
            where T : IEntry, new();
        void Add<T>(T entity) where T: IEntry, new();
        void Update<T>(T entity) where T: IEntry, new();
    }
}