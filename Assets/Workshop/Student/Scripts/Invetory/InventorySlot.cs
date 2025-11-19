using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class InventorySlot : MonoBehaviour , IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
{
    [Header("Inventory Detail")]
    public InventoryCanvas inventory;

    [Header("Slot Detail")]
    public SO_item item;
    public int stack;

    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI stackText;

    [Header("Drag and Drop")]
    public int siblingIndex;
    public RectTransform draggable;
    Canvas canvas;
    CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        siblingIndex = transform.GetSiblingIndex();
    }

    #region Drag and Drop Method

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
        inventory.SetLayoutControlChild(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggable.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        draggable.anchoredPosition = Vector3.zero;
        transform.SetSiblingIndex(siblingIndex);
    }
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            InventorySlot slot = eventData.pointerDrag.GetComponent<InventorySlot>();
            if(slot != null)
            {
                if(slot.item == item)
                {
                    //Merge
                    MergeThisSlot(slot);
                }
                else
                {
                    //Swap
                    SwapSlot(slot);
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (item == inventory.EMPTY_ITEM)
                return;

            // inventory Open mini canvas
            inventory.OpenMiniCanvas(eventData.position);
            inventory.SetRightClickSlot(this);
        }
    }

    #endregion
    public void UseItem()
    {
        stack = Mathf.Clamp(stack - 1, 0, item.maxStack);
        if (stack > 0)
            CheckShowText();
        else
            inventory.RemoveItem(this);
    }

    public void SwapSlot(InventorySlot newSlot)
    {
        SO_item keepItem;
        int keepstack;

        keepItem = item;
        keepstack = stack;

        SetSwap(newSlot.item, newSlot.stack);

        newSlot.SetSwap(keepItem, keepstack);
    }

    public void SetSwap(SO_item swapItem, int amount)
    {
        item = swapItem;
        stack = amount;
        icon.sprite = swapItem.icon;

        CheckShowText();
    }

    public void MergeThisSlot(InventorySlot mergeSlot)
    {
        if (stack == item.maxStack || mergeSlot.stack == mergeSlot.item.maxStack)
        {
            SwapSlot(mergeSlot);
            return;
        }

        int ItemAmount = stack + mergeSlot.stack; // ไอเท็มรวมของทั้ง 2 ช่อง

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, item.maxStack); // ปรับให้เข้ากับ Maxstack
        stack = intInthisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInthisSlot;
        if (amountLeft > 0)
        {
            //set slot
            mergeSlot.SetThisSlot(mergeSlot.item, amountLeft);
        }
        else
        {
            // remove
            inventory.RemoveItem(mergeSlot);
        }

    }

    public void MergeThisSlot(SO_item mergeItem, int mergeAmount)
    {
        item = mergeItem;
        icon.sprite = mergeItem.icon;

        int ItemAmount = stack + mergeAmount; // ไอเท็มรวมของทั้ง 2 ช่อง

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, item.maxStack); // ปรับให้เข้ากับ Maxstack
        stack = intInthisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInthisSlot;
        if (amountLeft > 0)
        {
            InventorySlot slot = inventory.IsEmptySlotLeft(mergeItem, this);
            if (slot == null)
            {
                inventory.DropItem(mergeItem, amountLeft);
                return;
            }
            else
            {
                slot.MergeThisSlot(mergeItem, amountLeft);
            }
        }
    }

    public void SetThisSlot(SO_item newItem, int amount)
    {
        item = newItem;
        icon.sprite = newItem.icon;

        int ItemAmount = amount; // รับค่าเข้ามา

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, newItem.maxStack); // คำนวณว่าค่าที่เก็บมาเกิน Slot รึป่าว
        stack = intInthisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInthisSlot;
        if (amountLeft > 0)
        {
            InventorySlot slot = inventory.IsEmptySlotLeft(newItem, this); // Inventory Check empty slot
            if (slot == null)
            {
                //ถ้าเกินแล้วไม่มีไอเท็มเหมือนกันให้ทิ้งไอเท็ม
                return;
            }
            else
            {
                //ถ้าเกินแล้วมีไอเท็มเหลืออยู่ชนิดเดียวกันให้เก็บไว้
                slot.SetThisSlot(newItem, amountLeft);
            }
        }
    }

    public void CheckShowText() // ตรวจเช็คการ ShowText
    {
        stackText.text = stack.ToString();
        if(item.maxStack < 2)
        {
            stackText.gameObject.SetActive(false);
        }
        else
        {
            if(stack > 1)
                stackText.gameObject.SetActive(true); // มากกว่า 1 โชว์
            else
                stackText.gameObject.SetActive(false); // น้อยกว่า 1 ไม่โชว์
        }
    }

}
