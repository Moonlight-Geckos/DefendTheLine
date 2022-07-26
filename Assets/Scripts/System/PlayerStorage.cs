using UnityEditor;
using UnityEngine;

public static class PlayerStorage
{
    static int skinSelected = PlayerPrefs.GetInt("SelectedStick");
    static int coinsCollected = PlayerPrefs.GetInt("CoinsCollected");
    static int healthUpgrade = PlayerPrefs.GetInt("HealthUpgrade");
    static int damageUpgrade = PlayerPrefs.GetInt("DamageUpgrade");
    static string boughtSkins = PlayerPrefs.GetString("BoughtSkins");

    public static int SkinSelected
    {
        get { return skinSelected; }
        set {
            PlayerPrefs.SetInt("SelectedStick", value);
            skinSelected = value;
        }
    }
    public static int CoinsCollected
    {
        get { return coinsCollected; }
        set
        {
            PlayerPrefs.SetInt("CoinsCollected", value);
            coinsCollected = value;
        }
    }
    public static int DamageUpgradeLevel
    {
        get { return damageUpgrade; }
        set
        {
            PlayerPrefs.SetInt("DamageUpgrade", value);
            damageUpgrade = value;
        }
    }
    public static int HealthUpgradeLevel
    {
        get { return healthUpgrade; }
        set
        {
            PlayerPrefs.SetInt("HealthUpgrade", value);
            healthUpgrade = value;
        }
    }
    public static string BoughtSkins
    {
        get { return boughtSkins; }
        set
        {
            PlayerPrefs.SetString("BoughtSkins", value);
            boughtSkins = value;
        }
    }
}
