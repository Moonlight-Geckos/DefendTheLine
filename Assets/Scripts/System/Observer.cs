using UnityEngine;
public class Observer : MonoBehaviour
{
    private static bool _started;
    private static bool _finished;

    private static Observer _instance;
    private static Transform _playerTransform;

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
        }
    }
    private void StartGame()
    {
        _started = true;
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    private void FinishGame(bool w)
    {
        _finished = true;
    }
}
