using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    [SerializeField] private float value;

    private Captureable captureable;
    private FinanceManager holderFinance;

    private void Awake()
    {
        captureable = GetComponent<Captureable>();
    }

    private void Update()
    {
        ShipManager currentHolder = captureable.GetHolder();

        if (currentHolder != null && currentHolder.GetFinanceManager() != holderFinance)
            holderFinance = currentHolder.GetFinanceManager();

        if (holderFinance != null)
            holderFinance.AddMoney(value * Time.deltaTime);
    }
}
