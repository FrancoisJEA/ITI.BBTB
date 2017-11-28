using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB.Tests
{
    [TestFixture]
    public class SimpleTests
    {
        [Test]
        public void ShouldPass()
        {
            Assert.That(0, Is.EqualTo(0));
        }

        [Test]
        public void ShouldFail()
        {
            Assert.That(0, Is.EqualTo(1));
        }
    }
}
