public class Zicke : Enemy
{
    public override void OnDamageTaken()
    {
        maxVelocity *= 2;
    }
}