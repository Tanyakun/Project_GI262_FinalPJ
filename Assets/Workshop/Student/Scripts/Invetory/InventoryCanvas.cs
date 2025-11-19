using Solution;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvas : MonoBehaviour
{
    [Header("Inventory")]
    public SO_item EMPTY_ITEM;
    public Transform slotPrefab;
    public Transform InventoryPanel;
    protected GridLayoutGroup gridLayoutGroup;
    [Space(5)]
    public int slotAmount = 10;
    public InventorySlot[] inventorySlots;

    [Header("Mini canvas")]
    public RectTransform miniCanvas;
    [SerializeField] protected InventorySlot rightClickSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridLayoutGroup = InventoryPanel.GetComponent<GridLayoutGroup>();
        CreateInventorySlots();
    }

    #region Inventory Methods

    public void AddItem(SO_item item, int amount)
    {
        InventorySlot slot = IsEmptySlotLeft(item);
        if(slot == null)
        {
            // Full
            DropItem(item, amount);
            return;
        }
        slot.MergeThisSlot(item , amount);
    }

    public void UseItem() // OnClick Event
    {
        rightClickSlot.UseItem();
        OnFinishMiniCavas();
    }

    public void DropItem() // OnClick Event
    {
        // Item Spawner Spawn Item
        DestroyItem();
    }

    public void DestroyItem() // OnClick Event
    {
        rightClickSlot.SetThisSlot(EMPTY_ITEM, 0);
        OnFinishMiniCavas();
    }

    public void DropItem(SO_item item, int amount)
    {
        // Item Spawner Spawn Item

    }

    public void RemoveItem(InventorySlot slot)
    {
        slot.SetThisSlot(EMPTY_ITEM, 0);
    }

    public void SortItem(bool Ascending = true)
    {

    }

    public void CreateInventorySlots()
    {
        inventorySlots = new InventorySlot[slotAmount];
        for (int i = 0; i < slotAmount; i++)
        {
            Transform slot = Instantiate(slotPrefab, InventoryPanel);
            InventorySlot invSlot = slot.GetComponent<InventorySlot>();

            inventorySlots[i] = invSlot;
            invSlot.inventory = this;
            invSlot.SetThisSlot(EMPTY_ITEM, 0); // สั่งให้เป็นช่องว่าง
        }
    }

    public InventorySlot IsEmptySlotLeft(SO_item itemChecker = null, InventorySlot itemSlot = null)
    {
        InventorySlot firstEmptySlot = null;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot == itemSlot) // เช็คว่า Slot เดียวกันไหมถ้าเดียวกันจะข้าม
                continue;

            if (slot.item == itemChecker) // เช็คว่าเจอไอเท็มที่ตรงกันแล้ว
            {
                if (slot.stack < slot.item.maxStack) // เช็ค Stack ว่าเหลือพื้นที่ว่าไหม
                {
                    return slot;
                }
            }
            else if (slot.item == EMPTY_ITEM && firstEmptySlot == null) // เช็คว่ามีช่องว่างรึป่าวและเน้นให้เก็บไปที่ช่องว่างช่องแรกก่อน
                 firstEmptySlot = slot;
        }
        return firstEmptySlot;
    }

    public void SetLayoutControlChild(bool isControlled)
    {
        gridLayoutGroup.enabled = isControlled;
    }

    #endregion

    #region Mini Canvas

    public void SetRightClickSlot(InventorySlot slot)
    {
        rightClickSlot = slot;
    }

    public void OpenMiniCanvas(Vector2 clickPosition)
    {
        miniCanvas.position = clickPosition;
        miniCanvas.gameObject.SetActive(true);
    }

    public void OnFinishMiniCavas()
    {
        rightClickSlot = null;
        miniCanvas.gameObject.SetActive(false);
    }

    #endregion

}
