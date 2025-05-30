using AtlayasMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlayasMod.Common.Systems
{
    public class HurricaneEvent : ModSystem
    {
        public static bool IsHurricaneActive => ModContent.GetInstance<HurricaneEvent>().hurricaneActive;
        public static int WrathOfTheWindMusicSlot { get; private set; }
        private bool hurricaneActive = false;
        private int hurricaneDuration = 0;
        private int thunderTimer = 0;
        private const int HurricaneLength = 60 * 60 * 2; // You can adjust the length of the hurricane here (2 hours in-game time) here

        #region Custom Particle System (commented out for now)
        /*
        // Custom Particle system
        private struct HurricaneParticle
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public float Alpha;
            public float Scale;
            public float Life;
        }
        private static List<HurricaneParticle> particles = new();
        private static Texture2D particleTexture;
        */
        #endregion

        public override void Load()
        {
            if (!Main.dedServ)
            {
                // Register music
                WrathOfTheWindMusicSlot = MusicLoader.GetMusicSlot(Mod, "AtlayasMod/Assets/Music/WrathOfTheWind");
                /*
                // Load particle texture (fallback to MagicPixel if not found)
                if (ModContent.HasAsset("AtlayasMod/Assets/Images/HurricaneParticle"))
                    particleTexture = ModContent.Request<Texture2D>("AtlayasMod/Assets/Images/HurricaneParticle").Value;
                else
                    particleTexture = TextureAssets.MagicPixel.Value;
                */
            }
        }

        public override void Unload()
        {
            /*
            particleTexture = null;
            particles.Clear();
            */
        }

        public override void OnWorldLoad()
        {
            hurricaneActive = false;
            hurricaneDuration = 0;
            thunderTimer = 0;
            /*
            particles.Clear();
            */
        }

        public override void PreUpdateWorld()
        {
            // 30% chance each day at dawn (4:30 AM in Terraria)
            if (Main.time == 0 && !hurricaneActive && Main.dayTime) //adjust start time of the event here
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && player.ZoneBeach && Main.rand.NextFloat() < 0.9f)
                    {
                        StartHurricane();
                        break;
                    }
                }
            }

            if (hurricaneActive)
            {
                hurricaneDuration--;
                if (hurricaneDuration <= 0)
                {
                    EndHurricane();
                }
            }
        }

        private void StartHurricane()
        {
            hurricaneActive = true;
            hurricaneDuration = HurricaneLength;
            Main.NewText("A hurricane is raging at the ocean!", Color.LightGray);

            // Play hurricane SFX
            SoundStyle hurricaneSound = new SoundStyle("AtlayasMod/Assets/Sfx/HurricaneSfx")
            {
                Volume = 1.0f,
                Pitch = 0.0f,
                PitchVariance = 0.1f,
                IsLooped = true
            };
            SoundEngine.PlaySound(hurricaneSound, Main.LocalPlayer.position);

            thunderTimer = Main.rand.Next(30, 90);
        }

        private void EndHurricane()
        {
            hurricaneActive = false;
            // Reset wind to calm when hurricane ends
            Main.windSpeedTarget = 0f;
            Main.windSpeedCurrent = 0f;
            // Turn off rain and clouds
            Main.raining = false;
            Main.rainTime = 0;
            Main.maxRaining = 0f;
            Main.cloudBGActive = 0f;
        }

        public override void PostUpdateWorld()
        {
            if (!hurricaneActive)
                return;

            Player player = Main.LocalPlayer;

            // If hurricane is active but player is not in the ocean biome, turn off rain for this player
            if (hurricaneActive && (!player.active || !player.ZoneBeach))
            {
                Main.raining = false;
                Main.rainTime = 0;
                Main.maxRaining = 0f;
                Main.cloudBGActive = 0f;
                return;
            }

            // --- Dramatic wind for tree sway during hurricane --- (Touch the number more dramatic effect,or gut it completely)
            // Randomly set wind direction and strength for a natural, stormy effect
            if (Main.rand.NextBool(120)) // Change direction every ~2 seconds
            {
                Main.windSpeedTarget = Main.rand.NextFloat(1.2f, 1.7f) * (Main.rand.NextBool() ? 1 : -1);
            }
            // Smoothly interpolate current wind towards target
            Main.windSpeedCurrent = MathHelper.Lerp(Main.windSpeedCurrent, Main.windSpeedTarget, 0.05f);

            // --- Rain background and weather effect ---
            Main.raining = true;
            Main.rainTime = Math.Max(Main.rainTime, 60);
            Main.cloudBGActive = 1f;
            Main.maxRaining = 1f;

            // Thunder spawn logic
            thunderTimer--;
            if (thunderTimer <= 0)
            {
                SpawnThunder(player);
                thunderTimer = Main.rand.Next(30, 90);
            }

            // --- VANILLA BLIZZARD PARTICLE EFFECT ---
            // i use vanilla blizzard dust as placeholder for hurricane wind. For a custom effect, replace with your own dust type. if you want to further customize it,
            // uncomment the old particle system code and modify it to your liking.
            for (int i = 0; i < 6; i++) // Adjust count for density
            {
                Vector2 spawnPos = new Vector2(
                    Main.screenPosition.X + Main.rand.Next(Main.screenWidth),
                    Main.screenPosition.Y + Main.rand.Next(Main.screenHeight)
                );
                Dust d = Dust.NewDustDirect(spawnPos, 1, 1, DustID.Ice,
                    Main.windSpeedCurrent * 12f + Main.rand.NextFloat(2f, 6f),
                    Main.rand.NextFloat(-1f, 1f),
                    150,
                    default,
                    Main.rand.NextFloat(0.8f, 1.2f));
                d.noGravity = true;
            }

            #region Custom Particle System (commented out for now)
            /*
            // Particle logic
            // Spawn new particles
            if (Main.rand.NextBool(2))
            {
                HurricaneParticle p = new HurricaneParticle
                {
                    Position = new Vector2(Main.rand.Next(0, Main.screenWidth), Main.rand.Next(0, Main.screenHeight)),
                    Velocity = new Vector2(Main.windSpeedCurrent * 16f + Main.rand.NextFloat(6f, 12f), Main.rand.NextFloat(-1f, 1f)),
                    Alpha = Main.rand.NextFloat(0.3f, 0.7f),
                    Scale = Main.rand.NextFloat(0.7f, 1.2f),
                    Life = Main.rand.NextFloat(20f, 40f)
                };
                particles.Add(p);
            }

            // Update particles
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                HurricaneParticle p = particles[i];
                p.Position += p.Velocity;
                p.Life--;
                if (p.Life <= 0 || p.Position.X < 0 || p.Position.X > Main.screenWidth || p.Position.Y < 0 || p.Position.Y > Main.screenHeight)
                {
                    particles.RemoveAt(i);
                }
                else
                {
                    particles[i] = p;
                }
            }
            */
            #endregion
        }

        private void SpawnThunder(Player player) // Spawns a lightning strike at a random position above the screen (this should be self-explanatory)
        {
            float x = Main.screenPosition.X + Main.rand.Next(Main.screenWidth);
            float y = Main.screenPosition.Y - 120; // 120 pixels above the top of the screen
            Vector2 strikePos = new Vector2(x, y);
            Projectile.NewProjectile(
                new EntitySource_Misc("HurricaneEvent"),
                strikePos,
                Vector2.Zero,// No initial velocity, i already set the velocity in the projectile itself
                ModContent.ProjectileType<LightningProjectile>(),
                60,
                6f,
                Main.myPlayer
            );
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor) // This method darken the background and tile colors during the hurricane event
        {
            if (hurricaneActive && Main.LocalPlayer.active && Main.LocalPlayer.ZoneBeach)
            {
                // Darken the background and tile colors a little bit
                Color darkened = Color.Lerp(backgroundColor, new Color(40, 40, 40), 0.45f);
                backgroundColor = darkened;
                tileColor = darkened;
            }
        }

        public override void ModifyScreenPosition() // this method adds a slight screen shake effect during the hurricane event
        {
            if (hurricaneActive && Main.LocalPlayer.active && Main.LocalPlayer.ZoneBeach)
            {
                Main.screenPosition += new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            }
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)// Still here for future use, but currently unused
        {
            #region Custom Particle System (commented out for now)
            /*
            if (hurricaneActive && Main.LocalPlayer.active && Main.LocalPlayer.ZoneBeach && particleTexture != null)
            {
                foreach (var p in particles)
                {
                    Color color = Color.White * p.Alpha;
                    spriteBatch.Draw(
                        particleTexture,
                        p.Position,
                        null,
                        color,
                        0f,
                        new Vector2(particleTexture.Width / 2f, particleTexture.Height / 2f),
                        p.Scale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }
            */
            #endregion
        }
    }
}