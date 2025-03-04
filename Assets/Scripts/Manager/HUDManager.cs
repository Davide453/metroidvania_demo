using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;
    [SerializeField] private TextMeshProUGUI _currency;
    [SerializeField] private TextMeshProUGUI _health;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else { Destroy(gameObject); }
    }

    public void UpdateCurrency(int currentAmount)
    {
        _currency.text = currentAmount.ToString();
    }
    public void UpdateHealth(float currentAmount)
    {
        _health.text = currentAmount.ToString();
    }

}
