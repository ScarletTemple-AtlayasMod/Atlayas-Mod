using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace AtlayasMod.Content.Biome
// My attempt at an animated main menu background,doesn't work so i disabled it for now.
{
    /*public class AnimatedMainMenuBackgroundSystem : ModSystem
    {
        private Asset<Texture2D> _mainMenuTexture;
        private const int FrameCount = 6;
        private const int FrameSpeed = 9; // Ticks per frame (60 FPS / 9 ≈ 0.15s per frame)
        private int _frameCounter;
        private int _currentFrame;

        public override void Load()
        {
            _mainMenuTexture = ModContent.Request<Texture2D>("AtlayasMod/Assets/Textures/Menu/MainMenu");
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            // Only draw on the main menu
            if (!Main.gameMenu)
                return;

            int layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Main Menu", System.StringComparison.Ordinal));
            if (layerIndex != -1)
            {
                layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
                    "AtlayasMod: Animated Main Menu Background",
                    DrawAnimatedBackground,
                    InterfaceScaleType.UI)
                );
            }
        }

        private bool DrawAnimatedBackground()
        {
            if (_mainMenuTexture?.Value == null)
                return true;

            // Animate frames
            _frameCounter++;
            if (_frameCounter > FrameSpeed)
            {
                _currentFrame = (_currentFrame + 1) % FrameCount;
                _frameCounter = 0;
            }

            Texture2D texture = _mainMenuTexture.Value;
            int frameWidth = texture.Width / FrameCount;
            Rectangle sourceRect = new Rectangle(_currentFrame * frameWidth, 0, frameWidth, texture.Height);

            Main.spriteBatch.Draw(
                texture,
                new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
                sourceRect,
                Color.White
            );

            return true; // Continue drawing other layers
        }
    }*/
}