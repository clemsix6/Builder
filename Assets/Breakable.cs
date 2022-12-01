using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakable : MonoBehaviour
{
    [SerializeField] public Actor  actor;
    [SerializeField] public int    hp = 10;
    [SerializeField] public Loot[] lootOnHit;


    public void Hit(Robot robot, int damages)
    {
        foreach (var loot in lootOnHit)
        {
            if (Random.value > loot.lootChance / 100f)
                continue;
            robot.inventory.AddItem(loot.itemId);
            loot.lootCount++;
        }
    }
    

    [Serializable]
    public class Loot
    {
        public string itemId;
        public float  lootChance;
        public int    lootMaxCount;
        public int    lootCount;
    }
}
