using TMPro;
using UnityEngine;

public class CoinHolder : MonoBehaviour
{
    TextMeshProUGUI coinsText;

    private void Awake()
    {
        coinsText = GetComponentInChildren<TextMeshProUGUI>();
        EventsPool.UpdateUIEvent.AddListener(() =>
        {
            coinsText.text = Mathf.Min(9999, PlayerStorage.CoinsCollected).ToString();
        });
    }
    private void OnEnable()
    {
        coinsText.text = Mathf.Min(9999, PlayerStorage.CoinsCollected).ToString();
    }

}
