using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    protected ParticleSystem.MainModule _ps;
    protected IDisposable _disposable;
    protected float _damage;
    public void Initialize(Vector3 position, float damage)
    {
        transform.position = position;
        _damage = damage;
    }
    private void Awake()
    {
        var system = GetComponent<ParticleSystem>().main;
        system.stopAction = ParticleSystemStopAction.Callback;
        _ps = GetComponent<ParticleSystem>().main;
        _disposable = GetComponent<IDisposable>();
    }
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Target>()?.GetDamage(_damage);
    }
    void OnParticleSystemStopped()
    {
        if(gameObject.activeSelf)
            _disposable?.Dispose();
    }
}