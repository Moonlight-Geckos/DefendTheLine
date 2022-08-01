using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Renderer _renderer;
    protected Target _target;
    protected float _velocity;
    protected Vector3 _direction;
    protected IDisposable _disposable;

    protected bool _canDamage;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
    private void Update()
    {
        if (_target == null)
            return;
        if (!_target.gameObject.activeSelf)
            _canDamage = false;

        transform.position += _direction * _velocity * Time.deltaTime;
        if(transform.position.z >= _target.transform.position.z)
        {

            if (_canDamage)
                MakeDamage();
            Dispose();
        }
    }
    private void Dispose()
    {
        if (_disposable == null)
            _disposable = GetComponent<IDisposable>();
        _target = null;
        _disposable.Dispose();
    }
    protected abstract void MakeDamage();
    public virtual void Initialize(Material mat, Target target, float velocity)
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
        _target = target;

        _direction = (_target.transform.position - transform.position).normalized;

        _renderer.material = mat;
        _velocity = velocity;
        _canDamage = true;
    }
}
