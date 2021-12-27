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
    private float spaceToLocalZScale;
    private Coroutine shootingCoroutine;
    private int notAllyMask;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        spaceToLocalZScale = GetComponentInChildren<Renderer>().bounds.size.z;

        startingPosition = transform.localPosition;
        regularScale = transform.localScale;

        notAllyMask = ~(1 << gameObject.layer);
    }

    public void Shoot(Ship target)
    {
        meshRenderer.enabled = true;
        
        if (shootingCoroutine != null)
            StopCoroutine(shootingCoroutine);
        
        shootingCoroutine = StartCoroutine(ShootEnumerator(target));
    }

    private IEnumerator ShootEnumerator(Ship target)
    {
        Vector3 sourcePosition = transform.position;

        while(!HitShip(target, sourcePosition) || !ShouldDisappear())
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.position += transform.forward * speed * Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Min(extendedScale, transform.localScale.z + speed * spaceToLocalZScale / 2));

            yield return null;
        }

        transform.localPosition = startingPosition;
        transform.localScale = regularScale;
        target.TakeDamage(damage);
        meshRenderer.enabled = false;
    }

    private bool HitShip(Ship target, Vector3 sourcePosition)
    {
        return (target.transform.position - sourcePosition).magnitude <= (transform.position - sourcePosition).magnitude;
    }

    // private bool HitShip(Vector3 prevPosition, Ship target) actual hit detection
    // {
    //     if (prevPosition == Vector3.zero)
    //         return false;
        
    //     Vector3 diff = transform.position - prevPosition;

    //     RaycastHit[] hits = Physics.RaycastAll(transform.position, diff, diff.magnitude, notAllyMask, QueryTriggerInteraction.Ignore);

    //     foreach (RaycastHit hit in hits)
    //     {
    //         if (hit.GetComponent<Ship>() == target)
    //              return true;
    //     }
        // return false;
    // }

    private bool ShouldDisappear()
    {
        return (transform.localPosition - startingPosition).magnitude < DISAPPEARANCE_RANGE;
    }
}
