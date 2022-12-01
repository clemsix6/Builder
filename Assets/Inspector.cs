using UnityEngine;

public class Inspector : MonoBehaviour
{
    [SerializeField] private BreakableInspector breakableInspector;
    [SerializeField] private GameObject         bodyPanel;

    private Selectable selected;

    private void Update()
    {
        UpdateSelected();
    }


    private void UpdateSelected()
    {
        if (Equals(Selectable.selected, selected))
            return;
        selected = Selectable.selected;
        bodyPanel.SetActive(true);

        var breakable = selected.gameObject.GetComponent<Breakable>();
        if (breakable != null)
            breakableInspector.UpdateData(breakable);
        else
            bodyPanel.SetActive(false);
    }
}
