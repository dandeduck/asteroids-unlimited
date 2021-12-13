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
        targetY = transform.position.y;
        angleX = transform.eulerAngles.x;
    }

    private void LateUpdate()
    {
        SetZoom();

        Vector3 targetPosition = new Vector3(transform.position.x, targetY, CalcNewZ());
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * zoomDamping);
        // transform.position = targetPosition;
    }

    private float CalcNewZ()
    {
        return Mathf.Tan(angleX) * targetY;
    }

    private void SetZoom()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            ZoomIn();
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
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
