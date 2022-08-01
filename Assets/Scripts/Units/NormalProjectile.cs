public class NormalProjectile : Projectile
{
    protected override void MakeDamage()
    {
        _target.GetDamage(1);
    }
}