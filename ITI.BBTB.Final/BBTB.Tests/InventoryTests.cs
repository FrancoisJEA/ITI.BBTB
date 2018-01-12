using BBTB.Items;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTB.Tests
{
    [TestFixture]
    public class InventoryTests
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
        public void Add_items_in_world()
        {
            var WorldItems = new WorldItem();
            string[,] Items = WorldItems.AddItems();
            string ItemName = "Steel helmet";
            string ItemName2 = "Leather plastron";
            Assert.That(Items[0, 0], Is.SameAs(ItemName));
            Assert.That(Items[1, 1], Is.SameAs(ItemName2));

        }
        [Test]
        public void Add_item_in_inventory ()
        {
            int i = 0;
            int j = 0;

            var InventoryItem = new InventoryItem();
            string ItemName = "Steel helmet";
            string [] Inventory = InventoryItem.AddItemToInventory(i, j);
            Assert.That(Inventory[0], Is.SameAs(ItemName));

        }
        [Test]
        public void Show_inventory()
        {
            int i = 0;
            int j = 0;

            var InventoryItem = new InventoryItem();
            string ItemName = "Steel helmet";
            string[] tmp = InventoryItem.AddItemToInventory(i, j);
            string[] Inventory = InventoryItem.InventoryList();

            Assert.That(Inventory[0], Is.SameAs(ItemName));


        }

        [Test]
        public void Pick_an_item_in_inventory()
        {
            var Items = new Items.InventoryItem();
            var InventorySystem = new Items.InventorySystem2();
            string ItemName = "Steel helmet";
            InventorySystem.PickItem(ItemName, Items);
            string ItemName2 = "Leather plastron";
            InventorySystem.PickItem(ItemName2, Items);


            string[] Inventory = Items.InventoryList();


            Assert.That(Inventory[0], Is.SameAs(ItemName));
            Assert.That(Inventory[1], Is.SameAs(ItemName2));

        }
        
        [Test]
        public void Drop_a_random_item ()
        {
            var InventorySystem = new Items.InventorySystem2();
            string ItemName = InventorySystem.Drop_Random_Item();
            Assert.That(ItemName, Is.Not.EqualTo(null));
        }

    }
}
