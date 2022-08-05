using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlobSpawnButton : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Button _button;

    private string _canSpawnText = "+1";
    private int _points, _cost;
    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _button = GetComponent<Button>();
    }
    private void LateUpdate()
    {
        _points = Observer.AvailablePoints;
        _cost = BlobSpawner.SpawnCost;
        if (!_button.interactable)
        {
            if (_points >= _cost)
                EnableButton();
            else
                UpdateValues();
        }
        else if (_points < _cost)
            DisableButton();

    }
    private void EnableButton()
    {
        _button.interactable = true;
        _text.text = _canSpawnText;
    }
    private void DisableButton()
    {
        _button.interactable = false;
        UpdateValues();
    }
    private void UpdateValues()
    {
        _text.text = Observer.AvailablePoints.ToString() + '/' + BlobSpawner.SpawnCost.ToString();
    }

}
