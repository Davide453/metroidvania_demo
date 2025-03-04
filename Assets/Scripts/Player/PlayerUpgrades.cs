using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public bool DashUpgradeUnlocked { get; set; }
    public bool SwimUpgradeUnlocked { get; set; }


    private void Awake()
    {
        DashUpgradeUnlocked = false;
        SwimUpgradeUnlocked = false;
    }

    public void UnlockDash()
    {
        DashUpgradeUnlocked = true;
    }
    public void UnlockSwim()
    {
        DashUpgradeUnlocked = true;
    }

}
