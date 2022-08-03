public class NormalProjectile : Projectile
{
    protected override void MakeDamage()
    {
        _target.GetDamage(_damage);
    }
}