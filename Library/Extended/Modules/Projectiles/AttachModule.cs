using System;
using AquaRegia.Library.Extended.Extensions;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class AttachModule : IModule, IProjectileRuntime
{
    public bool CanAttachToNPC { get; set; } = true;
    public bool CanAttachToTile { get; set; } = true;

    public Vector2 Offset { get; private set; }
    public NPC? Target { get; private set; }
    public Projectile? Projectile { get; private set; }

    public bool Attached => Target is not null || Projectile is not null;

    public Action? OnAttach { get; set; }
    public Action? OnTileAttach { get; set; }
    public Action? OnNpcAttach { get; set; }

    public void SetDefaults(bool canAttachToNPC = true, bool canAttachToTile = true)
    {
        CanAttachToNPC = canAttachToNPC;
        CanAttachToTile = canAttachToTile;
    }

    public bool CanAttach(NPC target)
    {
        return CanAttachToNPC &&
               (Projectile is null || !Projectile.IsTileCollide()) &&
               (Target is null || !Target.CanBeChasedBy()) &&
               target.CanBeChasedBy();
    }

    public bool CanAttach(Projectile projectile)
    {
        return CanAttachToTile &&
               (Target is null || !Target.CanBeChasedBy()) &&
               (Projectile is null || !Projectile.IsTileCollide()) &&
               projectile.IsTileCollide();
    }

    private void AttachCommon(Projectile projectile)
    {
        projectile.velocity = Vector2.Zero;
        OnAttach?.Invoke();
    }

    public void Attach(Projectile projectile, NPC target)
    {
        AttachCommon(projectile);
        OnNpcAttach?.Invoke();

        Projectile = null;
        Target = target;
        Offset = projectile.Center - target.Center;
    }

    public void Attach(Projectile projectile)
    {
        AttachCommon(projectile);
        OnTileAttach?.Invoke();

        Target = null;
        Projectile = projectile;
    }

    public void ApplyAttach(Projectile projectile)
    {
        if (CanAttach(projectile))
        {
            Attach(projectile);
        }

        if (Target != null)
        {
            if (!Target.CanBeChasedBy())
            {
                Offset = Vector2.Zero;
                Target = null;
                return;
            }

            projectile.Center = Target.Center + Offset;
        }

        if (Projectile != null && !Projectile.IsTileCollide())
        {
            Offset = Vector2.Zero;
            Projectile = null;
        }
    }

    public void RuntimeOnHitNPC(BaseProjectile baseProjectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (CanAttach(target))
        {
            Attach(baseProjectile.Projectile, target);
        }
    }

    public void RuntimeAI(BaseProjectile baseProjectile)
    {
        ApplyAttach(baseProjectile.Projectile);
    }
}