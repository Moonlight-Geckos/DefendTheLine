using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBar : MonoBehaviour
{
    private ProgressBar _spawnBar;

    private bool _rech;
    private void Awake()
    {
        _spawnBar = GetComponent<ProgressBar>();
        _spawnBar.UpdateValue(1);
        _rech = false;

        EventsPool.ShouldSpawnBlobEvent.AddListener(Spawn);
    }
    private void Spawn()
    {
        if (_rech)
            return;
        _spawnBar.UpdateValue(_spawnBar.Value - 0.2f);


        if (_spawnBar.Value <= 0.01f)
        {
            EventsPool.RechargingEvent.Invoke();
            StartCoroutine("recharge");
        }
    }
    IEnumerator recharge()
    {
        float time = 0;
        _rech = true;
        while (time < 3)
        {
            _spawnBar.UpdateValue(_spawnBar.Value + Time.deltaTime / 3);
            time += Time.deltaTime;
            yield return null;
        }
        _rech = false;

        EventsPool.RechargedEvent.Invoke();
    }
}
