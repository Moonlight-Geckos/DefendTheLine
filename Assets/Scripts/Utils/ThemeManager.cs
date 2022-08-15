using System;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    [Serializable]
    public class ColorPalette
    {
        public Color[] colors;
    }
    [SerializeField]
    private int randomizerSeed = 5713;

    [SerializeField]
    private ColorPalette[] palettes;

    [SerializeField]
    private List<Material> blobMaterials;

    private static ThemeManager _instance;
    private static ColorPalette _currentActivePalette;

    public static ThemeManager Instance
    {
        get { return _instance; }
    }
    public ColorPalette ActivePalette
    {
        get { return _currentActivePalette; }
    }
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            _currentActivePalette = palettes[UnityEngine.Random.Range(0, palettes.Length)];
            blobMaterials.Sort((x,y) => int.Parse(x.name).CompareTo(int.Parse(y.name)));
        }
    }

    public Material GetBlobMaterialByLevel(int x)
    {
        x = Mathf.Min(x, blobMaterials.Count);

        return blobMaterials[x];
    }
    public float Sigmoid01(double value)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-value + 2));
    }
}
