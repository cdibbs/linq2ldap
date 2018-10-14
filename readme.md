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
    // CompileFromLinq returns an RFC1960 filter string
    string filter = new LDAPFilterCompiler().CompileFromLinq(
        (MyUserModel u) => u.SamAccountName.StartsWith("will")
                    && u.Email.Contains("uiowa")
                    && u["customprop"] != "123");

    var searcher = new DirectorySearcher();
    searcher.Filter = filter;

    // -- or --

    var searchReq = new SearchRequest(targetOu, filter, /* ... */);
```

Also supported examples:

```c#
(User u) => u.Title.Matches("univ*of*iowa"); // (title=univ*of*iowa)
(User u) => u.Email.EndsWith("@gmail.com"); // (mail=*@gmail.com)
(User u) => u["acustomproperty"].Contains("some val"); // (acustomproperty=some val)
(User u) => u.Has("somekey"); // (somekey=*)
```

## LinqDirectorySearcher

If you don't mind another layer of abstraction, you can also use the included `LinqDirectorySearcher<T>`
and implemented the Repository pattern:

```c#
public IEnumerable<T> Page<T>(
    ISpecification<T> spec,
    int offsetPage = 0, int pageSize = 10,
    SortOption sortOpt = null)
    where T : Entry
{
    var searcher = new LinqDirectorySearcher<T>(Entry);
    searcher.SearchScope = SearchScope.Subtree;
    searcher.Filter = spec.AsExpression();
    searcher.VirtualListView = new DirectoryVirtualListView(0, pageSize - 1, pageSize * offsetPage);
    if (sortOpt != null)
        searcher.Sort = sortOpt;

    return searcher.FindAll();
}
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
        }.AsQueryable();
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