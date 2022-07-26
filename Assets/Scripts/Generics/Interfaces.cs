using System;

public interface IDisposable
{
    public void Dispose();
}
public interface IDamagable
{
    public void GetDamage(float damage, float cooldown = -1);
    public void StopDamage(float damage);
}