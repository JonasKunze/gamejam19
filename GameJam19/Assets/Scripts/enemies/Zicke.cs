public class Zicke : Enemy
{
    public float runningVelocity = 4;
    public override void OnDamageTaken()
    {
        maxVelocity = runningVelocity;
    }
}