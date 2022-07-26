using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    GameObject failPanel;

    [SerializeField]
    GameObject winPanel;

    public void Win()
    {
        winPanel.SetActive(true);
        failPanel.SetActive(false);
    }
    public void Lose()
    {
        winPanel.SetActive(false);
        failPanel.SetActive(true);
    }
}
