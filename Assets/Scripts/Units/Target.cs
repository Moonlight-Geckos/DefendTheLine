using UnityEngine;

public abstract class Target : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    protected float _health;
    protected int id = -1;
    protected IDisposable _disposable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag[0] == 'S')
        {
            EventsPool.EnemySpawnedEvent.Invoke(this);
        }
        else if(other.tag[0] == 'D')
        {
            DamagePlayer();
        }
        else if(other.tag[0] == 'P')
        {
            TurnTo(other.transform.forward);
        }
    }
    public void GetDamage(float damage)
    {
        if (_health <= 0)
            return;
        _health -= damage;
        if (_health <= 0)
        {
            EventsPool.EnemyDiedEvent.Invoke(this);
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
        if (id < 0)
            id = Observer.UniqueID;
    }
    public override int GetHashCode()
    {
        return id;
    }
    public override bool Equals(object other)
    {
        return ((Target)other).id == id;
    }
    protected virtual void Dispose()
    {
        if (_disposable == null)
        {
            _disposable = GetComponent<IDisposable>();
        }
        _disposable.Dispose();
        EventsPool.TargetDisposedEvent.Invoke(this);
    }
    protected abstract void DeadVisuals();
    protected abstract void HitVisuals();
    protected abstract void TurnTo(Vector3 direction);
    protected virtual void DamagePlayer()
    {
        EventsPool.DamagePlayerEvent.Invoke(this);
        DeadVisuals();
        Dispose();
    }
}
