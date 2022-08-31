using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    private List<Transform> _variations;
    private int _index;
    private void Awake()
    {
        _variations = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++)
        {
            _variations.Add(transform.GetChild(i));
        }
        _variations.Remove(transform);
        _index = 0;

        EventsPool.DamagePlayerEvent.AddListener(GetHit);
    }

    private void GetHit(Target t)
    {
        if (_index == _variations.Count - 1)
            return;
        _variations[_index++].gameObject.SetActive(false);
        _variations[_index].gameObject.SetActive(true);
    }
}
