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
            EventsPool.EnemyDiedEvent.AddListener(RemoveTarget);
        }
    }
    private void Update()
    {
        if(_targets != null)
            _targets.UpdatePositions();
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
        _started = true;
        _targets = new KdTree<Target>();
        uniqueId = 0;
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    private void FinishGame(bool w)
    {
        _finished = true;
    }
    private void AddTarget(Target target)
    {
        _targets.Add(target);
    }
    private void RemoveTarget(Target target)
    {
        var t = _targets.Count;
        _targets.RemoveAll((x) => x.Equals(target));
        if(t == _targets.Count)
        {
            Debug.LogWarning("Didnt remove");
        }
    }
}
