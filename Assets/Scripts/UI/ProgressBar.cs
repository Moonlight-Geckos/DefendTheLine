using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    protected Gradient colorGradient;

    protected Slider slider;
    protected Image image;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        image = slider.fillRect.GetComponent<Image>();
    }

    public void UpdateValue(float val, Color? color = null)
    {
        if(color == null)
            image.color = colorGradient.Evaluate(val);
        else
            image.color = (Color)color;
        slider.value = val;
    }

}