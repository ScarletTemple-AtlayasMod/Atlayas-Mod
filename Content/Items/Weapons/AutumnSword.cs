using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlayasMod.Content.Items.Weapons
{
    public class AutumnSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Autumn Sword");
            // Tooltip.SetDefault("A sword made from autumn's bounty.");
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40; // Adjust to your sprite
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 10)
                .AddIngredient(ItemID.Hay, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}