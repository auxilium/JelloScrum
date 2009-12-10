using System;
using JelloScrum.Model.Helpers;
using NUnit.Framework;

namespace JelloScrum.Model.Tests.Helpers
{
    [TestFixture]
    public class DateRangeTests : TestBase
    {
        private DateTime start;
        private DateTime end;
        private DateRange dr;

        [SetUp]
        public override void SetUp()
        {
            start = new DateTime(2009, 11, 06);
            end = new DateTime(2009, 11, 20);
        }


        [Test, ExpectedException(typeof(ArgumentException))]
        public void StartDateIsBeforeEndDate()
        {
            start = new DateTime(2009, 12, 06);

            new DateRange(start, end);
        }

        [Test]
        public void Overlap()
        {
            dr = new DateRange(start, end);

            Assert.IsTrue(dr.Overlap(new DateTime(2009, 11, 10)));
        }

        [Test]
        public void DoesNotOverlap()
        {
            dr = new DateRange(start, end);

            Assert.IsFalse(dr.Overlap(new DateTime(2009, 12, 10)));
        }

        [Test]
        public void OverlapsOnStartDate()
        {
            dr = new DateRange(start, end);
            Assert.IsTrue(dr.Overlap(new DateTime(2009, 11, 06)));
        }

        [Test]
        public void OverlapsOnEndDate()
        {
            dr = new DateRange(start, end);
            Assert.IsTrue(dr.Overlap(new DateTime(2009, 11, 20)));
        }
    }
}