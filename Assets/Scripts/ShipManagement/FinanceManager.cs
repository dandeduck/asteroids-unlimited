using UnityEngine;

public class FinanceManager : MonoBehaviour
{
    [SerializeField] private float passiveIncome;

    private float balance = 0;

    private void Update()
    {
        balance += passiveIncome * Time.deltaTime;
    }

    public void AddMoney(float amount)
    {
        balance += amount;
    }

    public bool Spend(float amount)
    {
        if (balance >= amount)
        {
            balance -= amount;

            return true;
        }

        return false;
    }
}
