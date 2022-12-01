using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float  zoomSensibility = 20f;
    [SerializeField] private float  minZoom         = 5f;
    [SerializeField] private float  maxZoom         = 40f;

    private Vector2 lastMovePoint;


    private void Update()
    {
        StartMove();
        MoveCamera();
        ZoomCamera();
    }


    private void StartMove()
    {
        if (!Input.GetButtonDown("Fire3")) return;
        lastMovePoint = camera.ScreenToWorldPoint(Input.mousePosition);
    }


    private void MoveCamera()
    {
        if (!Input.GetButton("Fire3")) return;
        var currentPoint = (Vector2)camera.ScreenToWorldPoint(Input.mousePosition);
        var diff         = lastMovePoint - currentPoint;
        transform.Translate(diff.x, diff.y, 0);
    }


    private void ZoomCamera()
    {
        var scroll = -Input.GetAxis("Mouse ScrollWheel") * zoomSensibility;

        if ((scroll < 0 && camera.orthographicSize > minZoom) || 
            (scroll > 0 && camera.orthographicSize < maxZoom))
            camera.orthographicSize += scroll;
    }
}
