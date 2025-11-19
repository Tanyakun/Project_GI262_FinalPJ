using Solution;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public SO_item item;
    public int amount = 1;
    

    public void SetAmount(int newAmount)
    {
        amount = newAmount;
    }

    public void RandomAmount()
    {
        amount = Random.Range (1, item.maxStack + 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            //Add Item
            other.GetComponent<OOPPlayer>().inventoryCanvas.AddItem(item, amount);
            Destroy(gameObject);
        }
    }
}
