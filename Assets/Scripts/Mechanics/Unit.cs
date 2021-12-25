using UnityEngine;
using UnityEngine.Events;

public interface Ship
{
    void OnSelect();
    void OnDeselect();

    void Move(Vector3 position, int arrivalIndex, int arrivalAmount);
    void Move(Vector3 position)
    {
        Move(position, 0, 0);
    }

    void Attack(Ship unit, bool chase);
    void OnKill();
    void TakeDamage(float damage);
    void StopCombat();
    bool IsAlive();

    GameObject Object();
    int Id()
    {
        return Object().GetInstanceID();
    }

    void AddDeathListener(UnityAction action);
}
