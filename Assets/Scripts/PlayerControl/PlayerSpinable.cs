using UnityEngine;

public class PlayerSpinable : MonoBehaviour
{
    [SerializeField] private float regularSpeed;
    [SerializeField] private float increasedSpeed;

    private void Update()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? increasedSpeed : regularSpeed;
        speed *= Time.deltaTime;

        if (Input.GetKey(KeyCode.E))
            speed *= -1;
        if (speed > 0 && !Input.GetKey(KeyCode.Q))
            speed = 0;

        transform.Rotate(Vector3.up * speed);
    }
}
