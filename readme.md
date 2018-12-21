![LINQ2LDAP][banner]
[![MIT License][license-badge]][LICENSE]
[![Build status][appveyorimg]][appveyorlink]
[![codecov][codecovimg]][codecovlink]
[![Code of Conduct][coc-badge]][coc]

# Linq2Ldap.*

This project wraps [Linq2Ldap.Core][core] to facilitate LINQ-based querying of LDAP using the System.DirectoryServices
and System.DirectoryServices.Protocols libraries. The intention is to facilitate using built-in LINQ Expressions so you
don't have to use yet another metaprogramming target language in your code base.

Here is an examples that takes Expressions (wrapped in Specifications) to page through LDAP results.

## System.DirectoryServices Example

```c#
public virtual IEnumerable<T> PageWithVLV<T>(
    Specification<T> spec,
    int offsetPage = 0, int pageSize = 10,
    SortKey[] sortKeys = null
)
    where T : IEntry, new()
{
    var search = new LinqSearchRequest<T>(DistinguishedName, spec.AsExpression(), Scope);
    var pageControl = new VlvRequestControl(0, pageSize - 1, pageSize * offsetPage + 1);
    var soc = new SearchOptionsControl(SearchOption.DomainScope);
    search.Controls.Add(pageControl);
    search.Controls.Add(soc);
    if (sortKeys != null)
    {
        var sortControl = new SortRequestControl(sortKeys);
        search.Controls.Add(sortControl);
    }
    return Connection.SendRequest(search).Entries;
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
[coc-badge]: https://img.shields.io/badge/code%20of-conduct-ff69b4.svg?style=flat-square
[coc]: https://github.com/cdibbs/linq2ldap/blob/master/code_of_conduct.md
[license-badge]: https://img.shields.io/badge/license-MIT-blue.svg
[LICENSE]: https://github.com/ossplz/alsatian-fluent-assertions/blob/master/LICENSE
