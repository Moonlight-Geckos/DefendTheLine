using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    private Outline outline;
    private Image skinImage;
    private TextMeshProUGUI priceText;
    private EventTrigger eventTrigger;

    private Color activeColor = Color.white;
    private Color inactiveColor = new Color(0, 0, 0, 0.3f);

    public void Initialize(SkinItem skin, UnityAction selectAction)
    {
        if(outline == null)
        {
            outline = GetComponent<Outline>();
            priceText = GetComponentInChildren<TextMeshProUGUI>();
            skinImage = GetComponentsInChildren<Image>()[1];
            eventTrigger = GetComponent<EventTrigger>();
        }
        priceText.gameObject.SetActive(true);
        priceText.text = skin.price.ToString();
        skinImage.sprite = skin.skinSprite;
        skinImage.color = Color.white;
        skinImage.preserveAspect = true;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { selectAction.Invoke(); });
        eventTrigger.triggers.Add(entry);
    }
    public void Select() => outline.effectColor = activeColor;
    public void UnSelect() => outline.effectColor = inactiveColor;
    public void HidePrice() => priceText.gameObject.SetActive(false);
}
