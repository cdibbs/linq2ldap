![LINQ2LDAP][banner]
[![MIT License][license-badge]][LICENSE]
[![Build status][appveyorimg]][appveyorlink]
[![codecov][codecovimg]][codecovlink]
[![Code of Conduct][coc-badge]][coc]

# Linq2Ldap.*

This is a thin wrapper library for System.DirectoryServices.* that facilitates using LINQ Expressions to represent LDAP filters.
It uses [Linq2Ldap.Core][core] to compile and parse LDAP filters.

Here is an example that takes an Expression provides a page of results.

## System.DirectoryServices.Protocols Example

```c#
public virtual IEnumerable<T> PageWithVLV<T>(
    Expression<Func<T, bool>> filter,
    int offsetPage = 0, int pageSize = 10,
    SortKey[] sortKeys = null
)
    where T : IEntry, new()
{
    var search = new LinqSearchRequest<T>(DistinguishedName, filter, Scope);
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
you can still use [Linq2Ldap.Core][core] with a non-Windows LDAP library.

For more information, please visit the [Wiki](https://github.com/cdibbs/linq2ldap/wiki).

## Contributing

| Coin     | Address                                     
|----------|---------------------------------------------
| Ethereum | 0xfCdA80Be00F8907FfcD227683D9D96f7C47eC67f  
| Bitcoin  | 33pypS6oRmgmvMwAnX5rpJAaxnqScxSALS          

<form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_top">
<input type="hidden" name="cmd" value="_s-xclick" />
<input type="hidden" name="hosted_button_id" value="JC7R9TLHQNDAU" />
<input type="image" src="https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif" border="0" name="submit" title="PayPal - The safer, easier way to pay online!" alt="Donate with PayPal button" />
<img alt="" border="0" src="https://www.paypal.com/en_US/i/scr/pixel.gif" width="1" height="1" />
</form>

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
