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

## System.DirectoryServices.Protocols Example

```c#
public IEnumerable<T> Page<T>(
	Specification<T> spec,
	int offsetPage = 0, int pageSize = 10,
	SortKey[] sortKeys = null)
	where T : IEntry, new()
{
	LinqSearchResponse<T> result = null;
	int curPage = 0;
	while (curPage++ <= offsetPage)
	{
		var search = new LinqSearchRequest<T>(DistinguishedName, spec.AsExpression(), Scope);
		var pageControl = new PageResultRequestControl(pageSize);
		var soc = new SearchOptionsControl(SearchOption.DomainScope);
		search.Controls.Add(pageControl);
		search.Controls.Add(soc);
		if (sortKeys != null)
		{
			var sortControl = new SortRequestControl(sortKeys);
			search.Controls.Add(sortControl);
		}
		result = Connection.SendRequest(search);
	}

	return result?.Entries;
}
```

Please note that, at the time of writing, the `System.DirectoryServices.*` libraries and, therefore, this
library's abstraction layer, are not compatible with Mac/Linux. You should still be able to use the LINQ
Transpiler in [Linq2Ldap.Core][core] with a non-Windows LDAP library, however, since RFC 1960 filters are cross-platform.

For more information, please visit the [Wiki](https://github.com/cdibbs/linq2ldap/wiki).

# Development setup

## Code coverage 

To setup free code coverage analysis in VS Code, see this:

https://medium.com/bluekiri/code-coverage-in-vsts-with-xunit-coverlet-and-reportgenerator-be2a64cd9c2f

If not using Visual Studio Code, please see .vscode/tasks.json for examples to run the build and tests.

## End-to-end testing

In VS Code, open a terminal, then launch a second pane (the icon to the right of the plus sign).
In that pane:

```bash
cd e2e-helpers
npm install
npm run serve
```

After the LDAP test server is running, you can use the other pane to run integration tests.

```
dotnet test .\Linq2Ldap.IntegrationTest\
```

[banner]: https://github.com/cdibbs/linq2ldap/blob/master/resources/header.svg "The only way to discover the limits of the possible is to go beyond them into the impossible. - Arthur C. Clarke"
[1]: https://github.com/cdibbs/linq2ldap/blob/master/Linq2Ldap/Specification.cs#L42
[appveyorimg]: https://ci.appveyor.com/api/projects/status/i8u7bshsqw63wj7e?svg=true
[appveyorlink]: https://ci.appveyor.com/project/cdibbs/linq2ldap
[codecovimg]: https://codecov.io/gh/cdibbs/linq2ldap/branch/master/graph/badge.svg
[codecovlink]: https://codecov.io/gh/cdibbs/linq2ldap
[core]: https://github.com/cdibbs/linq2ldap.core