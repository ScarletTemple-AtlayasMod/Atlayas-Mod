using AtlayasMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlayasMod.Content.Items.Weapons
{

    public class Bassderbuss : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 33;
            Item.height = 33;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 9;
            Item.useAnimation = 27;
            Item.autoReuse = true;
            Item.reuseDelay = 41;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 9;
            Item.knockBack = 4.5f;

            Item.noMelee = true;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;

            Item.scale = 0.875f;

            Item.shoot = ModContent.ProjectileType<BassBlast>();
            Item.shootSpeed = 10.25f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const int NumProjectiles = 3; // The number of projectiles that this gun will shoot.
            SoundEngine.PlaySound(SoundID.DSTMaleHurt, player.position);
            for (int i = 0; i < NumProjectiles; i++)
            {
                // Rotate the velocity randomly by 30 degrees at max.
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(7.25f));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.01f);

                // Create a projectile.
                Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }

            return false; // Return false because we don't want tModLoader to shoot projectile
        }



        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Here we add a tooltipline that will later be removed, showcasing how to remove tooltips from an item
            var line = new TooltipLine(Mod, "Face", "Shoots out blasts of multiple soundwaves");
            tooltips.Add(line);

            line = new TooltipLine(Mod, "Face", "Does not require ammo")
            {
                OverrideColor = new Color(255, 255, 255)
            };
            tooltips.Add(line);



            // Here we will hide all tooltips whose title end with ':RemoveMe'
            // One like that is added at the start of this method
            foreach (var l in tooltips)
            {
                if (l.Name.EndsWith(":RemoveMe"))
                {
                    l.Hide();
                }
            }

            // Another method of hiding can be done if you want to hide just one line.
            // tooltips.FirstOrDefault(x => x.Mod == "ExampleMod" && x.Name == "Verbose:RemoveMe")?.Hide();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-25f, 0f);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IllegalGunParts);
            recipe.AddRecipeGroup("IronBar", 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}
