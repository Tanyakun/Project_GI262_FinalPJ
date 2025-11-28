using UnityEngine;

namespace Solution
{
    public class InventoryUI : MonoBehaviour
    {
        public GameObject inventoryUI; // ใส่ Panel UI ที่ใช้แสดงอินเวนทอรี
        private bool isOpen = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
        }

        void ToggleInventory()
        {
            isOpen = !isOpen;
            inventoryUI.SetActive(isOpen);
        }
    }
}
