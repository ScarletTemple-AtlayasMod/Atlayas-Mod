using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlayasMod.Content.Items.Weapons
{
    public class SelsaSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Selsa Sword");
            // Tooltip.SetDefault("A sword forged from Selsa bars.");
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40; // Adjust to your sprite
            Item.height = 40;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Placeable.SelsaBar>(), 7)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}