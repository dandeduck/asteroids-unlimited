using UnityEngine;

public class Zoomable : MonoBehaviour
{
    private const float MAX_OFFSET = 0.1f;

    [SerializeField] private float maxY = 2.5f;
    [SerializeField] private float minY = 0.5f;
    [SerializeField] private float zoomSpeed = 0.3f;
    [SerializeField] private float zoomDamping = 3f;

    private float targetY;
    private float angleX;

    private void Awake()
    {
        targetY = transform.localPosition.y;
        angleX = transform.localEulerAngles.x;
    }

    private void Update()
    {
        SetZoom();

        Vector3 targetPosition = new Vector3(transform.localPosition.x, targetY, CalcNewZ());
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * zoomDamping);
    }

    private float CalcNewZ()
    {
        return Mathf.Tan(angleX) * targetY;
    }

    private void SetZoom()
    {
        if (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.x > 0)
            ZoomIn();
        else if (Input.mouseScrollDelta.y < 0 || Input.mouseScrollDelta.x < 0)
            ZoomOut();
    }

    private void ZoomIn()
    {
        targetY -= zoomSpeed;
        targetY = Mathf.Max(minY, targetY);
    }

    private void ZoomOut()
    {
        targetY += zoomSpeed;
        targetY = Mathf.Min(maxY, targetY);
    }
}
