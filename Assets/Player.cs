using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Actor actor;
    [SerializeField] private Pawn pawn;
    
    private List<GameObject> trees = new();


    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            OnClick();
        MoveCamera();
        UpdateHiddenTrees();
    }


    private void OnClick()
    {
        var cursorPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.x = Mathf.RoundToInt(cursorPos.x);
        cursorPos.y = Mathf.RoundToInt(cursorPos.y);
        pawn.Go(cursorPos);
    }


    private void MoveCamera()
    {
        if (Camera.main == null)
            return;
        var cam = Camera.main.transform;
        if (Vector2.Distance(cam.position, transform.position) > 0.2f)
        {
            var pos = Vector2.Lerp(cam.position, transform.position, Time.deltaTime * 2f);
            cam.position = new Vector3(
                pos.x,
                pos.y,
                cam.transform.position.z
            );
        }
    }


    private void UpdateHiddenTrees()
    {
        var currentPos = transform.position;

        foreach (var tree in trees.ToArray())
        {
            if (Vector2.Distance(currentPos, (Vector2)tree.transform.position + Vector2.up * 4) < 7)
                continue;
            var selectable = tree.GetComponent<Selectable>();
            if (selectable == null)
                continue;
            selectable.selectedColor = new Color(
                selectable.selectedColor.r,
                selectable.selectedColor.g,
                selectable.selectedColor.b,
                1
            );
            selectable.unselectedColor = new Color(
                selectable.unselectedColor.r,
                selectable.unselectedColor.g,
                selectable.unselectedColor.b,
                1
            );
            selectable.UpdateColor();
            trees.Remove(tree);
        }

        foreach (var tree in GameObject.FindGameObjectsWithTag("Tree"))
        {
            if (Vector2.Distance(currentPos, (Vector2)tree.transform.position + Vector2.up * 4) > 6 ||
                trees.Contains(tree))
                continue;
            var selectable = tree.GetComponent<Selectable>();
            if (selectable == null)
                continue;
            selectable.selectedColor = new Color(
                selectable.selectedColor.r,
                selectable.selectedColor.g,
                selectable.selectedColor.b,
                0.5f
            );
            selectable.unselectedColor = new Color(
                selectable.unselectedColor.r,
                selectable.unselectedColor.g,
                selectable.unselectedColor.b,
                0.5f
            );
            selectable.UpdateColor();
            trees.Add(tree);
        }
    }
}
