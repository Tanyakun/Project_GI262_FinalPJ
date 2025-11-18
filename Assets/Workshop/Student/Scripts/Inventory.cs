using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    public class Inventory : MonoBehaviour
    {
        public Dictionary<string, int> inventory = new Dictionary<string, int>();

        // **ส่วนที่แก้ไข/เพิ่ม: 1. อ้างอิงถึง UI Updater**
        public InventoryUIUpdater uiUpdater;

        // เพิ่มไอเท็ม
        public void AddItem(string item, int amount)
        {
            if (inventory.ContainsKey(item))
            {
                inventory[item] += amount;
            }
            else
            {
                inventory.Add(item, amount);
            }

            Debug.Log("Added " + amount + " " + item + ". Total: " + inventory[item]);

            // **ส่วนที่แก้ไข/เพิ่ม: 2. เรียกอัปเดต UI**
            if (uiUpdater != null)
            {
                uiUpdater.UpdateAllItemCounts();
            }
        }

        // ลบไอเท็ม
        public void UseItem(string item, int amount)
        {
            if (inventory.ContainsKey(item))
            {
                inventory[item] -= amount;

                if (inventory[item] <= 0)
                {
                    inventory.Remove(item);
                    Debug.Log("Removed all " + item + " from inventory.");
                }
                else
                {
                    Debug.Log("Removed " + amount + " " + item + ". Remaining: " + inventory[item]);
                }

                // **ส่วนที่แก้ไข/เพิ่ม: 3. เรียกอัปเดต UI**
                if (uiUpdater != null)
                {
                    uiUpdater.UpdateAllItemCounts();
                }
            }
            else
            {
                Debug.Log("Cannot remove " + item + ". Not found in inventory.");
            }
        }

        public bool HasItem(string item, int amount)
        {
            return inventory.ContainsKey(item) && inventory[item] >= amount;
        }

        // ตรวจสอบจำนวนไอเท็ม
        public int GetItemCount(string item)
        {
            if (inventory.ContainsKey(item))
            {
                return inventory[item];
            }
            return 0;
        }

        // แสดงรายการทั้งหมดในคลัง
        public void PrintInventory()
        {
            Debug.Log("--- Inventory Content ---");
            if (inventory.Count == 0)
            {
                Debug.Log("Inventory is empty.");
                return;
            }

            foreach (var itemEntry in inventory)
            {
                Debug.Log(itemEntry.Key + ": " + itemEntry.Value);
            }
        }
    }
}