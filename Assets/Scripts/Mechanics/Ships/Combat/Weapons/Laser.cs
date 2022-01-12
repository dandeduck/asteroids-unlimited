using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private MeshRenderer meshRenderer;
    private Vector3 startingPosition;
    private float length;
    private Coroutine shootingCoroutine;
    private bool isShooting;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        length = meshRenderer.bounds.size.z;

        Reset();
    }

    public void SetMaterial(Material material)
    {
        GetComponentInChildren<Renderer>().material = material;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public bool IsShooting()
    {
        return isShooting;
    }

    public void Shoot(Ship target)
    {
        Reset();
        meshRenderer.enabled = true;
        
        if (shootingCoroutine != null)
            StopCoroutine(shootingCoroutine);

        target.AddDeathListener(ship => {StopCoroutine(shootingCoroutine); Reset();});
        shootingCoroutine = StartCoroutine(ShootEnumerator(target));
    }

    private IEnumerator ShootEnumerator(Ship target)
    {
        isShooting = true;

        Vector3 sourcePosition = transform.position;
        float maxDistance = (target.transform.position - sourcePosition).magnitude;

        while(target != null && !HitShip(target.transform.position - transform.forward * speed * Time.fixedDeltaTime, sourcePosition) && !ShouldDisappear(maxDistance))
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.position += transform.forward * speed * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        Reset();

        if (target != null)
            target.TakeDamage(damage);
    }

    private void Reset()
    {
        transform.localPosition = startingPosition;
        meshRenderer.enabled = false;
        isShooting = false;
    }

    private bool HitShip(Vector3 target, Vector3 sourcePosition)
    {
        if (target != null)
            return ((transform.position - sourcePosition).magnitude + length / 2) >= (target - sourcePosition).magnitude;
        return true;
    }

    private bool ShouldDisappear(float maxDistance)
    {
        return (transform.localPosition - startingPosition).magnitude >= maxDistance;
    }
}
