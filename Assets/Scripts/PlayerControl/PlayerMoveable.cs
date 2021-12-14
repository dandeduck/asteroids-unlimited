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
    
    private void LateUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float speed = Input.GetKey(KeyCode.LeftShift) ? increasedSpeed : regularSpeed;

        speed *= Time.deltaTime;

        Vector3 travelDistance = HorizontalDistance(horizontal, speed) + VerticalDistance(vertical, speed);
        transform.position = CalcEndPosition(travelDistance);
    }

    private Vector3 HorizontalDistance (float input, float speed)
    {
        if (input > 0)
            return transform.right * speed;
        if (input < 0)
            return transform.right * -speed;
        
        return Vector3.zero;
    }

    private Vector3 VerticalDistance (float input, float speed)
    {
        if (input > 0)
            return transform.forward * speed;
        if (input < 0)
            return transform.forward * -speed;
        
        return Vector3.zero;
    }

    private Vector3 CalcEndPosition(Vector3 travelDistance)
    {
        Vector3 endPosition = transform.position + travelDistance;

        if (endPosition.x > currentLevel.GetBorder().endX)
            endPosition.x = currentLevel.GetBorder().endX;
        else if (endPosition.x < currentLevel.GetBorder().startX)
            endPosition.x = currentLevel.GetBorder().startX;

        if (endPosition.z > currentLevel.GetBorder().endZ)
            endPosition.z = currentLevel.GetBorder().endZ;
        else if (endPosition.z < currentLevel.GetBorder().startZ)
            endPosition.z = currentLevel.GetBorder().startZ;

        return endPosition;
    }
}
