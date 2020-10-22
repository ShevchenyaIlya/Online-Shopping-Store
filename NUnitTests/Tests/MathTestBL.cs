using NUnit.Framework;
using ProductStore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitTests.Tests
{
    [TestFixture]
    class MathTestBL
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var mathBL = new MathBL();
            int result = mathBL.Sum(2, 4);
            Assert.IsTrue(result == 6);
        }
    }
}
