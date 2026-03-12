using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.Chat;
using Terraria.Localization;

namespace AtlayaasMod.Common.Systems
{
    public class SpringWind : ModSystem
    {
        public static bool eventActive = false;

        private int eventTimer;
        private int leafTimer;
        private int eventDuration;

        public override void PostUpdateWorld()
        {
            if (!eventActive)
            {
               
                if (Main.rand.NextBool(36000))
                {
                    StartEvent();
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
                    EndEvent();
                }
            }
        }

        private void StartEvent()
        {
            eventActive = true;

            eventDuration = (int)(Main.dayTime ? Main.dayLength - Main.time : Main.nightLength - Main.time);
            eventTimer = eventDuration;

            Main.windSpeedCurrent = 1.2f;
            Main.windSpeedTarget = 1.4f;

            if (!Main.dedServ)
            {
                SoundEngine.PlaySound(new SoundStyle("AtlayasMod/Assets/Sfx/windsfx"), Main.LocalPlayer.position);
            }

            ChatHelper.BroadcastChatMessage(
                NetworkText.FromLiteral("spring wind starts"),
                new Color(150, 255, 150)
            );
        }

        private void EndEvent()
        {
            eventActive = false;

            Main.windSpeedTarget = 0.3f;

            ChatHelper.BroadcastChatMessage(
                NetworkText.FromLiteral("spring wind over"),
                new Color(180, 220, 180)
            );
        }

        private void SpawnLeaves()
        {
            int leafAmount = Main.rand.Next(2, 5);

            for (int i = 0; i < leafAmount; i++)
            {
                Dust.NewDust(
                    new Vector2(Main.rand.Next(0, Main.maxTilesX) * 16, Main.rand.Next(0, Main.maxTilesY) * 16),
                    10,
                    10,
                    DustID.GrassBlades,
                    Main.windSpeedCurrent * 10,
                    -Main.rand.Next(1, 3)
                );
            }
        }

        public static bool IsActive()
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
            return SpringWind.IsActive();
        }
    }
}