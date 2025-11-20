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
            // ตรวจสอบว่ามี PickAxe ใน static inventory
            if (!HasItemIgnoreCase(ItemToDig, 1))
            {
                Debug.Log("You need a PickAxe to break this wall!");
                return false;
            }

            // ใช้ PickAxe 1 ชิ้น
            UseItemIgnoreCase(ItemToDig, 1);

            // ลบกำแพงจาก map
            mapGenerator.mapdata[positionX, positionY] = null;
            Destroy(gameObject);

            Debug.Log("Wall broken using PickAxe!");
            return true;
        }

        // ตรวจสอบไอเท็มแบบ ignore case
        private bool HasItemIgnoreCase(string item, int amount)
        {
            foreach (var kvp in Inventory.inventory)
            {
                if (kvp.Key.Equals(item, System.StringComparison.OrdinalIgnoreCase) && kvp.Value >= amount)
                    return true;
            }
            return false;
        }

        // ใช้ไอเท็มแบบ ignore case
        private void UseItemIgnoreCase(string item, int amount)
        {
            foreach (var key in Inventory.inventory.Keys)
            {
                if (key.Equals(item, System.StringComparison.OrdinalIgnoreCase))
                {
                    // แค่พิมพ์ข้อความว่าใช้ไอเท็ม แต่ไม่ลดจำนวน
                    Debug.Log(item + " used but not removed from inventory.");
                    return;
                }
            }
        }
    }
}
