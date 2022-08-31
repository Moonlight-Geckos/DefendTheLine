using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlobSpawnButton : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Button _button;

    private string _canSpawnText = "+1";
    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _button = GetComponent<Button>();

        EventsPool.RechargingEvent.AddListener(DisableButton);
        EventsPool.RechargedEvent.AddListener(EnableButton);
    }
    private void EnableButton()
    {
        _button.interactable = true;
        _text.text = _canSpawnText;
    }
    private void DisableButton()
    {
        _button.interactable = false;
    }

}
