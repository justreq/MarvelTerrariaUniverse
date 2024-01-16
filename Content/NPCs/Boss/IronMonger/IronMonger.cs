using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using Terraria.Utilities;

namespace MarvelTerrariaUniverse.Content.NPCs.Boss.IronMonger
{
    [AutoloadBossHead]
    public class IronMonger : ModNPC //https://github.com/tModLoader/tModLoader/blob/1.4.4/ExampleMod/Content/NPCs/MinionBoss/MinionBossBody.cs
    {
        public int phase
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public int attack
        {
            get => (int)NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        public Vector2 destination
        {
            get => new Vector2(NPC.ai[2], NPC.ai[3]);
            set
            {
                NPC.ai[2] = value.X;
                NPC.ai[3] = value.Y;
            }
        }

        public Vector2 LastFirstStageDestination { get; set; } = Vector2.Zero;

        public ref float Timer => ref NPC.localAI[0];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1; //update as added
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void SetDefaults()
        {
            NPC.width = 58;
            NPC.height = 72;
            NPC.damage = 50;
            NPC.defense = 20;
            NPC.lifeMax = 5000;
            NPC.knockBackResist = .25f;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.value = Item.buyPrice(gold: 10);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            NPC.aiStyle = -1;
            //if (!Main.dedServ)
            //{
            //    Music = MusicLoader.GetMusicSlot(Mod, "");
            //}
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("A former father figure now obscured in greed for power.")
            });
        }
        //need to update with loot
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            base.ModifyNPCLoot(npcLoot);
        }
        //need to update
        public override void OnKill()
        {
            base.OnKill();
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return true;
        }
    }
}
