using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNumber : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        Destroy(this);
    }
}
