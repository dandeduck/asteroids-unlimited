using UnityEngine;

public static class VectorUtil
{
    private static Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

    public static Vector2 Abs(Vector2 vector)
    {
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }

    public static Vector3 ScreenPosToGround(Vector2 screen)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
        float distance = 0; 
        
        if (groundPlane.Raycast(ray, out distance))
            return ray.GetPoint(distance);

        return Vector3.zero;
    }
}
