using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;


public class TimersPool : MonoBehaviour
{
    [SerializeField]
    int maxPoolSize = 100;


    private static TimersPool _instance;
    private static LinkedPool<Timer> _pool;
    private static UnityEvent<float> _updateTimersEvent;
    
    public static TimersPool Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        _pool = new LinkedPool<Timer>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            null,
            false,
            maxPoolSize
        );
        _pool.Clear();
        _updateTimersEvent = new UnityEvent<float>();
        EventsPool.ClearPoolsEvent.AddListener(ClearPool);
    }
    public IObjectPool<Timer> Pool
    {
        get { return _pool; }
    }
    private Timer CreatePooledItem()
    {
        Timer timer = new Timer();
        return timer;
    }
    private void OnReturnedToPool(Timer timer)
    {
        timer.Stop();
        timer.Available = false;
        _updateTimersEvent.RemoveListener(timer.Update);
    }
    private void OnTakeFromPool(Timer timer)
    {
        timer.Stop();
        timer.Available = true;
        _updateTimersEvent.AddListener(timer.Update);
    }
    public void ClearPool()
    {
        _pool.Clear();
    }
    private void Update()
    {
        _updateTimersEvent.Invoke(Time.deltaTime);
    }
}