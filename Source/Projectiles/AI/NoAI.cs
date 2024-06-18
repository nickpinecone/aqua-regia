namespace WaterGuns.Projectiles.AI;

public class NoAI : BaseAI
{
    public NoAI(BaseProjectile baseProjectile)
        : base(baseProjectile, AINames.NoAI)
    {
    }

    public override AIData Update(AIData aiData)
    {
        return new AIData();
    }
}