using System.Collections.Generic;
using UnityEngine;
public class Observer : MonoBehaviour
{
    private static bool _started;
    private static bool _finished;

    private static Observer _instance;
    private static Transform _playerTransform;
    private static KdTree<Target> _targets;
    private static int uniqueId;
    private static int _currentHealth;
    private static int _availablePoints;
    private static int _enemiesToKill;
    private static int _spawnedEnemies;
    private static int _killedEnemies;

    public bool Finished
    {
        get { return _finished; }
        set { _finished = value; }
    }
    public bool Started
    {
        get { return _started; }
        set { _started = value; }
    }
    public Transform PlayerTransform
    {
        get { return _playerTransform; }
    }
    public static Observer Instance
    {
        get { return _instance; }
    }
    public static int UniqueID
    {
        get { return uniqueId++; }
    }
    public static int AvailablePoints
    {
        get { return _availablePoints; }
        set 
        { 
            _availablePoints = value;
            EventsPool.UpdateUIEvent.Invoke();
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            _started = false;
            _finished = false;

            EventsPool.GameStartedEvent.AddListener(StartGame);
            EventsPool.GameFinishedEvent.AddListener(FinishGame);
            EventsPool.EnemySpawnedEvent.AddListener(AddTarget);
            EventsPool.EnemyDiedEvent.AddListener(EnemyDied);
            EventsPool.TargetDisposedEvent.AddListener(RemoveTarget);
            EventsPool.DamagePlayerEvent.AddListener(PlayerDamaged);
        }
    }
    private void Update()
    {
        if(_targets != null)
            _targets.UpdatePositions();

        if (_started && _enemiesToKill <= 0 && _spawnedEnemies <= 0)
        {
            EventsPool.GameFinishedEvent.Invoke(true);
        }
    }
    public Target GetClosestTarget(Vector3 pos)
    {
        return _targets.FindClosest(pos);
    }
    public IEnumerable<Target> GetCloseTargets(Vector3 pos)
    {
        return _targets.FindClose(pos);
    }
    private void StartGame()
    {
        uniqueId = 0;
        _killedEnemies = 0;
        _enemiesToKill = GameManager.Instance.EnemiesToSpawn;
        _availablePoints = GameManager.Instance.StartingPoints;
        _targets = new KdTree<Target>();
        _currentHealth = GameManager.Instance.MaxPlayerHealth;
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        _started = true;
    }
    private void FinishGame(bool w)
    {
        _started = false;
        _finished = true;
    }
    private void AddTarget(Target target)
    {
        var t = _targets.Count;
        _targets.Add(target);
        if (t == _targets.Count)
        {
            Debug.LogWarning("Didnt add");
            return;
        }
        _enemiesToKill--;
        _spawnedEnemies++;
    }
    private void EnemyDied(Target target)
    {
        _availablePoints++;
        _killedEnemies++;
    }
    private void RemoveTarget(Target target)
    {
        var t = _targets.Count;
        _targets.RemoveAll((x) => x.Equals(target));
        if(t == _targets.Count)
        {
            Debug.LogWarning("Didnt remove");
            return;
        }
        _spawnedEnemies--;
    }
    private void PlayerDamaged(Target t)
    {
        if (_currentHealth <= 0)
            return;

        _currentHealth--;
        if (_currentHealth <= 0)
            EventsPool.GameFinishedEvent.Invoke(false);
    }
}
