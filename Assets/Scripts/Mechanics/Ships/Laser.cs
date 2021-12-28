using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private const int DISAPPEARANCE_RANGE = 50;

    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private MeshRenderer meshRenderer;
    private Vector3 startingPosition;
    private float length;
    private Coroutine shootingCoroutine;
    private int notAllyMask;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        length = meshRenderer.bounds.size.z;

        startingPosition = transform.localPosition;

        notAllyMask = ~(1 << gameObject.layer);
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
        Vector3 sourcePosition = transform.position;

        while(!HitShip(target.transform.position - transform.forward * speed * Time.fixedDeltaTime, sourcePosition) && !ShouldDisappear())
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
    }

    private bool HitShip(Vector3 target, Vector3 sourcePosition)
    {
        if (target != null)
            return ((transform.position - sourcePosition).magnitude + meshRenderer.bounds.size.z) >= (target - sourcePosition).magnitude;
        return true;
    }

    private bool ShouldDisappear()
    {
        return (transform.localPosition - startingPosition).magnitude >= DISAPPEARANCE_RANGE;
    }
}
