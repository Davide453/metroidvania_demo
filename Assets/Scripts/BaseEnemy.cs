using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{

    public float MaxHealth { get; set; } = 5;
    public float CurrentHealth { get; set; }

    public void Damage(float damageAmount, Vector2 pos)
    {
        CurrentHealth -= damageAmount;
        Debug.Log(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
