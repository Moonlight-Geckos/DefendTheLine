using UnityEngine;

public class ExplodingProjectile : Projectile
{
    private ParticlesPool _explosionParticlesPool;
    private void Awake()
    {
        _explosionParticlesPool = PoolsPool.Instance.ExplosionParticlesPool;
    }
    protected override void MakeDamage()
    {
        var ps = _explosionParticlesPool.Pool.Get();
        ps.Initialize(transform.position + Vector3.up, _damage);
    }
}
