using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using Terraria.ID;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.UI;

namespace AtlayaasMod.Common.Systems
{
    public class SpringWind : ModSystem
    {
        private bool eventActive = false;
        private int eventTimer;

        private int leafTimer;

        private int eventDuration;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                //Preloads the tree sway shader (not yet applied, crashes tmodloader)

                //Asset<Effect> TreeSwayShader = ModContent.Request<Effect>("AtlayasMod/Assets/Effects/TreeSway");

            }
        }

        public override void Unload()
        {
            /*
            particleTexture = null;
            particles.Clear();
            */
        }
        public override void PostUpdateWorld()
        {
            if (!eventActive)
            {
                //replaced the stupid reduntant if statement that is true if its day, or it isnt day
                //any you might be thinking, wait? it checks wether its day or it isnt day? isnt that always???
                //yes, it is always, Main.isdaytimeorwtvr || !Main.isdaytimeorwtvr is always true  bro
                if (Main.rand.NextFloat() <= 0.40f)
                {
                    eventActive = true;
                    eventDuration = (int)(Main.dayTime ? Main.dayLength - Main.time : Main.nightLength - Main.time);
                    eventTimer = eventDuration;
                    Main.windSpeedCurrent = 1.2f;
                    Main.windSpeedTarget = 1.4f;

                    if (!Main.dedServ)
                    {
                        // Play heavy wind sound effect
                        SoundEngine.PlaySound(new SoundStyle("AtlayasMod/Assets/Sfx/windsfx"), Main.LocalPlayer.position);
                    }
                }
            }

            else
            {
                eventTimer--;
                leafTimer--;

                if (leafTimer <= 0)
                {
                    SpawnLeaves();
                    leafTimer = 60;
                }

                if (eventTimer <= 0)
                {
                    eventActive = false;
                    Main.windSpeedTarget = 0.3f;
                }
            }
        }
        private void SpawnLeaves()
        {
            int leafAmount = Main.rand.Next(2, 5);
            for (int i = 0; i < leafAmount; i++)
            {
                Dust.NewDust(new Vector2(Main.rand.Next(0, Main.maxTilesX) * 16, Main.rand.Next(0, Main.maxTilesY) * 16),
                     10, 10, DustID.GrassBlades, Main.windSpeedCurrent * 10, -Main.rand.Next(1, 3));
            }
        }
        public bool IsActive()
        {
            return eventActive;
        }

    }
    public class SpringWindMusic : ModSceneEffect
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/WhispersOfSpring");

        public override SceneEffectPriority Priority => SceneEffectPriority.Event;

        public override bool IsSceneEffectActive(Player player)
        {
            return ModContent.GetInstance<SpringWind>().IsActive();
        }
    }
}