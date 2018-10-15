using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.DirectoryServices;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Linq2Ldap.Models;
using Linq2Ldap.Proxies;

[assembly: InternalsVisibleTo("Linq2Ldap.Tests")]
namespace Linq2Ldap {
    public class LinqDirectorySearcher<T> : DirectorySearcherProxy, ILinqDirectorySearcher<T>
        where T: class, IEntry
    {
        protected LDAPFilterCompiler FilterCompiler { get; set; }
            = new LDAPFilterCompiler();
        protected IMapper Mapper { get; set; }

        // Allows our tests to get at our code.
        internal IDirectorySearcherProxy Base;

        public LinqDirectorySearcher(IMapper mapper = null): base() {
            Setup(mapper);
        }

        public LinqDirectorySearcher(
            DirectoryEntry entry,
            IMapper mapper = null) : base(entry)
        {
            Setup(mapper);
        }

        public string RawFilter {
            get => Base.Filter;
            set => Base.Filter = value;
        }

        public new Expression<Func<T, bool>> Filter {
            internal get {
                throw new NotImplementedException(
                    "Converting from a LDAP filter back into a LINQ Expression is not currently supported.");
            }

            set {
                Base.Filter = FilterCompiler.CompileFromLinq(value);
            }
        }

        public new IEnumerable<T> FindAll() => Mapper.Map<IEnumerable<T>>(Base.FindAll());

        public new T FindOne() => Mapper.Map<T>(Base.FindOne());
        
        protected virtual void Setup(IMapper mapper) {
            Base = this;
            Mapper = mapper ?? new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile<T>());
                cfg.AddCollectionMappers();
            }).CreateMapper();
        }
    }
}