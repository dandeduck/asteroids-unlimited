using UnityEngine;

public class Spinning : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + rotationSpeed * Time.deltaTime, 0);
    }
}
