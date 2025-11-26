using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    // สืบทอดจาก Inventory เพื่อใช้ AddItem/UseItem/GetItemCount ได้เลย
    public class Craftable : Inventory
    {
        private bool isCraftMenuOpen = false;

        // ====== ฟังก์ชันกลาง ใช้ตรวจของ + คราฟ ======
        private void CraftItem(string itemToCraft, Dictionary<string, int> requiredMaterials)
        {
            List<string> missingMaterials = new List<string>();

            foreach (var material in requiredMaterials)
            {
                int countInInventory = GetItemCount(material.Key);
                if (countInInventory < material.Value)
                {
                    // เก็บว่าขาดอะไรเท่าไหร่
                    int needMore = material.Value - countInInventory;
                    missingMaterials.Add(material.Key + " x" + needMore);
                }
            }

            if (missingMaterials.Count > 0)
            {
                Debug.Log("Cannot craft " + itemToCraft + ". Missing: " + string.Join(", ", missingMaterials));
                return;
            }

            // มีครบ → หักของ + เพิ่มของที่คราฟได้
            foreach (var material in requiredMaterials)
            {
                UseItem(material.Key, material.Value);
            }

            AddItem(itemToCraft, 1);
            Debug.Log("Crafted " + itemToCraft + " successfully!");
        }

        // ====== Recipe แต่ละแบบ ======
        // ดาบไม้: ใช้ไม้ 2 ชิ้น
        private void CraftWoodenSword()
        {
            var req = new Dictionary<string, int>()
            {
                { "Wood Log", 2 }          // ชื่อ item ใน inventory ต้องตรงกับตอน AddItem
            };

            CraftItem("Wooden Sword", req);
        }

        // ดาบเหล็ก: ดาบไม้ 1 เล่ม + เงิน 1 แท่ง
        private void CraftSilverSword()
        {
            var req = new Dictionary<string, int>()
            {
                { "Wooden Sword", 1 },
                { "Silver Ingot", 1 }
            };

            CraftItem("Silver Sword", req);
        }

        // ดาบทอง: ดาบเงิน 1 เล่ม + ทอง 1 แท่ง
        private void CraftGoldenSword()
        {
            var req = new Dictionary<string, int>()
            {
                { "Silver Sword", 1 },
                { "Golden Ingot", 1 }
            };

            CraftItem("Golden Sword", req);
        }

        // ====== Input Logic ======
        private void Update()
        {
            // กด C เพื่อเปิด/ปิดเมนูคราฟ
            if (Input.GetKeyDown(KeyCode.C))
            {
                isCraftMenuOpen = !isCraftMenuOpen;

                if (isCraftMenuOpen)
                {
                    Debug.Log(
                        "=== Craft Menu ===\n" +
                        "1 - Wooden Sword (Wood Log x2)\n" +
                        "2 - Silver Sword (Wooden Sword x1, Silver Ingot x1)\n" +
                        "3 - Golden Sword (Silver Sword x1, Golden Ingot x1)\n" +
                        "Press 1/2/3 to craft."
                    );
                }
                else
                {
                    Debug.Log("Closed Craft Menu.");
                }
            }

            if (!isCraftMenuOpen) return;

            // เลือกสูตรคราฟด้วยเลข 1 / 2 / 3
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CraftWoodenSword();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CraftSilverSword();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CraftGoldenSword();
            }
        }
    }
}
