using TMPro;
using UnityEngine;

public class BlobLevel : MonoBehaviour
{
    private Blob _blob;
    private TextMeshProUGUI _tmpro;
    private Transform _cameraTransform;
    private Vector3 _vec;
    private Color _textColor;
    private int _currentLevel;
    private void Start()
    {
        _blob = GetComponentInParent<Blob>();
        _tmpro = GetComponentInChildren<TextMeshProUGUI>();
        _cameraTransform = Camera.main.transform;

        _currentLevel = _blob.Level;
        InitializeText();
    }
    void LateUpdate()
    {
        _vec = _cameraTransform.transform.position - transform.position;
        transform.LookAt(_cameraTransform.transform.position - _vec);

        if (_currentLevel != _blob.Level)
        {
            _currentLevel = _blob.Level;
            InitializeText();
        }
    }
    private void InitializeText()
    {
        _tmpro.text = (_currentLevel + 1).ToString();
        _textColor = _blob.MainColor.grayscale < 0.5f ? Color.white : Color.black;
        _tmpro.color = _textColor;
    }
}
