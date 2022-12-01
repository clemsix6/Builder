using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootInspection : MonoBehaviour
{
    [SerializeField] public  HorizontalLayoutGroup horizontalLayout;
    [SerializeField] private Image                 icon;
    [SerializeField] private TMP_Text              countText;


    public void UpdateData(string itemId, int maxCount, float probability)
    {
        UpdateIcon(itemId);
        countText.SetText(maxCount                   + "\n" +
                          Math.Round(probability, 2) + "%");
    }


    private void UpdateIcon(string itemId)
    {
        var sprite = Resources.Load<Sprite>("Icons/" + itemId);
        if (sprite == null)
            return;
        icon.sprite = sprite;
    }
}
