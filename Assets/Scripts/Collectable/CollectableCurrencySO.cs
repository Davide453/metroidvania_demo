using UnityEngine;

[CreateAssetMenu(fileName = "Collectable/Currency", menuName = "New Coin Collectable")]
public class CollectableCurrencySO : CollectableSOBase
{
    [Header("Collectable Stats")]
    public int CurrencyAmount = 1;
    public override void Collect(GameObject objectThatCollected)
    {
        PlayerManager.instance.IncrementCurrency(CurrencyAmount);
    }
}
