using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    public class OOPWall : Identity
    {
        public int Damage;
        public bool IsIceWall;
        public string ItemToDig = "PickAxe";

        private void Start()
        {
            IsIceWall = Random.Range(0, 100) < 20 ? true : false;
            if (IsIceWall)
            {
                GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }

        public override bool Hit()
        {
            // ตรวจสอบว่าผู้เล่นมี Inventory
            Inventory playerInventory = mapGenerator.player.GetComponent<Inventory>();
            if (playerInventory == null)
            {
                Debug.LogWarning("Player has no Inventory component!");
                return false;
            }

            // ตรวจสอบว่าผู้เล่นมี PickAxe แบบ ignore case
            if (!HasItemIgnoreCase(playerInventory, ItemToDig, 1))
            {
                Debug.Log("You need a PickAxe to break this wall!");
                return false;
            }

            // ใช้ PickAxe 1 ชิ้น
            UseItemIgnoreCase(playerInventory, ItemToDig);

            // ลบกำแพงจาก map
            mapGenerator.mapdata[positionX, positionY] = null;
            Destroy(gameObject);

            Debug.Log("Wall broken using PickAxe!");
            return true;
        }

        // ฟังก์ชันตรวจสอบไอเท็มแบบ ignore case
        private bool HasItemIgnoreCase(Inventory inv, string item, int amount)
        {
            foreach (var key in inv.inventory.Keys)
            {
                if (key.ToLower() == item.ToLower() && inv.inventory[key] >= amount)
                    return true;
            }
            return false;
        }

        // ฟังก์ชันใช้ไอเท็มแบบ ignore case
        private void UseItemIgnoreCase(Inventory inv, string item)
        {
            foreach (var key in inv.inventory.Keys)
            {
                if (key.ToLower() == item.ToLower())
                {
                    Debug.Log("PickAxe used but not removed from inventory.");
                    return;
                }
            }
        }
    }
}
