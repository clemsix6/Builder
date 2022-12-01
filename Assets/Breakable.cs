using System;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public Actor  actor;
    public int    hp = 10;
    public Loot[] lootOnHit;


    public void Hit(Robot robot, int damages)
    {
    }
    

    [Serializable]
    public class Loot
    {
        public  string itemId;
        public  float  lootChance;
        public  int    lootMaxCount;
        private int    lootCount;
    }
}
