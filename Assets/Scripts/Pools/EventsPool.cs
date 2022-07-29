using UnityEngine.Events;

public static class EventsPool
{
    public readonly static UnityEvent ClearPoolsEvent = new UnityEvent();

    public readonly static UnityEvent GameStartedEvent = new UnityEvent();

    public readonly static UnityEvent GamePausedEvent = new UnityEvent();

    public readonly static UnityEvent<bool> GameFinishedEvent = new UnityEvent<bool>();

    public readonly static UnityEvent<SkinItem> UpdateSkinEvent = new UnityEvent<SkinItem>();

    public readonly static UnityEvent UpdateUIEvent = new UnityEvent();

    public readonly static UnityEvent<SwipeDirection> UserSwipedEvent = new UnityEvent<SwipeDirection>();

    public readonly static UnityEvent ShouldSpawnBlobEvent = new UnityEvent();

    public readonly static UnityEvent<Target> EnemySpawnedEvent = new UnityEvent<Target>();

    public readonly static UnityEvent<Target> EnemyDiedEvent = new UnityEvent<Target>();
}