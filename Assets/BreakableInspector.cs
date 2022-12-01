using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakableInspector : MonoBehaviour
{
    [SerializeField] private Transform     lootsData;
    [SerializeField] private Image          icon;
    [SerializeField] private TMP_Text       titleText;
    [SerializeField] private TMP_Text       idText;
    [SerializeField] private TMP_Text       descriptionText;
    [SerializeField] private TMP_Text       durabilityText;
    [SerializeField] private LootInspection lootInspection;

    private List<LootInspection> loots = new();

    public void UpdateData(Breakable breakable)
    {
        var title = breakable.actor.type.ToArray();
        title[0] = char.ToUpper(title[0]);
        
        titleText.SetText(new string(title));
        idText.SetText("ID: " + breakable.actor.id);
        descriptionText.SetText(breakable.actor.description);
        durabilityText.SetText(breakable.hp.ToString());
        loots.ForEach(x => Destroy(x.gameObject));
        loots.Clear();

        for (var i = 0; i < breakable.lootOnHit.Length; i++)
        {
            var loot = breakable.lootOnHit[i];
            var newLootInspection = Instantiate(lootInspection.gameObject, lootsData)
               .GetComponent<LootInspection>();
            newLootInspection.gameObject.SetActive(true);
            newLootInspection.horizontalLayout.reverseArrangement = i % 2 != 0;
            newLootInspection.UpdateData(loot.itemId, loot.lootMaxCount, loot.lootChance);
            loots.Add(newLootInspection);
        }
    }
}
