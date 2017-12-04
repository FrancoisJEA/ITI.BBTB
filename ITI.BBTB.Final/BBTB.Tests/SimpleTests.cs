using BBTB;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
                var BlackGuy = new PlayerModel("FirstPlayer", 3);
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(stream, BlackGuy);
                stream.Position = 0;
                PlayerModel WhiteGuy = (PlayerModel)f.Deserialize(stream);//(PlayerModel)f.Deserialise(stream) mean that we convert what we are talking about after in a PlayerModel
                Assert.AreNotSame( WhiteGuy, BlackGuy, "We have 2 different object.");
                Assert.That(WhiteGuy.Name == BlackGuy.Name, "But with the same value.");
            }
        }


    }
}
