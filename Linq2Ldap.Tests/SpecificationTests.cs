using Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Linq2Ldap.Tests
{
    public class SpecificationTest
    {
        [Fact]
        public void NoneIsNone()
        {
            var list = new List<TestModel>()
            {
                new TestModel(),
                new TestModel()
            };
            var filtered = list.Where(Specification<TestModel>.None());
            Assert.Equal(0, filtered.Count());
        }

        [Fact]
        public void AllIsAll()
        {
            var list = new List<TestModel>()
            {
                new TestModel(),
                new TestModel()
            };
            var filtered = list.Where(Specification<TestModel>.All());
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void TrueIsAll()
        {
            var list = new List<TestModel>()
            {
                new TestModel(),
                new TestModel()
            };
            var filtered = list.Where(Specification<TestModel>.True);
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void FalseIsNone()
        {
            var list = new List<TestModel>()
            {
                new TestModel(),
                new TestModel()
            };
            var filtered = list.Where(Specification<TestModel>.False);
            Assert.Equal(0, filtered.Count());
        }

        [Fact]
        public void AndIsSubset()
        {
            var list = new List<TestModel>()
            {
                new TestModel() { Id = 1 },
                new TestModel() { Id = 2 },
                new TestModel() { Id = 3 }
            };
            var expr1 = Specification<TestModel>.Start(t => t.Id > 1);
            var expr2 = Specification<TestModel>.Start(t => t.Id < 3);
            var andExpr = expr1.And(expr2);
            var filtered = list.Where(andExpr);
            Assert.Equal(1, filtered.Count());
            Assert.Equal(2, filtered.First().Id);
        }

        [Fact]
        public void OrIsSuperset()
        {
            var list = new List<TestModel>()
            {
                new TestModel() { Id = 1 },
                new TestModel() { Id = 2 },
                new TestModel() { Id = 3 }
            };
            var expr1 = Specification<TestModel>.Start(t => t.Id < 2);
            var expr2 = Specification<TestModel>.Start(t => t.Id >= 3);
            var orExpr = expr1.Or(expr2);
            var filtered = list.Where(orExpr);
            Assert.Equal(2, filtered.Count());
            Assert.Equal(1, filtered.First().Id);
            Assert.Equal(3, filtered.Last().Id);
        }

        [Fact]
        public void AndExprIsSubset()
        {
            var list = new List<TestModel>()
            {
                new TestModel() { Id = 1 },
                new TestModel() { Id = 2 },
                new TestModel() { Id = 3 }
            };
            var expr1 = Specification<TestModel>.Start(t => t.Id > 1);
            var andExpr = expr1.And(t => t.Id < 3);
            var filtered = list.Where(andExpr);
            Assert.Equal(1, filtered.Count());
            Assert.Equal(2, filtered.First().Id);
        }

        [Fact]
        public void OrExprIsSuperset()
        {
            var list = new List<TestModel>()
            {
                new TestModel() { Id = 1 },
                new TestModel() { Id = 2 },
                new TestModel() { Id = 3 }
            };
            var expr1 = Specification<TestModel>.Start(t => t.Id < 2);
            var orExpr = expr1.Or(t => t.Id >= 3);
            var filtered = list.Where(orExpr);
            Assert.Equal(2, filtered.Count());
            Assert.Equal(1, filtered.First().Id);
            Assert.Equal(3, filtered.Last().Id);
        }

        [Fact]
        public void EqualsEquates()
        {
            var sa = Specification<TestModel>.Start(t => t.Id == 1);
            var sb = Specification<TestModel>.Start(t => t.Id == 2);
            Assert.False(sa.Equals(sb));
            Assert.True(sa.Equals(sa));
            Assert.False(sa.Equals(null));
        }

        [Fact]
        public void GetHashCodeHashes()
        {
            Assert.NotEqual(0, Specification<TestModel>.True.GetHashCode());
        }

        [Fact]
        public void CanGetMetadata()
        {
            var meta = Specification<TestModel>.True.Metadata;
        }

        [Fact]
        public void ImplicitConversion_Expr() {
            var spec = Specification<TestModel>.Start(t => t.Id == 314);
            Expression<Func<TestModel, bool>> e = spec;
        }

        [Fact]
        public void ImplicitConversion_Spec() {
            Expression<Func<TestModel, bool>> e = t => t.Id == 314;
            Specification<TestModel> spec = e;
        }
    }
}
