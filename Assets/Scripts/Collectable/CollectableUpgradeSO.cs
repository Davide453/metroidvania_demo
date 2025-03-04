using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectable/Player Upgrade", menuName = "New Player Upgrade")]
public class CollectableUpgradeSO : CollectableSOBase
{
    PlayerUpgrades _playerUpgrades;



    [Header("Collectable Stats")]
    [SerializeField] private UpgradeToGivePlayer _upgradeToGivePlayer;


    private enum UpgradeToGivePlayer
    {
        HealthUp,
        AttackUp,
        Dash,
        Swim
    }


    public override void Collect(GameObject objectThatCollected)
    {
    }


    private void GivePowerUp(GameObject objectThatCollected)
    {
        if (_playerUpgrades == null)
        {
            _playerUpgrades = FinderHelper.GetComponentOnObject<PlayerUpgrades>(objectThatCollected);
        }
        switch (_upgradeToGivePlayer)
        {
            case UpgradeToGivePlayer.AttackUp:
                GiveAttackUp();
                break;
            case UpgradeToGivePlayer.Dash:
                GiveDash();
                break;
            case UpgradeToGivePlayer.HealthUp:
                GiveHealthUp();
                break;
            case UpgradeToGivePlayer.Swim:
                GiveSwim();
                break;
        }
    }

    private void GiveSwim()
    {
        _playerUpgrades.UnlockSwim();
    }

    private void GiveHealthUp()
    {
        throw new NotImplementedException();
    }

    private void GiveDash()
    {
        _playerUpgrades.UnlockDash();
    }

    private void GiveAttackUp()
    {
        throw new NotImplementedException();
    }
}
