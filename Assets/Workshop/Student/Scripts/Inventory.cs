using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    public class Inventory : MonoBehaviour
    {
        // เปลี่ยนเป็น static เพื่อแชร์ระหว่างทุก instance
        public static Dictionary<string, int> inventory = new Dictionary<string, int>();

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
        }

        public bool HasItem(string item, int amount)
        {
            foreach (var kvp in inventory)
            {
                if (kvp.Key.Equals(item, System.StringComparison.OrdinalIgnoreCase) && kvp.Value >= amount)
                    return true;
            }
            return false;
        }

        public void UseItem(string item, int amount)
        {
            foreach (var kvp in new Dictionary<string,int>(inventory))
            {
                if (kvp.Key.Equals(item, System.StringComparison.OrdinalIgnoreCase))
                {
                    inventory[kvp.Key] -= amount;
                    if (inventory[kvp.Key] <= 0)
                        inventory.Remove(kvp.Key);
                    return;
                }
            }
            Debug.Log("Cannot remove " + item + ". Not found in inventory.");
        }

        public int GetItemCount(string item)
        {
            if (inventory.ContainsKey(item))
            {
                return inventory[item];
            }
            return 0;
        }

        public void PrintInventory()
        {
            if (inventory.Count == 0) return;

            Debug.Log("--- Inventory Content ---");
            foreach (var itemEntry in inventory)
            {
                Debug.Log(itemEntry.Key + ": " + itemEntry.Value);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                PrintInventory();
            }
        }
    }

    
}
