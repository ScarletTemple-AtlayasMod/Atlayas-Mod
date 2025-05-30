using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlayasMod.Content.Items.Weapons
{
    public class SelsaGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Selsa Gun");
            // Tooltip.SetDefault("A rapid-fire gun forged from Selsa bars.");
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40; // Adjust to your sprite
            Item.height = 20;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(silver: 90);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 8f;
            Item.useAmmo = AmmoID.Bullet;
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