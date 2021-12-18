using UnityEngine;

public static class VectorUtil
{
    private static Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

    public static Vector3 Abs(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public static Vector2 Abs(Vector2 vector)
    {
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }

    public static Vector3 ScreenPosToGround(Vector2 screen)
    {
        return ScreenPosToGround(screen, Camera.main);
    }

    public static Vector3 ScreenPosToGround(Vector2 screen, Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(screen);    
        float distance = 0; 
        
        if (groundPlane.Raycast(ray, out distance))
            return ray.GetPoint(distance);

        return Vector3.zero;
    }

    public static bool IsInsideRect(Vector2 pos, Vector2 rectMin, Vector2 rectMax)
    {
        return pos.x > rectMin.x && pos.x < rectMax.x && pos.y > rectMin.y && pos.y < rectMax.y;
    }
}
