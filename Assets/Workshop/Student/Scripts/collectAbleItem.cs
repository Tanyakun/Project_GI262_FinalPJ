using UnityEngine;

namespace Solution
{
    public class CollectAbleItem : Identity
    {
        public override bool Hit()
        {
            mapGenerator.player.inventory.AddItem(Name, 1);
            Destroy(gameObject);
            return true;
        }
    }
}

