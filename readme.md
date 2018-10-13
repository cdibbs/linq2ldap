![LINQ2LDAP][banner]
[![Build Status][travisimg]][travislink]

# Linq2Ldap

This project centers around the ability to transpile C# Linq Expressions into RFC 1960 LDAP filter strings.
It facilitates using the Repository and Specification patterns with LDAP.

If you only want to use the filter transpiler, you can do this:

```c#
    var searcher = new DirectorySearcherProxy(Domain.GetDirectoryEntry());
    searcher.SearchScope = SearchScope.Subtree;

    // 
    searcher.Filter = new LDAPFilterCompiler().CompileFromLinq(
        (MyUser u) => u.SamAccountName.StartsWith("will")
                    && u.Email.Contains("uiowa")
                    && u["customprop"] != "123");

    var results = new SearchResultCollectionProxy(searcher.FindAll());
```

Also supported examples:

```c#
(User u) => u.Title.Matches("univ*of*iowa"); // (title=univ*of*iowa)
(User u) => u.Email.EndsWith("@gmail.com"); // (mail=*@gmail.com)
(User u) => u["acustomproperty"].Contains("some val"); // (acustomproperty=some val)
(User u) => u.Has("somekey"); // (somekey=*)
```

## Expression reusability

This library also contains an implementation of the Specification pattern to wrap your Expressions
and improve code reuse and testability. It does so by facilitating the [otherwise abstruse][1] ability
to glue your Expressions together with And and Or.

```csharp
public class MyBizSpecifications {
    public virtual ISpecification<User> ActiveUsers() {
        return Specification<User>.Start(u => u.Active && ! u.Suspended);
    }

    public virtual ISpecification<User> UsersInCountry(string country) {
        return Specification<User>.Start(u => u.Country == country);
    }

    public virtual ISpecification<User> ActiveUsersInCountry(string country) {
        return ActiveUsers().And(UsersInCountry(country));
    }

    // ...
}
```

# Testability

## Expressions & Specifications
Instead of testing whether an Expression calls a particular string method, for example, testing should
focus on breaking Expressions up into testable, constituent parts. The Specification pattern facilitates
using boolean operations with the constituents, and testing can be applied by mocking virtual Specification
methods. [Example]

# Development setup

To setup free code coverage analysis in VS Community, see this:

https://medium.com/bluekiri/code-coverage-in-vsts-with-xunit-coverlet-and-reportgenerator-be2a64cd9c2f



[banner]: https://github.com/cdibbs/linq2ldap/blob/master/resources/header.svg "The only way to discover the limits of the possible is to go beyond them into the impossible. - Arthur C. Clarke"
[1]: https://github.com/cdibbs/linq2ldap/blob/master/Linq2Ldap/Specification.cs#L42
[travisimg]: https://travis-ci.org/cdibbs/linq2ldap.svg?branch=master
[travislink]: https://travis-ci.org/cdibbs/linq2ldap