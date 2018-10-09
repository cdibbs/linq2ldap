# Linq2Ldap

This project centers around the ability to transpile C# Linq Expressions into RFC 1960 LDAP filter strings.
It intends to facilitate using the Repository and Specification patterns with LDAP for easier integrations.

If you only want to use the filter transpiler, you can do this:

```c#
    var filterCompiler = new LDAPFilterCompiler();
    var searcher = new DirectorySearcherProxy(Domain.GetDirectoryEntry());
    searcher.SearchScope = SearchScope.Subtree;
    searcher.Filter = filterCompiler.CompileFromLinq(
        (User u) => u.SamAccountName.StartsWith("will*") && u.Email.Contains("uiowa"));
    var results = new SearchResultCollectionProxy(searcher.FindAll());
```

However, this library also contains helpers for implementing better abstractions:

- Show 

# Development setup

To setup free code coverage analysis in VS Community, see this:

https://medium.com/bluekiri/code-coverage-in-vsts-with-xunit-coverlet-and-reportgenerator-be2a64cd9c2f


# Contributing

This code is free, and most of the author's time on it is free, but if you feel like contributing financially
to this open source project, please do so at ___.

