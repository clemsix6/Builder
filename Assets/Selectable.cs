using UnityEngine;

public class Selectable : MonoBehaviour
{
    public static Selectable selected;

    [SerializeField] private Actor            actor;
    [SerializeField] private SpriteRenderer[] renderers;

    public Color selectedColor   = new(1, 0.5f, 0.5f, 1);
    public Color unselectedColor = new(1, 1, 1, 1);
    public bool  isSelected      = false;


    private void UpdateColor()
    {
        foreach (var renderer in renderers)
            renderer.color = isSelected ? selectedColor : unselectedColor;
    }


    private void OnMouseDown()
    {
        selected = this;
    }


    private void OnMouseEnter()
    {
        isSelected = true;
        UpdateColor();
    }


    private void OnMouseExit()
    {
        isSelected = false;
        UpdateColor();
    }
}
