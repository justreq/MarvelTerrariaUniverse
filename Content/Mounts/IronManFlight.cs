using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Mounts;
public class IronManFlight : ModMount
{
    public override void SetStaticDefaults()
    {
        MountData.totalFrames = 1;
        MountData.heightBoost = 0;
        MountData.flightTimeMax = int.MaxValue;
        MountData.fatigueMax = int.MaxValue;
        MountData.fallDamage = 0f;
        MountData.usesHover = true;
        MountData.runSpeed = 7f;
        MountData.dashSpeed = 7f;
        MountData.acceleration = 0.2f;
        MountData.blockExtraJumps = true;
        int[] array = new int[MountData.totalFrames];
        for (int l = 0; l < array.Length; l++) array[l] = 0;
        MountData.playerYOffsets = new int[] { 0 };
        MountData.xOffset = 0;
        MountData.bodyFrame = 0;
        MountData.yOffset = 0;
        MountData.playerHeadOffset = 0;
        MountData.standingFrameCount = 0;
        MountData.standingFrameDelay = 0;
        MountData.standingFrameStart = 0;
        MountData.runningFrameCount = 0;
        MountData.runningFrameDelay = 0;
        MountData.runningFrameStart = 0;
        MountData.flyingFrameCount = 0;
        MountData.flyingFrameDelay = 0;
        MountData.flyingFrameStart = 0;
        MountData.inAirFrameCount = 0;
        MountData.inAirFrameDelay = 0;
        MountData.inAirFrameStart = 0;
        MountData.idleFrameCount = 0;
        MountData.idleFrameDelay = 0;
        MountData.idleFrameStart = 0;
        MountData.idleFrameLoop = true;
        MountData.swimFrameCount = 0;
        MountData.swimFrameDelay = 0;
        MountData.swimFrameStart = 0;

        if (!Main.dedServ || Main.netMode != NetmodeID.Server)
        {
            MountData.textureWidth = MountData.backTexture.Width();
            MountData.textureHeight = MountData.backTexture.Height();
        }
    }

    public override void SetMount(Player player, ref bool skipDust)
    {
        skipDust = true;
    }

    public override void Dismount(Player player, ref bool skipDust)
    {
        skipDust = true;
    }
}
