using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private Image[] _hearts;
    int ind;

    private void Awake()
    {
        _hearts = GetComponentsInChildren<Image>();
        ind = _hearts.Length - 1;

        EventsPool.DamagePlayerEvent.AddListener(Damage);
    }
    private void Damage(Target t)
    {
        if(ind >= 0)
        {
            _hearts[ind--].color *= 0;
        }
    }
}
