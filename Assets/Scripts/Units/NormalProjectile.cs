using UnityEngine;

public class NormalProjectile : Projectile
{
    private ParticlesPool _dropletsPool;

    public void Awake()
    {
        _dropletsPool = PoolsPool.Instance.DropletsPool;
    }

    protected override void MakeDamage()
    {
        var ps = _dropletsPool.Pool.Get();
        ps.Initialize(transform.position, 0, _renderer.material.color);
        _target.GetDamage(_damage);
    }
}