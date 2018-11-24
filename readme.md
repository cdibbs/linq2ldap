![LINQ2LDAP][banner]
[![Build status][appveyorimg]][appveyorlink]
[![codecov][codecovimg]][codecovlink]

# Linq2Ldap.*

This project wraps [Linq2Ldap.Core][core] to facilitate LINQ-based querying of LDAP using the System.DirectoryServices
and System.DirectoryServices.Protocols libraries. The intention is to facilitate using built-in LINQ Expressions so you
don't have to use yet another metaprogramming target language in your code base.

Here are a couple examples that use Expressions to page through LDAP results.

## System.DirectoryServices Example

```c#
public IEnumerable<T> Page<T>(
    Expression<Func<T, bool>> filter,
    int offsetPage = 0,
    int pageSize = 10,
    SortOption sortOpt = null
)
    where T : Entry
{
    var searcher = new LinqDirectorySearcher<T>(DirEntry);
    searcher.SearchScope = SearchScope.Subtree;
    searcher.Filter = filter; // (.AsExpression() is implicit, here)
    searcher.VirtualListView = new DirectoryVirtualListView(
        0, pageSize - 1, pageSize * offsetPage + 1);

    // Not obvious, but VLV must have a sort option.
    searcher.Sort = sortOpt ?? new SortOption("cn", SortDirection.Ascending);
    return searcher.FindAll();
}
```

Please note that even though the `System.DirectoryServices.*` libraries aren't compatible with Mac/Linux,
you should still be able to use the LINQ Transpiler in [Linq2Ldap.Core][core] with a non-Windows LDAP library.

For more information, please visit the [Wiki](https://github.com/cdibbs/linq2ldap/wiki).

## Contributing

Do you appreciate the hard work that went into this software? We accept [donations]!

[banner]: https://github.com/cdibbs/linq2ldap/blob/master/resources/header.svg "The only way to discover the limits of the possible is to go beyond them into the impossible. - Arthur C. Clarke"
[1]: https://github.com/cdibbs/linq2ldap/blob/master/Linq2Ldap/Specification.cs#L42
[appveyorimg]: https://ci.appveyor.com/api/projects/status/i8u7bshsqw63wj7e?svg=true
[appveyorlink]: https://ci.appveyor.com/project/cdibbs/linq2ldap
[codecovimg]: https://codecov.io/gh/cdibbs/linq2ldap/branch/master/graph/badge.svg
[codecovlink]: https://codecov.io/gh/cdibbs/linq2ldap
[core]: https://github.com/cdibbs/linq2ldap.core
[wiki-dev]: https://github.com/cdibbs/linq2ldap/wiki/Development-Setup
[donations]: https://cdibbs.github.io/foss-giving
