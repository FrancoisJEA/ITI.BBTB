using Game1.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
        public class InventorySystem
        {
            private const int MAXIMUM_SLOTS_IN_INVENTORY = 5;

            public readonly List<InventoryRecord> InventoryRecords = new List<InventoryRecord>(); //Inventory of the player

            public void AddItem(ObtainableItem item, int quantityToAdd)
            {
                while (quantityToAdd > 0)
                {
                    // If an object of this item type already exists in the inventory, and has room to stack more items,
                    // then add as many as we can to that stack.
                    if (InventoryRecords.Exists(x => (x.InventoryItem.ID == item.ID) && (x.Quantity < item.MaximumStackableQuantity)))
                    {
                        // Get the item we're going to add quantity to
                        InventoryRecord inventoryRecord =
                        InventoryRecords.First(x => (x.InventoryItem.ID == item.ID) && (x.Quantity < item.MaximumStackableQuantity));

                        // Calculate how many more can be added to this stack
                        int maximumQuantityYouCanAddToThisStack = (item.MaximumStackableQuantity - inventoryRecord.Quantity);

                        // Add to the stack (either the full quanity, or the amount that would make it reach the stack maximum)
                        int quantityToAddToStack = Math.Min(quantityToAdd, maximumQuantityYouCanAddToThisStack);

                        inventoryRecord.AddToQuantity(quantityToAddToStack);

                        // Decrease the quantityToAdd by the amount we added to the stack.
                        // If we added the total quantityToAdd to the stack, then this value will be 0, and we'll exit the 'while' loop.
                        quantityToAdd -= quantityToAddToStack;
                    }
                    else
                    {
                        // We don't already have an existing inventoryRecord for this ObtainableItem object,
                        // so, add one to the list, if there is room.
                        if (InventoryRecords.Count < MAXIMUM_SLOTS_IN_INVENTORY)
                        {
                            // Don't set the quantity value here.
                            // The 'while' loop will take us back to the code above, which will add to the quantity.
                            InventoryRecords.Add(new InventoryRecord(item, 0));
                        }
                        else
                        {
                            // If no more place in Inventory, replace item.
                            throw new Exception("There is no more space in the inventory");
                        }
                    }
                }
            }

            public class InventoryRecord
            {
                public ObtainableItem InventoryItem { get; private set; }
                public int Quantity { get; private set; }

                public InventoryRecord(ObtainableItem item, int quantity)
                {
                    InventoryItem = item;
                    Quantity = quantity;
                }

                public void AddToQuantity(int amountToAdd)
                {
                    Quantity += amountToAdd;
                }
            }
        }
    }
