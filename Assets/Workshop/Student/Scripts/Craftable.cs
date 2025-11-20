using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    public class Craftable : Inventory
    {
        public void CraftSilverSword()
        {
            string itemToCraft = "Silver Sword";
            Dictionary<string, int> requiredMaterials = new Dictionary<string, int>()
            {
                { "Silver Ingot", 1 },
                { "Wood Log", 1 }
            };

            List<string> missingMaterials = new List<string>();

            foreach (var material in requiredMaterials)
            {
                int countInInventory = GetItemCount(material.Key);
                if (countInInventory < material.Value)
                {
                    missingMaterials.Add(material.Key + " x" + (material.Value - countInInventory));
                }
            }

            if (missingMaterials.Count > 0)
            {
                Debug.Log("Cannot craft " + itemToCraft + ". Missing: " + string.Join(", ", missingMaterials));
            }
            else
            {
                foreach (var material in requiredMaterials)
                {
                    UseItem(material.Key, material.Value);
                }

                AddItem(itemToCraft, 1);
                Debug.Log("Crafted " + itemToCraft + " successfully!");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CraftSilverSword();
            }
        }
    }
}
