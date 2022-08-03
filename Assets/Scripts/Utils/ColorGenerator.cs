using System;
using UnityEngine;

public class ColorGenerator : MonoBehaviour
{
    [Serializable]
    public class ColorPalette
    {
        public Color[] colors;
    }
    [SerializeField]
    private int randomizerSeed = 5713;

    [SerializeField]
    [Range(0f, 1f)]
    private float hueShifting = 0;

    [SerializeField]
    private ColorPalette[] palettes;

    private static ColorGenerator _instance;
    private static ColorPalette _currentActivePalette;

    public static ColorGenerator Instance
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
        }
    }

    public Tuple<Color, Color> GenerateBlobPalette(int x)
    {
        UnityEngine.Random.InitState(randomizerSeed * x);
        float sig = Mathf.Abs(hueShifting - Sigmoid01(x / 5));
        Color c1 = UnityEngine.Random.ColorHSV(sig, sig, 1, 1, 1, 1);

        sig = Mathf.Abs(hueShifting - Sigmoid01(x % 6));
        Color c2 = UnityEngine.Random.ColorHSV(sig, sig, 1, 1, 1, 1);

        Tuple<Color, Color>  palette = new Tuple<Color, Color>(c1, c2);

        return palette;
    }
    public float Sigmoid01(double value)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-value + 2));
    }
}
