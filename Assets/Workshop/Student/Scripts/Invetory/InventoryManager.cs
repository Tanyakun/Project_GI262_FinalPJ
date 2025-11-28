using Solution;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu; // ใส่ Panel UI ที่ใช้แสดงอินเวนทอรี
    public OOPPlayer player;
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
        inventoryMenu.SetActive(isOpen);

        if (isOpen)
        {
            Time.timeScale = 0f;   // หยุดเกม
            player.canMove = false; // หยุดผู้เล่น(คุณ มฤตยู98)

        }
        else
        {
            Time.timeScale = 1f;   // เล่นต่อ
            player.canMove = true; // ผู้เล่นเล่นต่อ(คุณ มฤตยู98)
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        Debug.Log("itemName = " + itemName + "quantity = " + quantity + "itemSprite = " + itemSprite);
    }

}
