using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.ID;

namespace AtlayasMod.Content.NPCs.Minibosses
{
    public class CinderClaw : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 7;
        }
        public override void SetDefaults()
        {
            NPC.width = 82;
            NPC.height = 55;
            NPC.damage = 30;
            NPC.defense = 30;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCDeath8;
            NPC.DeathSound = SoundID.NPCDeath5;
            NPC.value = Item.buyPrice(silver: 28);
            NPC.aiStyle = 3; 
            NPC.knockBackResist = 0.2f;
            NPC.noTileCollide = false;
            NPC.scale = 1f;
            NPC.noGravity = false;
            AIType = NPCID.GoblinScout;
        }
        public override void AI()
        {
            NPC.spriteDirection = -NPC.direction;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 0f, 0f, 190, default, 0.8f);
            Lighting.AddLight(NPC.Center, 1.2f, 0.4f, 0.4f);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 8)
            {
                NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[NPC.type] * frameHeight);
                NPC.frameCounter = 0;
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.FireflyHit, 0f, 0f, 50, default, 1.2f);
            }
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"CinderClawGore1").Type, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"CinderClawGore2").Type, NPC.scale);

            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {
                return spawnInfo.Player.ZoneUnderworldHeight ? 0.07f : 0f;
            }
            else
            {
                return 0f;
            }
        }
        public override void SetBestiary
           (BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("")
           });
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
    }
}
