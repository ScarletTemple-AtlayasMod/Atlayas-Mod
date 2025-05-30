using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlayasMod
{
    public class AtlayasModMenu : ModMenu
    {
        private const string menuAssetPath = "AtlayasMod/Assets/Textures/Menu"; // Creates a constant variable representing the texture path, so we don't have to write it out multiple times

        private Asset<Texture2D> LogoTexture;
        

        public override void Load()
        {
            LogoTexture = ModContent.Request<Texture2D>($"{menuAssetPath}/MainLogo");
        }

        public override Asset<Texture2D> Logo => LogoTexture;

        

        public override string DisplayName => "AtlayasMod";

        public override void OnSelected()
        {
            SoundEngine.PlaySound(SoundID.Thunder); // Plays a thunder sound when this ModMenu is selected
        }
        // Uncomment this if you want to change the logo color to disco color
        /*public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            drawColor = Main.DiscoColor;
            return true;
        }*/

        // music
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/Atlayas");
    }
}