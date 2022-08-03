using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemiesPool[] enemiesPools;

    [SerializeField]
    [Range(0f, 10f)]
    private float spawnCooldown;

    private Vector3 _bottomRightCorner;
    private Vector3 _bottomLeftCorner;
    private Vector3 _newPos;

    private Timer _spawnTimer;
    private float _additionalEnemiesHealth;

    private void Awake()
    {
        _additionalEnemiesHealth = 0;

        EventsPool.EnemyDiedEvent.AddListener(IncreaseDifficulty);
    }

    private void Start()
    {
        SetupPositions();
        SetupSpawning();
    }

    private void SetupPositions()
    {
        var camera = Camera.main;

        _bottomRightCorner = MathHelper.GetPointAtHeight(camera.ViewportPointToRay(new Vector3(1, 0, 0)), -2);
        _bottomLeftCorner = MathHelper.GetPointAtHeight(camera.ViewportPointToRay(new Vector3(0, 0, 0)), -2);
        transform.position = 
            (MathHelper.GetPointAtHeight(camera.ViewportPointToRay(new Vector3(1, 1, 0)), transform.position.y) + MathHelper.GetPointAtHeight(camera.ViewportPointToRay(new Vector3(0, 1, 0)), transform.position.y))
            / 2;
        _newPos = transform.position + Vector3.forward * 3f;
    }
    private void SetupSpawning()
    {
        _spawnTimer = TimersPool.Instance.Pool.Get();
        _spawnTimer.AddTimerFinishedEventListener(Spawn);
        _spawnTimer.Duration = spawnCooldown;

        EventsPool.GameStartedEvent.AddListener(Spawn);
    }
    private void Spawn()
    {
        _newPos.x = Random.Range(_bottomLeftCorner.x + 0.5f, _bottomRightCorner.x - 0.5f);
        var enemy = enemiesPools[Random.Range(0, enemiesPools.Length)].Pool.Get();
        enemy.Initialize(_newPos, _additionalEnemiesHealth);
        enemy.transform.LookAt(-Vector3.forward);

        _spawnTimer.Run();
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
}