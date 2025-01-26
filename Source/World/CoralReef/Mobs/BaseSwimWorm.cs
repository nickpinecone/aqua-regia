using System;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.World.CoralReef.Mobs;

public enum WormSegmentType
{
    Head,
    Body,
    Tail,
}

public abstract class BaseSwimWorm : ModNPC
{
    public abstract WormSegmentType SegmentType { get; }

    public float MoveSpeed { get; set; }
    public float Acceleration { get; set; }

    public NPC HeadSegment => Main.npc[NPC.realLife];
    public NPC FollowingNPC => SegmentType == WormSegmentType.Head ? null : Main.npc[(int)NPC.ai[1]];
    public NPC FollowerNPC => SegmentType == WormSegmentType.Tail ? null : Main.npc[(int)NPC.ai[0]];

    private bool _didInit = false;

    public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
    {
        return SegmentType == WormSegmentType.Head ? null : false;
    }

    public sealed override bool PreAI()
    {
        if (!_didInit)
        {
            _didInit = true;
            Init();
        }

        if (SegmentType == WormSegmentType.Head)
        {
            HeadAI();

            if (!NPC.HasValidTarget)
            {
                NPC.TargetClosest(true);
            }
        }
        else
        {
            BodyTailAI();
        }

        return true;
    }

    internal virtual void HeadAI()
    {
    }

    internal virtual void BodyTailAI()
    {
    }

    public abstract void Init();
}

public abstract class BaseSwimWormHead : BaseSwimWorm
{
    public sealed override WormSegmentType SegmentType => WormSegmentType.Head;

    public abstract int BodyType { get; }
    public abstract int TailType { get; }

    public int MinSegmentLength { get; set; }
    public int MaxSegmentLength { get; set; }

    public virtual int MaxDistanceForUsingTileCollision => 1000;

    public Vector2? ForcedTargetPosition { get; set; }
    private Vector2? _wanderPoint { get; set; }

    protected int SpawnSegment(IEntitySource source, int type, int latestNPC)
    {
        int oldLatest = latestNPC;
        latestNPC = NPC.NewNPC(source, (int)NPC.Center.X, (int)NPC.Center.Y, type, NPC.whoAmI, 0, latestNPC);

        Main.npc[oldLatest].ai[0] = latestNPC;

        NPC latest = Main.npc[latestNPC];
        latest.realLife = NPC.whoAmI;

        return latestNPC;
    }

    internal sealed override void HeadAI()
    {
        HeadAI_SpawnSegments();

        bool collision = HeadAI_CheckCollision();

        HeadAI_CheckTargetDistance(ref collision);

        HeadAI_Movement(collision);
    }

    private void HeadAI_SpawnSegments()
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            bool hasFollower = NPC.ai[0] > 0;

            if (!hasFollower)
            {
                NPC.realLife = NPC.whoAmI;
                int latestNPC = NPC.whoAmI;

                int randomWormLength = Main.rand.Next(MinSegmentLength, MaxSegmentLength + 1);
                int distance = randomWormLength - 2;

                IEntitySource source = NPC.GetSource_FromAI();

                while (distance > 0)
                {
                    latestNPC = SpawnSegment(source, BodyType, latestNPC);
                    distance--;
                }

                SpawnSegment(source, TailType, latestNPC);

                NPC.netUpdate = true;

                int count = 0;

                foreach (var n in Main.ActiveNPCs)
                {
                    if ((n.type == Type || n.type == BodyType || n.type == TailType) && n.realLife == NPC.whoAmI)
                        count++;
                }

                if (count != randomWormLength)
                {
                    foreach (var n in Main.ActiveNPCs)
                    {
                        if ((n.type == Type || n.type == BodyType || n.type == TailType) && n.realLife == NPC.whoAmI)
                        {
                            n.active = false;
                            n.netUpdate = true;
                        }
                    }
                }

                NPC.TargetClosest(true);
            }
        }
    }

    private bool HeadAI_CheckCollision()
    {
        int minTilePosX = (int)(NPC.Left.X / 16) - 1;
        int maxTilePosX = (int)(NPC.Right.X / 16) + 2;
        int minTilePosY = (int)(NPC.Top.Y / 16) - 1;
        int maxTilePosY = (int)(NPC.Bottom.Y / 16) + 2;

        if (minTilePosX < 0)
            minTilePosX = 0;
        if (maxTilePosX > Main.maxTilesX)
            maxTilePosX = Main.maxTilesX;
        if (minTilePosY < 0)
            minTilePosY = 0;
        if (maxTilePosY > Main.maxTilesY)
            maxTilePosY = Main.maxTilesY;

        bool collision = false;

        for (int i = minTilePosX; i < maxTilePosX; ++i)
        {
            for (int j = minTilePosY; j < maxTilePosY; ++j)
            {
                Tile tile = Main.tile[i, j];

                if (tile.LiquidAmount >= 255)
                {
                    Vector2 tileWorld = new Point16(i, j).ToWorldCoordinates(0, 0);

                    if (NPC.Right.X > tileWorld.X && NPC.Left.X < tileWorld.X + 16 && NPC.Bottom.Y > tileWorld.Y &&
                        NPC.Top.Y < tileWorld.Y + 16)
                    {
                        collision = true;
                    }
                }
            }
        }

        return collision;
    }

    private void HeadAI_CheckTargetDistance(ref bool collision)
    {
        if (!collision)
        {
            Rectangle hitbox = NPC.Hitbox;

            int maxDistance = MaxDistanceForUsingTileCollision;

            bool tooFar = true;

            foreach (var player in Main.ActivePlayers)
            {
                Rectangle areaCheck;

                if (ForcedTargetPosition is Vector2 target)
                    areaCheck = new Rectangle((int)target.X - maxDistance, (int)target.Y - maxDistance, maxDistance * 2,
                                              maxDistance * 2);
                else if (!player.dead && !player.ghost)
                    areaCheck = new Rectangle((int)player.position.X - maxDistance,
                                              (int)player.position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
                else
                    continue; // Not a valid player

                if (hitbox.Intersects(areaCheck))
                {
                    tooFar = false;
                    break;
                }
            }

            if (tooFar)
                collision = true;
        }
    }

    private void HeadAI_Movement(bool collision)
    {
        float speed = MoveSpeed;
        float acceleration = Acceleration;

        float targetXPos, targetYPos;

        Player playerTarget = Main.player[NPC.target];

        Vector2 forcedTarget = ForcedTargetPosition ?? playerTarget.Center;
        (targetXPos, targetYPos) = (forcedTarget.X, forcedTarget.Y);

        Vector2 npcCenter = NPC.Center;

        float targetRoundedPosX = (float)((int)(targetXPos / 16f) * 16);
        float targetRoundedPosY = (float)((int)(targetYPos / 16f) * 16);
        npcCenter.X = (float)((int)(npcCenter.X / 16f) * 16);
        npcCenter.Y = (float)((int)(npcCenter.Y / 16f) * 16);
        float dirX = targetRoundedPosX - npcCenter.X;
        float dirY = targetRoundedPosY - npcCenter.Y;

        float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

        if (!collision)
            HeadAI_Movement_HandleFallingFromNoCollision(dirX, speed, acceleration);
        else
        {
            if (TileHelper.AnySolidInSight(npcCenter, Main.LocalPlayer.Center))
            {
                HeadAI_Movement_Wander(speed, acceleration);
            }
            else
            {
                HeadAI_Movement_HandleMovement(dirX, dirY, length, speed, acceleration);
            }
        }

        HeadAI_Movement_SetRotation(collision);
    }

    private void HeadAI_Movement_Wander(float speed, float acceleration)
    {
        var maxIters = 1000;
        var iters = 0;

        if (_wanderPoint == null || NPC.Center.DistanceSQ((Vector2)_wanderPoint) < 16f * 16f)
        {
            do
            {
                var amountX = Main.rand.Next(128, 256);
                var amountY = Main.rand.Next(128, 256);
                _wanderPoint = NPC.Center + new Vector2(amountX, amountY).RotatedByRandom(MathHelper.Pi * 2);

                iters += 1;
            } while (TileHelper.AnySolidInSight(NPC.Center, (Vector2)_wanderPoint) && iters < maxIters);
        }

        float targetRoundedPosX = (float)((int)(((Vector2)_wanderPoint).X / 16f) * 16);
        float targetRoundedPosY = (float)((int)(((Vector2)_wanderPoint).Y / 16f) * 16);
        var npcCenter = NPC.Center;
        npcCenter.X = (float)((int)(npcCenter.X / 16f) * 16);
        npcCenter.Y = (float)((int)(npcCenter.Y / 16f) * 16);

        var dirX = targetRoundedPosX - npcCenter.X;
        var dirY = targetRoundedPosY - npcCenter.Y;

        float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

        HeadAI_Movement_HandleMovement(dirX, dirY, length, speed / 4f, acceleration / 2f);
    }

    private void HeadAI_Movement_HandleFallingFromNoCollision(float dirX, float speed, float acceleration)
    {
        NPC.TargetClosest(true);
        NPC.velocity.Y += 0.11f;

        if (NPC.velocity.Y > speed)
            NPC.velocity.Y = speed;

        if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.4f)
        {
            if (NPC.velocity.X < 0.0f)
                NPC.velocity.X -= acceleration * 1.1f;
            else
                NPC.velocity.X += acceleration * 1.1f;
        }
        else if (NPC.velocity.Y == speed)
        {
            if (NPC.velocity.X < dirX)
                NPC.velocity.X += acceleration;
            else if (NPC.velocity.X > dirX)
                NPC.velocity.X -= acceleration;
        }
        else if (NPC.velocity.Y > 4)
        {
            if (NPC.velocity.X < 0)
                NPC.velocity.X += acceleration * 0.9f;
            else
                NPC.velocity.X -= acceleration * 0.9f;
        }
    }

    private void HeadAI_Movement_HandleMovement(float dirX, float dirY, float length, float speed, float acceleration)
    {
        float absDirX = Math.Abs(dirX);
        float absDirY = Math.Abs(dirY);
        float newSpeed = speed / length;
        dirX *= newSpeed;
        dirY *= newSpeed;

        if ((NPC.velocity.X > 0 && dirX > 0) || (NPC.velocity.X < 0 && dirX < 0) || (NPC.velocity.Y > 0 && dirY > 0) ||
            (NPC.velocity.Y < 0 && dirY < 0))
        {
            if (NPC.velocity.X < dirX)
                NPC.velocity.X += acceleration;
            else if (NPC.velocity.X > dirX)
                NPC.velocity.X -= acceleration;

            if (NPC.velocity.Y < dirY)
                NPC.velocity.Y += acceleration;
            else if (NPC.velocity.Y > dirY)
                NPC.velocity.Y -= acceleration;

            if (Math.Abs(dirY) < speed * 0.2 && ((NPC.velocity.X > 0 && dirX < 0) || (NPC.velocity.X < 0 && dirX > 0)))
            {
                if (NPC.velocity.Y > 0)
                    NPC.velocity.Y += acceleration * 2f;
                else
                    NPC.velocity.Y -= acceleration * 2f;
            }

            if (Math.Abs(dirX) < speed * 0.2 && ((NPC.velocity.Y > 0 && dirY < 0) || (NPC.velocity.Y < 0 && dirY > 0)))
            {
                if (NPC.velocity.X > 0)
                    NPC.velocity.X = NPC.velocity.X + acceleration * 2f;
                else
                    NPC.velocity.X = NPC.velocity.X - acceleration * 2f;
            }
        }
        else if (absDirX > absDirY)
        {
            if (NPC.velocity.X < dirX)
                NPC.velocity.X += acceleration * 1.1f;
            else if (NPC.velocity.X > dirX)
                NPC.velocity.X -= acceleration * 1.1f;

            if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
            {
                if (NPC.velocity.Y > 0)
                    NPC.velocity.Y += acceleration;
                else
                    NPC.velocity.Y -= acceleration;
            }
        }
        else
        {
            if (NPC.velocity.Y < dirY)
                NPC.velocity.Y += acceleration * 1.1f;
            else if (NPC.velocity.Y > dirY)
                NPC.velocity.Y -= acceleration * 1.1f;

            if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
            {
                if (NPC.velocity.X > 0)
                    NPC.velocity.X += acceleration;
                else
                    NPC.velocity.X -= acceleration;
            }
        }
    }

    private void HeadAI_Movement_SetRotation(bool collision)
    {
        NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
        NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;

        if (collision)
        {
            if (NPC.localAI[0] != 1)
                NPC.netUpdate = true;

            NPC.localAI[0] = 1f;
        }
        else
        {
            if (NPC.localAI[0] != 0)
                NPC.netUpdate = true;

            NPC.localAI[0] = 0f;
        }

        if (((NPC.velocity.X > 0 && NPC.oldVelocity.X < 0) || (NPC.velocity.X < 0 && NPC.oldVelocity.X > 0) ||
             (NPC.velocity.Y > 0 && NPC.oldVelocity.Y < 0) || (NPC.velocity.Y < 0 && NPC.oldVelocity.Y > 0)) &&
            !NPC.justHit)
            NPC.netUpdate = true;
    }
}

public abstract class BaseSwimWormBody : BaseSwimWorm
{
    public sealed override WormSegmentType SegmentType => WormSegmentType.Body;

    internal override void BodyTailAI()
    {
        CommonAI_BodyTail(this);
    }

    internal static void CommonAI_BodyTail(BaseSwimWorm worm)
    {
        worm.NPC.spriteDirection = worm.HeadSegment.velocity.X > 0 ? 1 : -1;

        if (!worm.NPC.HasValidTarget)
            worm.NPC.TargetClosest(true);

        if (Main.player[worm.NPC.target].dead && worm.NPC.timeLeft > 30000)
            worm.NPC.timeLeft = 10;

        NPC following = worm.NPC.ai[1] >= Main.maxNPCs ? null : worm.FollowingNPC;
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            if (following is null || !following.active || following.friendly || following.townNPC ||
                following.lifeMax <= 5)
            {
                worm.NPC.life = 0;
                worm.NPC.HitEffect(0, 10);
                worm.NPC.active = false;
            }
        }

        if (following is not null)
        {
            float dirX = following.Center.X - worm.NPC.Center.X;
            float dirY = following.Center.Y - worm.NPC.Center.Y;
            worm.NPC.rotation = (float)Math.Atan2(dirY, dirX) + MathHelper.PiOver2;
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            float dist = (length - worm.NPC.width) / length;
            float posX = dirX * dist;
            float posY = dirY * dist;

            worm.NPC.velocity = Vector2.Zero;
            worm.NPC.position.X += posX;
            worm.NPC.position.Y += posY;
        }
    }
}

public abstract class BaseSwimWormTail : BaseSwimWorm
{
    public sealed override WormSegmentType SegmentType => WormSegmentType.Tail;

    internal override void BodyTailAI()
    {
        BaseSwimWormBody.CommonAI_BodyTail(this);
    }
}
