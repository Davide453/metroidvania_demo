using System;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public bool DashUpgradeUnlocked { get; set; }
    public bool SwimUpgradeUnlocked { get; set; }
    private Player _player;

    private void Awake()
    {
        DashUpgradeUnlocked = false;
        SwimUpgradeUnlocked = false;
        _player = GetComponent<Player>();
    }

    public void UnlockDash()
    {
        DashUpgradeUnlocked = true;
    }
    public void UnlockSwim()
    {
        DashUpgradeUnlocked = true;
    }

    internal void MaxHealthUp()
    {

        _player.IncrementMaxHealth();

    }
}
