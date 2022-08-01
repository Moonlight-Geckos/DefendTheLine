using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    protected ParticleSystem.MainModule _ps;
    protected IDisposable _disposable;
    public void Initialize(Vector3 position) => transform.position = position;
    private void Awake()
    {
        var system = GetComponent<ParticleSystem>().main;
        system.stopAction = ParticleSystemStopAction.Callback;
        _ps = GetComponent<ParticleSystem>().main;
        _disposable = GetComponent<IDisposable>();
    }
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Target>()?.GetDamage(1);
    }
    void OnParticleSystemStopped()
    {
        if(gameObject.activeSelf)
            _disposable?.Dispose();
    }
}