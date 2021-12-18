using UnityEngine;

public interface Unit
{
    void OnSelect();
    void OnDeselect();
    
    void OnAction();
    void OnKill();
    bool OnDamageTaken(float damage);

    int Id();
    Transform Transform();
}
