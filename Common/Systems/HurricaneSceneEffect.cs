namespace AtlayasMod.Common.Systems
{
    public class HurricaneSceneEffect : ModSceneEffect
    {
        public override int Music => HurricaneEvent.WrathOfTheWindMusicSlot;
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override bool IsSceneEffectActive(Player player)
        {
            // Only active if hurricane is running and player is in the ocean biome
            return HurricaneEvent.IsHurricaneActive && player.active && player.ZoneBeach;
        }
    }
}