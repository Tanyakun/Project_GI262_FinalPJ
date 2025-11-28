using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Solution
{

    public class OOPExit : Identity
    {
        public GameObject YouWin;
        public string ItemToOpen = "Key";
        public int ItemAmountToOpen = 1;
        public override bool Hit()
        {
            // ��Ǩ�ͺ��Ҽ����������������ͧ����������
            bool IsHasItemAmount = mapGenerator.player.inventory.HasItem(ItemToOpen, ItemAmountToOpen);
            if (IsHasItemAmount)
            {
                mapGenerator.player.inventory.UseItem(ItemToOpen, ItemAmountToOpen);
                YouWin.SetActive(true);
                Debug.Log("You win");
                return true;
            }
            else {
                Debug.Log("Need Item " + ItemToOpen + " to Open");
                return false;
            }
        }
    }
}