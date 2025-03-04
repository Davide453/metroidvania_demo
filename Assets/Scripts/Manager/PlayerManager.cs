using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int CurrentCurrency { get; set; }
    public float CurrentHealth { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else { Destroy(gameObject); }
    }

    public void IncrementCurrency(int amount)
    {
        CurrentCurrency += amount;
        HUDManager.instance.UpdateCurrency(CurrentCurrency);
    }
    public void RemoveCurrency(int amount)
    {
        CurrentCurrency -= amount;
        HUDManager.instance.UpdateCurrency(CurrentCurrency);
    }
    public void IncrementHealth(float amount)
    {
        CurrentHealth += amount;
        HUDManager.instance.UpdateHealth(CurrentHealth);
    }
    public void RemoveHealth(float amount)
    {
        CurrentHealth -= amount;
        HUDManager.instance.UpdateHealth(CurrentHealth);
    }

}
