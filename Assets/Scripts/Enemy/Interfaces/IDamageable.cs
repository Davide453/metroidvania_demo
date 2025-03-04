using UnityEngine;

public interface IDamageable
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    void Damage(float damageAmount, Vector2 pos);
    void Die();


}
