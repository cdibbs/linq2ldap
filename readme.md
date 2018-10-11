![LINQ2LDAP][banner]

# Linq2Ldap

This project centers around the ability to transpile C# Linq Expressions into RFC 1960 LDAP filter strings.
It intends to facilitate using the Repository and Specification patterns with LDAP for easier integrations.

If you only want to use the filter transpiler, you can do this:

```c#
    var searcher = new DirectorySearcherProxy(Domain.GetDirectoryEntry());
    searcher.SearchScope = SearchScope.Subtree;

    // 
    searcher.Filter = new LDAPFilterCompiler().CompileFromLinq(
        (User u) => u.SamAccountName.StartsWith("will") && u.Email.Contains("uiowa"));

    var results = new SearchResultCollectionProxy(searcher.FindAll());
```

Also supported:
```c#
(User u) => u.Title.Matches("univ*of*iowa"); // (title=univ*of*iowa)
(User u) => u.Email.EndsWith("@gmail.com"); // (mail=*@gmail.com)
(User u) => u.Properties["acustomproperty"] == "some val"; // (acustomproperty=some val)
(User u) => u.Properties.Has("somekey"); // (somekey=*)
```

However, this library also contains helpers for implementing better abstractions.

# Testability

## Expressions & Specifications
Instead of testing whether an Expression calls a particular string method, for example, testing should
focus on breaking Expressions up into testable, constituent parts. The Specification pattern facilitates
using boolean operations with the constituents, and testing can be applied by mocking virtual Specification
methods. [Example]

# Development setup

To setup free code coverage analysis in VS Community, see this:

https://medium.com/bluekiri/code-coverage-in-vsts-with-xunit-coverlet-and-reportgenerator-be2a64cd9c2f


# Contributing

This code is free, and most of the author's time on it is free, but if you feel like contributing financially
to this open source project, please do so at ___.

[banner]: https://github.com/cdibbs/linq2ldap/blob/master/resources/header.svg "The only way to discover the limits of the possible is to go beyond them into the impossible. - Arthur C. Clarke"