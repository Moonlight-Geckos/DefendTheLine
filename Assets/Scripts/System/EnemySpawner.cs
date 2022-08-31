using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemiesPool[] enemiesPools;

    [SerializeField]
    [Range(0f, 10f)]
    private float spawnCooldown;

    private Vector3 _startPos;

    private Timer _spawnTimer;
    private float _additionalEnemiesHealth;
    private float _boundaries;
    private int _enemiesLeftToSpawn;
    private float _chance;

    private void Awake()
    {
        _additionalEnemiesHealth = 0;
        _chance = 0;
        EventsPool.EnemyDiedEvent.AddListener(IncreaseDifficulty);
    }
    private void Start()
    {
        SetupSpawning();
        SetupPositions();
    }

    private void SetupPositions()
    {
        var collider = GetComponent<BoxCollider>();
        _boundaries = collider.size.x / 2f;

        //transform.position = new Vector3((topleft.x + topright.x) / 2f, topleft.y, topleft.z);
    }
    private void SetupSpawning()
    {
        _spawnTimer = TimersPool.Instance.Pool.Get();
        _spawnTimer.AddTimerFinishedEventListener(SpawnWithPattern);
        _spawnTimer.Duration = spawnCooldown;

        _startPos = transform.position + (transform.forward * -5f);

        Debug.DrawLine(_startPos, transform.position, Color.green, 10);

        _enemiesLeftToSpawn = GameManager.Instance.EnemiesToSpawn;

        EventsPool.GameStartedEvent.AddListener(SpawnWithPattern);
    }
    private void SpawnWithPattern()
    {

        if (_enemiesLeftToSpawn <= 0)
            return;

        var rand = Random.Range(0, _chance);
        if(rand <= 50)
            StartCoroutine(fastguys());

        else if (rand <= 80)
            StartCoroutine(midnfast());

        else
            StartCoroutine(allguys());

        if(Observer.EnemiesKilled < GameManager.Instance.EnemiesToSpawn)
            _spawnTimer.Run();
        _chance += spawnCooldown / 2f;
    }
    private void Spawn(Vector3 position, int poolIndex, bool boss)
    {

        var enemy = enemiesPools[poolIndex].Pool.Get();
        enemy.Initialize(position, transform.forward,
            Random.Range(_additionalEnemiesHealth - _additionalEnemiesHealth / 2f, _additionalEnemiesHealth + _additionalEnemiesHealth / 2f),
            boss);
        _enemiesLeftToSpawn--;
    }
    private void IncreaseDifficulty(Target t)
    {
        float ran = Random.Range(0f, 1f);
        if(ran < 0.35f)
        {
            DecreaseCooldown();
        }
        else
        {
            IncreaseEnemiesHealth();
        }
    }
    private void IncreaseEnemiesHealth()
    {
        _additionalEnemiesHealth++;
    }
    private void DecreaseCooldown()
    {
        _spawnTimer.Duration -= 0.05f;
    }
    private IEnumerator fastguys()
    {
        for (int i = 0; i < 6; i++)
        {
            var pos = _startPos + Random.Range(-_boundaries, _boundaries) * transform.right;
            Spawn(pos, 0, false);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator midnfast()
    {
        int spawnedmid = 0;
        for (int i = 0; i < 6; i++)
        {
            int ind = 0;
            if (spawnedmid < 2)
            {
                ind += Random.Range(0, 2);
                spawnedmid += ind;
            }

            var pos = _startPos + Random.Range(-_boundaries, _boundaries) * transform.right;
            Spawn(pos, ind, false);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator allguys()
    {
        for (int i = 0; i < 6; i++)
        {
            var pos = _startPos + Random.Range(-_boundaries, _boundaries) * transform.right;
            Spawn(pos, 0, false);
            yield return new WaitForSeconds(0.2f);
        }
    }
}