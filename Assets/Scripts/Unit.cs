using UnityEngine;

public interface Unit
{
    void OnSelect();
    void OnDeselect();
    
    void OnAttack(Unit unit);
    void OnMove(Vector3 position, int arrivalIndex, int arrivalAmount);
    void OnMove(Vector3 position)
    {
        OnMove(position, 0, 1);
    }

    void OnKill();
    bool OnDamageTaken(float damage);

    Transform Transform();
    
    int Id()
    {
        return Transform().GetInstanceID();
    }
}
