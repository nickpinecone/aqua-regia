using WaterGuns.Projectiles.AI;

namespace WaterGuns.Projectiles.Modules;

public class ArtificialModule : BaseProjectileModule
{
    public BaseAI CurrentAI { get; private set; }

    public ArtificialModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
        CurrentAI = new NoAI(baseProjectile);
    }

    public void SetAI(BaseAI ai)
    {
        CurrentAI = ai;

        CurrentAI.OnSwitchAI += (_, toAi) => SetAI(toAi);
    }

    public AIData Update(AIData aiData)
    {
        return CurrentAI.Update(aiData);
    }

    public override void RuntimeAI()
    {
        base.RuntimeAI();

        var data = AIData.FromProjectile(_baseProjectile);
        var result = Update(data);
        result.ApplyToProjectile(_baseProjectile);
    }
}