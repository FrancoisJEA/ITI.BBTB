using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBTB.Item;

namespace BBTB.Item
{
    public class InventorySystem2
    {
        int i;
        int j;
        public int InventoryEmplacements = 8;
        WorldItem WorldItem = new WorldItem();
        //InventoryItem InventoryItem = new InventoryItem();
        string ItemName { get; set; }
        //string[,] ItemList;      


        public void PickItem (string ItemName, InventoryItem inventory)
        {
           string[,] ItemList = WorldItem.AddItems();
           
            for (i = 0;  i < InventoryEmplacements; i++)
            {
                for (j = 0; j < ItemList.GetLength(1); j++)
                {
                    if (ItemName == ItemList[i,j])
                    {
                       inventory.AddItemToInventory(i, j);
                    }
                }
            }
        }
    
    }
}
