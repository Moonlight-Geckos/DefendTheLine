using UnityEngine;

public abstract class Target : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    private float _health;
    private int id = -1;
    private IDisposable _disposable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag[0] == 'S')
        {
            EventsPool.EnemySpawnedEvent.Invoke(this);
        }
    }
    public void GetDamage(float damage)
    {
        if (_health <= 0)
            return;
        _health -= damage;
        if (_health <= 0)
        {
            DeadVisuals();
            Dispose();
        }
        else
        {
            HitVisuals();
        }
    }
    public virtual void Initialize()
    {
        _health = maxHealth;
        if(id < 0)
            id = Observer.UniqueID;
    }
    protected void Dispose()
    {
        if(_disposable == null)
        {
            _disposable = GetComponent<IDisposable>();
        }
        EventsPool.EnemyDiedEvent.Invoke(this);
        _disposable.Dispose();
    }
    protected abstract void DeadVisuals();
    protected abstract void HitVisuals();
    public override int GetHashCode()
    {
        return id;
    }
    public override bool Equals(object other)
    {
        return ((Target)other).id == id;
    }
}
