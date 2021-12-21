using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private float borderStartX;
    [SerializeField] private float borderStartZ;
    [SerializeField] private float borderEndX;
    [SerializeField] private float borderEndZ;

    [SerializeField] private Vector2 playerSpawn;

    private Border border;
    
    private void Awake()
    {
        border = new Border(borderStartX, borderEndX, borderStartZ, borderEndZ);
    }

    public Border GetBorder()
    {
        return border;
    }

    public Vector3 GetPlayerSpawn()
    {
        return new Vector3(playerSpawn.x, 0, playerSpawn.y);
    }
}

public class Border 
{
    public float startX { get; }
    public float endX { get; }
    public float startZ { get; }
    public float endZ { get; }

    public Border(float startX, float endX, float startZ, float endZ)
    {
        this.startX = startX;
        this.endX = endX;
        this.startZ = startZ;
        this.endZ = endZ;
    }
}
