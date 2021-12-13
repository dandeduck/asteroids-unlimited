using UnityEngine;

public class PlayerMoveable : MonoBehaviour
{
    [SerializeReference] private Level currentLevel;
    [SerializeField] private float regularSpeed;
    [SerializeField] private float increasedSpeed;

    private void Awake()
    {
        transform.position = currentLevel.GetPlayerSpawn();
    }
    
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float speed = Input.GetKey(KeyCode.LeftShift) ? increasedSpeed : regularSpeed;

        speed *= Time.deltaTime;

        MoveHorizontally(horizontal, speed);
        MoveVertically(vertical, speed);
    }

    private void MoveHorizontally(float input, float speed)
    {
        if (input > 0)
            transform.position = new Vector3(Mathf.Min(transform.position.x + speed, currentLevel.GetBorder().endX), transform.position.y, transform.position.z);
        else if (input < 0)
            transform.position = new Vector3(Mathf.Max(transform.position.x - speed, currentLevel.GetBorder().startX), transform.position.y, transform.position.z);
    }

    private void MoveVertically(float input, float speed)
    {
        Debug.Log(input + " " + (transform.position.z + speed) + " " + currentLevel.GetBorder().endZ);
        if (input > 0)
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Min(transform.position.z + speed, currentLevel.GetBorder().endZ));
        else if (input < 0)
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Max(transform.position.z - speed, currentLevel.GetBorder().startZ));
    }
}
