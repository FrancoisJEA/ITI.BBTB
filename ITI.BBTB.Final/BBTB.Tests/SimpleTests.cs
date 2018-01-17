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
        public void PlayerSerializationandDeserialization()
            // Test the Serialization of 1 PlayerModel and check that the player stats are still the same even after the deserialization 
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
                Assert.That(WhiteGuy.Experience == BlackGuy.Experience);
                Assert.That(WhiteGuy.Agility == BlackGuy.Agility);
                Assert.That(WhiteGuy.Life == BlackGuy.Life);
                Assert.That(WhiteGuy.Intelligence == BlackGuy.Intelligence);
                Assert.That(WhiteGuy.Resistance == BlackGuy.Resistance);
                Assert.That(WhiteGuy.Strength == BlackGuy.Strength);
                Assert.That(WhiteGuy.Level == BlackGuy.Level);
                Assert.That(WhiteGuy.Money == BlackGuy.Money);
            }
        }
        
        [Test]
        public void PlayerLevel1()
        {
            using (var stream = new MemoryStream())
            {
                var BlackGuy = new PlayerModel("FirstPlayer", 1);
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(stream, BlackGuy);
                stream.Position = 0;
                PlayerModel WhiteGuy = (PlayerModel)f.Deserialize(stream);
                WhiteGuy.Level1SkillSetUp();
                Assert.That(WhiteGuy.Level == 1);
                Assert.That(WhiteGuy.Experience == 0);
                Assert.That(WhiteGuy.Life == 100);
                Assert.That(WhiteGuy.Strength == 10);
                Assert.That(WhiteGuy.Intelligence == 10);
                Assert.That(WhiteGuy.Agility == 10);
                Assert.That(WhiteGuy.Resistance == 10);
                Assert.That(WhiteGuy.Money == 0);
            }
        }

        [Test]
        public void LevelUp()
        {
            using (var stream = new MemoryStream())
            {
                var BlackGuy = new PlayerModel("FirstPlayer", 1);
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(stream, BlackGuy);
                stream.Position = 0;
                PlayerModel WhiteGuy = (PlayerModel)f.Deserialize(stream);
                WhiteGuy.Level1SkillSetUp();
                WhiteGuy.Experience = 900;
                WhiteGuy.LevelUp();
                Assert.That(WhiteGuy.Level == 4);
                Assert.That(WhiteGuy.Xpnext == 2700);
                Assert.That(WhiteGuy.SkillPoint == 3);
                Assert.That(WhiteGuy.Life == 130);
            }
        }

        [Test]
        public void UpSkill()
        {
            using (var stream = new MemoryStream())
            {
                var BlackGuy = new PlayerModel("FirstPlayer", 1);
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(stream, BlackGuy);
                stream.Position = 0;
                PlayerModel WhiteGuy = (PlayerModel)f.Deserialize(stream);
                WhiteGuy.Level1SkillSetUp();
                WhiteGuy.Experience = 900;
                WhiteGuy.LevelUp();
                WhiteGuy.UpSkills(3, "intelligence");
                Assert.That(WhiteGuy.SkillPoint == 0);
                Assert.That(WhiteGuy.Intelligence == 13);
            }
        }
    }
}
