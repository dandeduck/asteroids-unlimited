using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    [SerializeField] private float value;

    private Captureable captureable;
    private ShipManager holder;
    private FinanceManager holderFinance;

    private void Awake()
    {
        captureable = GetComponent<Captureable>();
    }

    private void Update()
    {
        ShipManager currentHolder = captureable.GetHolder();

        if (currentHolder != holder)
            holderFinance = currentHolder.GetComponent<FinanceManager>();

        if (holderFinance != null)
            holderFinance.AddMoney(value * Time.deltaTime);
    }
}
