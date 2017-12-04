using BBTB;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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

        [Test]
        public void PlayerSerialization()
        {
            using (var stream = new MemoryStream())
            {
               // var BlackGuy = new Player() { Texture2D _texture,};
            }
        }
    }
}
