using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Domain.Objects;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trendyol.App.Domain.Enums;
using Trendyol.App.Domain.Requests;
using Trendyol.App.EntityFramework.Extensions;

namespace Trendyol.App.EntityFramework.Tests
{
    public class EnumerableExtensionTests
    {
        [Test]
        public void Page_Should_Throw_InvalidOperationException_When_OrderByParameter_Empty()
        {
            Fixture fixture = new Fixture();

            IEnumerable<Sample> samples = fixture.CreateMany<Sample>(30);

            PagedRequest request = new PagedRequest()
            {
                PageSize = 5,
                OrderBy = "",
                Order = OrderType.Desc,
                Page = 2
            };

            Assert.Throws<InvalidOperationException>(() =>
            {
                var orderedSamples = samples.AsQueryable().ToPage(request);
            });
        }

        [Test]
        public void Page_Should_Throw_ParseException_When_OrderByParameter_InvalidColumnName()
        {
            Fixture fixture = new Fixture();

            IEnumerable<Sample> samples = fixture.CreateMany<Sample>(30);

            PagedRequest request = new PagedRequest()
            {
                PageSize = 5,
                OrderBy = "asdsadsadasd",
                Order = OrderType.Desc,
                Page = 2
            };

            Assert.Throws<ParseException>(() =>
            {
                var orderedSamples = samples.AsQueryable().ToPage(request);
            });
        }

        [Test]
        public void Page_Should_Order_When_Parameter_CamelCase()
        {
            Fixture fixture = new Fixture();

            IEnumerable<Sample> samples = fixture.CreateMany<Sample>(30);

            PagedRequest request = new PagedRequest()
            {
                PageSize = 5,
                OrderBy = "testLongFieldWithLongName",
                Order = OrderType.Desc,
                Page = 2
            };

            var orderedSamples = samples.AsQueryable().ToPage(request).Items;

            var expectedSamples = samples.OrderByDescending(s => s.TestLongFieldWithLongName).Skip(5).Take(5).ToList();

            for (int i = 0; i < expectedSamples.Count; i++)
            {
                Assert.AreEqual(orderedSamples[i].Name, expectedSamples[i].Name);
                Assert.AreEqual(orderedSamples[i].Size, expectedSamples[i].Size);
                Assert.AreEqual(orderedSamples[i].Id, expectedSamples[i].Id);
                Assert.AreEqual(orderedSamples[i].CreatedOn, expectedSamples[i].CreatedOn);
                Assert.AreEqual(orderedSamples[i].TestField, expectedSamples[i].TestField);
            }
        }

        [Test]
        public void Page_Should_Order_When_Parameter_PascalCase()
        {
            Fixture fixture = new Fixture();

            IEnumerable<Sample> samples = fixture.CreateMany<Sample>(30);

            PagedRequest request = new PagedRequest()
            {
                PageSize = 5,
                OrderBy = "TestLongFieldWithLongName",
                Order = OrderType.Desc,
                Page = 2
            };

            var orderedSamples = samples.AsQueryable().ToPage(request).Items;

            var expectedSamples = samples.OrderByDescending(s => s.TestLongFieldWithLongName).Skip(5).Take(5).ToList();

            for (int i = 0; i < expectedSamples.Count; i++)
            {
                Assert.AreEqual(orderedSamples[i].Name, expectedSamples[i].Name);
                Assert.AreEqual(orderedSamples[i].Size, expectedSamples[i].Size);
                Assert.AreEqual(orderedSamples[i].Id, expectedSamples[i].Id);
                Assert.AreEqual(orderedSamples[i].CreatedOn, expectedSamples[i].CreatedOn);
                Assert.AreEqual(orderedSamples[i].TestField, expectedSamples[i].TestField);
            }
        }

        [Test]
        public void Page_Should_Order_When_Parameter_Lowercase()
        {
            Fixture fixture = new Fixture();

            IEnumerable<Sample> samples = fixture.CreateMany<Sample>(30);

            PagedRequest request = new PagedRequest()
            {
                PageSize = 5,
                OrderBy = "testlongfieldwithlongname",
                Order = OrderType.Desc,
                Page = 2
            };

            var orderedSamples = samples.AsQueryable().ToPage(request).Items;

            var expectedSamples = samples.OrderByDescending(s => s.TestLongFieldWithLongName).Skip(5).Take(5).ToList();

            for (int i = 0; i < expectedSamples.Count; i++)
            {
                Assert.AreEqual(orderedSamples[i].Name, expectedSamples[i].Name);
                Assert.AreEqual(orderedSamples[i].Size, expectedSamples[i].Size);
                Assert.AreEqual(orderedSamples[i].Id, expectedSamples[i].Id);
                Assert.AreEqual(orderedSamples[i].CreatedOn, expectedSamples[i].CreatedOn);
                Assert.AreEqual(orderedSamples[i].TestField, expectedSamples[i].TestField);
            }
        }

        [Test]
        public void Page_Should_Order_When_Parameter_Uppercase()
        {
            Fixture fixture = new Fixture();

            IEnumerable<Sample> samples = fixture.CreateMany<Sample>(30);

            PagedRequest request = new PagedRequest()
            {
                PageSize = 5,
                OrderBy = "TESTLONGFIELDWITHLONGNAME",
                Order = OrderType.Desc,
                Page = 2
            };

            var orderedSamples = samples.AsQueryable().ToPage(request).Items;

            var expectedSamples = samples.OrderByDescending(s => s.TestLongFieldWithLongName).Skip(5).Take(5).ToList();

            for (int i = 0; i < expectedSamples.Count; i++)
            {
                Assert.AreEqual(orderedSamples[i].Name, expectedSamples[i].Name);
                Assert.AreEqual(orderedSamples[i].Size, expectedSamples[i].Size);
                Assert.AreEqual(orderedSamples[i].Id, expectedSamples[i].Id);
                Assert.AreEqual(orderedSamples[i].CreatedOn, expectedSamples[i].CreatedOn);
                Assert.AreEqual(orderedSamples[i].TestField, expectedSamples[i].TestField);
            }
        }
    }
}
