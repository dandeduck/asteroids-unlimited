using UnityEngine;

public interface Unit
{
    void OnSelect();
    void OnDeselect();
    
    void OnAction();
    void OnKill();
    bool OnDamageTaken(float damage);

    Transform Transform();
    
    int Id()
    {
        return Transform().GetInstanceID();
    }
}
