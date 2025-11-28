using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    // สืบทอดจาก Inventory เพื่อใช้ AddItem/UseItem/GetItemCount ได้
    public class Craftable : Inventory
    {
        // เก็บสูตรคราฟทั้งหมด (itemปลายทาง -> วัตถุดิบ)
        private Dictionary<string, Dictionary<string, int>> recipes =
            new Dictionary<string, Dictionary<string, int>>();

        private Dictionary<string, int> weaponDamage = new Dictionary<string, int>()
        {
            { "Wooden Sword", 5 },
            { "Silver Sword", 10 },
            { "Golden Sword", 20 }
        };

        private bool isCraftMenuOpen = false;
        private bool isSalvageMenuOpen = false;

        private void Awake()
        {
            // กำหนดสูตรคราฟครั้งเดียวที่นี่
            recipes["Wooden Sword"] = new Dictionary<string, int>()
            {
                { "Wood Log", 2 }                // ดาบไม้ = ไม้ 2 ชิ้น
            };

            recipes["Silver Sword"] = new Dictionary<string, int>()
            {
                { "Wooden Sword", 1 },         // ดาบเหล็ก = ดาบไม้ 1 เล่ม
                { "Silver Ingot", 1 }          //           + เหล็ก 1 แท่ง
            };

            recipes["Golden Sword"] = new Dictionary<string, int>()
            {
                { "Silver Sword", 1 },         // ดาบทอง = ดาบเหล็ก 1 เล่ม
                { "Golden Ingot", 1 }          //          + ทอง 1 แท่ง
            };
        }

        // ========= CRAFT =========
        private void CraftItem(string itemToCraft)
        {
            if (!recipes.TryGetValue(itemToCraft, out var requiredMaterials))
            {
                Debug.LogWarning("No recipe for " + itemToCraft);
                return;
            }

            List<string> missingMaterials = new List<string>();

            foreach (var material in requiredMaterials)
            {
                int countInInventory = GetItemCount(material.Key);
                if (countInInventory < material.Value)
                {
                    int needMore = material.Value - countInInventory;
                    missingMaterials.Add(material.Key + " x" + needMore);
                }
            }

            if (missingMaterials.Count > 0)
            {
                Debug.Log("Cannot craft " + itemToCraft +
                        ". Missing: " + string.Join(", ", missingMaterials));
                return;
            }

            // มีครบ → หักวัตถุดิบ
            foreach (var material in requiredMaterials)
            {
                UseItem(material.Key, material.Value);
            }

            AddItem(itemToCraft, 1);
            Debug.Log("Crafted " + itemToCraft + " successfully!");

            // --- เพิ่มส่วนอัปเดตค่าโจมตีของ Player ---
            var player = FindObjectOfType<OOPPlayer>();
            if (player != null && weaponDamage.ContainsKey(itemToCraft))
            {
                player.AttackPoint = weaponDamage[itemToCraft];
                Debug.Log("Player's AttackPoint is now " + player.AttackPoint);
            }
        }


        private void CraftWoodenSword() => CraftItem("Wooden Sword");
        private void CraftSilverSword() => CraftItem("Silver Sword");
        private void CraftGoldenSword() => CraftItem("Golden Sword");

        // ========= SALVAGE (ย่อยของ) =========

        // ฟังก์ชันหลัก: ย่อยไอเท็ม 1 ชิ้น
        private void SalvageItem(string itemToSalvage)
        {
            if (!HasItem(itemToSalvage, 1))
            {
                Debug.Log("Cannot salvage " + itemToSalvage + ". You don't have it.");
                return;
            }

            // หักไอเท็มที่จะย่อยก่อน
            UseItem(itemToSalvage, 1);

            // ย้อนสูตรเพื่อคืนวัตถุดิบทั้งหมด
            RefundMaterialsRecursive(itemToSalvage, 1);

            Debug.Log("Salvaged " + itemToSalvage + " and got materials back.");
        }

        // ย้อนสูตรแบบ recursive:
        // ถ้า item มีสูตร → แตกเป็นวัตถุดิบแล้วเรียกซ้ำ
        // ถ้าไม่มีสูตร → ถือว่าเป็นวัตถุดิบพื้นฐาน → AddItem คืนให้
        private void RefundMaterialsRecursive(string item, int count)
        {
            if (!recipes.TryGetValue(item, out var requiredMaterials))
            {
                // base material
                AddItem(item, count);
                return;
            }

            foreach (var material in requiredMaterials)
            {
                int total = material.Value * count;
                RefundMaterialsRecursive(material.Key, total);
            }
        }

        private void SalvageWoodenSword() => SalvageItem("Wooden Sword");
        private void SalvageSilverSword() => SalvageItem("Silver Sword");
        private void SalvageGoldenSword() => SalvageItem("Golden Sword");

        // ========= INPUT / MENUS =========
        private void Update()
        {
            // ----- เปิด / ปิด Craft Menu (C) -----
            if (Input.GetKeyDown(KeyCode.C))
            {
                isCraftMenuOpen = !isCraftMenuOpen;
                if (isCraftMenuOpen)
                {
                    isSalvageMenuOpen = false; // ปิดอีกเมนู
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

            // ----- เปิด / ปิด Salvage Menu (Z) -----
            if (Input.GetKeyDown(KeyCode.Z))
            {
                isSalvageMenuOpen = !isSalvageMenuOpen;
                if (isSalvageMenuOpen)
                {
                    isCraftMenuOpen = false; // ปิดเมนูคราฟ
                    Debug.Log(
                        "=== Salvage Menu ===\n" +
                        "1 - Salvage Wooden Sword  (-> Wood Log x2)\n" +
                        "2 - Salvage Silver Sword  (-> Wood Log x2, Silver Ingot x1)\n" +
                        "3 - Salvage Golden Sword  (-> Wood Log x2, Silver Ingot x1, Golden Ingot x1)\n" +
                        "Press 1/2/3 to salvage."
                    );
                }
                else
                {
                    Debug.Log("Closed Salvage Menu.");
                }
            }

            // ----- กดเลขตอนอยู่ใน Craft Menu -----
            if (isCraftMenuOpen)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) CraftWoodenSword();
                else if (Input.GetKeyDown(KeyCode.Alpha2)) CraftSilverSword();
                else if (Input.GetKeyDown(KeyCode.Alpha3)) CraftGoldenSword();
            }

            // ----- กดเลขตอนอยู่ใน Salvage Menu -----
            if (isSalvageMenuOpen)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) SalvageWoodenSword();
                else if (Input.GetKeyDown(KeyCode.Alpha2)) SalvageSilverSword();
                else if (Input.GetKeyDown(KeyCode.Alpha3)) SalvageGoldenSword();
            }
        }
    }
}
