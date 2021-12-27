using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private const int DISAPPEARANCE_RANGE = 50;

    [SerializeField] private float speed;
    [SerializeField] private float extendedScale;
    [SerializeField] private float damage;

    private MeshRenderer meshRenderer;
    private Vector3 regularScale;
    private Vector3 startingPosition;
    private float length;
    private Coroutine shootingCoroutine;
    private int notAllyMask;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        length = meshRenderer.bounds.size.z;

        startingPosition = transform.localPosition;
        regularScale = transform.localScale;

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

        while(!HitShip(target, sourcePosition) && !ShouldDisappear())
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.position += transform.forward * speed * Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Min(extendedScale, transform.localScale.z + speed * length / 2));

            yield return null;
        }

        Reset();

        if (target != null)
            target.TakeDamage(damage);
    }

    private void Reset()
    {
        transform.localPosition = startingPosition;
        transform.localScale = regularScale;
        meshRenderer.enabled = false;
    }

    private bool HitShip(Ship target, Vector3 sourcePosition)
    {
        if (target != null)
            return ((transform.position - sourcePosition).magnitude + meshRenderer.bounds.size.z) >= (target.transform.position - sourcePosition).magnitude;
        return true;
    }

    private bool ShouldDisappear()
    {
        return (transform.localPosition - startingPosition).magnitude >= DISAPPEARANCE_RANGE;
    }
}
