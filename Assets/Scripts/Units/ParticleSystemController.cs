using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    protected ParticleSystem.MainModule _ps;
    protected IDisposable _disposable;
    protected float _damage;
    public void Initialize(Vector3 position, float damage, Color color)
    {
        transform.position = position;
        _damage = damage;
        _ps.startColor = color;
    }
    private void Awake()
    {
        var system = GetComponent<ParticleSystem>().main;
        system.stopAction = ParticleSystemStopAction.Callback;
        _ps = GetComponent<ParticleSystem>().main;
    }
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Target>()?.GetDamage(_damage);
    }
    void OnParticleSystemStopped()
    {
        if(_disposable == null)
            _disposable = GetComponent<IDisposable>();
        if (gameObject.activeSelf)
            _disposable?.Dispose();
    }
}