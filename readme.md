![LINQ2LDAP][banner]
[![Build Status][travisimg]][travislink]
[![Build status][appveyorimg]][appveyorlink]
[![codecov][codecovimg]][codecovlink]

# Linq2Ldap

This project centers around the ability to transpile C# LINQ Expressions into RFC 1960 LDAP filter strings.
It facilitates using the Repository and Specification patterns with LDAP, as well as unit testing your filter
logic.

If you only want to use the filter transpiler, you can do this:

```c#
    var searcher = new DirectorySearcherProxy(Domain.GetDirectoryEntry());
    searcher.SearchScope = SearchScope.Subtree;

    // CompileFromLinq returns an RFC1960 filter string
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

Beyond the filter transpiler, this library also contains an implementation of the Specification pattern
to wrap your Expressions and improve code reuse and testability. It does so by facilitating the
[otherwise abstruse][1] ability to glue your Expressions together with And and Or.

```csharp
public class MyBizSpecifications {
    public virtual ISpecification<User> ActiveUsers() {
        return Specification<User>.Start(
            u =>     u.Status == "active"
                && ! u.Suspended.Matches("*") /* not exists */
        );
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

## Testability

Drawing from the example, above, if you want to unit test your filter logic, you can now write tests like this:

```csharp
public class MyBizSpecificationsTests {
    public MyBizSpecifications Specs;
    public MyBizSpecificationsTests() {
        Specs = new MyBizSpecifications();
    }

    [Fact]
    public void ActiveUsers_ExcludesSuspended() {
        // Setup
        var users = new List<User>() {
            new User() { Id = 444, Status = "active", Suspended = "some reason" },
            new User() { Id = 314, Status = "active", Suspended = null }
        };
        var spec = Specs.ActiveUsers();

        // Run
        var subset = users.Where(spec.AsExpression().Compile()).ToList();

        // Inspect
        Assert.Equal(users.ElementAt(1), subset.FirstOrDefault());
    }
}
```

The alternative is either integration testing, which can be slow and hard to implement, or--worse--manual testing,
which is like trying to maintain a sandcastle in the tide.

# Development setup

To setup free code coverage analysis in VS Community, see this:

https://medium.com/bluekiri/code-coverage-in-vsts-with-xunit-coverlet-and-reportgenerator-be2a64cd9c2f

If not using Visual Studio Code, please see .vscode/tasks.json for examples to run the build and tests.

[banner]: https://github.com/cdibbs/linq2ldap/blob/master/resources/header.svg "The only way to discover the limits of the possible is to go beyond them into the impossible. - Arthur C. Clarke"
[1]: https://github.com/cdibbs/linq2ldap/blob/master/Linq2Ldap/Specification.cs#L42
[travisimg]: https://travis-ci.org/cdibbs/linq2ldap.svg?branch=master
[travislink]: https://travis-ci.org/cdibbs/linq2ldap
[appveyorimg]: https://ci.appveyor.com/api/projects/status/i8u7bshsqw63wj7e?svg=true
[appveyorlink]: https://ci.appveyor.com/project/cdibbs/linq2ldap
[codecovimg]: https://codecov.io/gh/cdibbs/linq2ldap/branch/master/graph/badge.svg
[codecovlink]: https://codecov.io/gh/cdibbs/linq2ldap