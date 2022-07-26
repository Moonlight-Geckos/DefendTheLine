using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

class ReturnToPool : MonoBehaviour, IDisposable
{
    public UnityAction releaseAction;
    public void Dispose()
    {
        releaseAction.Invoke();
    }
}
public abstract class GenericMonoBehaviourPool<T> : ScriptableObject
    where T : MonoBehaviour
{
    #region Serialized

    [SerializeField]
    private GameObject prefab;

    [Range(10, 10000)]
    [SerializeField]
    int maxPoolSize = 100;

    #endregion

    private GameObject itemsParent;
    private ObjectPool<T> _pool;
    public IObjectPool<T> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<T>(
                    CreatePooledItem,
                    OnTakeFromPool,
                    OnReturnedToPool,
                    OnDestroyPoolObject,
                    true,
                    maxPoolSize);
                EventsPool.ClearPoolsEvent.AddListener(ClearPool);
            }

            return _pool;
        }
    }
    protected virtual T CreatePooledItem()
    {
        GameObject obj = Instantiate(prefab);
        ReturnToPool u = obj.AddComponent<ReturnToPool>();
        var y = obj.GetComponent<T>();
        u.releaseAction = () =>
        {
            Pool.Release(y);
        };
        if (!itemsParent)
        {
            itemsParent = GameObject.Find(prefab.name + " Parent");
            itemsParent = new GameObject();
            itemsParent.name = prefab.name + " Parent";
        }
        obj.transform.parent = itemsParent.transform;
        return obj.GetComponent<T>();
    }
    protected virtual void OnReturnedToPool(T obj)
    {
        if (obj != null)
            obj.gameObject.SetActive(false);
    }
    protected virtual void OnTakeFromPool(T obj)
    {
        if (obj != null)
            obj.gameObject.SetActive(true);
    }
    protected virtual void OnDestroyPoolObject(T obj)
    {
        if(obj != null)
            Destroy(obj.gameObject);
    }
    public virtual void ClearPool()
    {
        _pool.Clear();
    }
}