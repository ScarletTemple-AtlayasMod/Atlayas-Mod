using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace AtlayasMod.Content.Projectiles
{
    public class LightningProjectile : ModProjectile
    {
        // This stores the points that make up the lightning's jagged path (i know this can be simplified, but i don't know how to do it yet)
        private List<Vector2> points = new();
        // This ensures the path is only generated once per projectile (Note : this is not a networked variable, so it won't sync across clients.)
        // please remember to remind me to deal with this later. right now,it works fine for singleplayer 
        private bool initialized = false;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lightning Bolt");
        }

        public override void SetDefaults()
        {
            // Basic projectile properties
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false; // set this to true make this look weird,so i set it to false and handle collisions manually
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // This only generate the lightning path once, on spawn (This is may not the best way to do it, but it works for now)
            if (!initialized)
            {
                GenerateLightningPath();
                initialized = true;
            }
        }

        /// <summary>
        /// Generates a jagged, possibly splitting lightning path starting from the projectile's center.
        /// The path will stop if it hits a solid tile.
        /// </summary>
        private void GenerateLightningPath()
        {
            points.Clear();
            Vector2 start = Projectile.Center;
            points.Add(start);

            int segments = 16; // Number of segments in the lightning bolt
            float segmentLength = 24f; // Length of each segment
            // Randomize the main direction a bit (not perfectly vertical)
            float angle = MathHelper.ToRadians(Main.rand.NextFloat(-15f, 15f));
            Vector2 direction = Vector2.UnitY.RotatedBy(angle);

            // Randomly decide if this bolt will split
            bool willSplit = Main.rand.NextBool(3); // 1 in 3 chance to split
            int splitSegment = willSplit ? Main.rand.Next(6, segments - 4) : -1;

            for (int i = 1; i <= segments; i++)
            {
                // Add random horizontal/vertical offset for jaggedness
                float xOffset = Main.rand.NextFloat(-16f, 16f);
                float yOffset = Main.rand.NextFloat(-2f, 2f);
                Vector2 next = start + direction * (i * segmentLength) + new Vector2(xOffset, yOffset);

                // Collision check: stop if This projectile hit a solid, non-platform tile
                Point tileCoords = next.ToTileCoordinates();
                Tile tile = Framing.GetTileSafely(tileCoords.X, tileCoords.Y);
                if (tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                {
                    points.Add(next);
                    break;
                }

                points.Add(next);

                // This section handle splitting: spawn a new bolt at a random segment, veering off at a random angle
                if (willSplit && i == splitSegment)
                {
                    float splitAngle = MathHelper.ToRadians(Main.rand.NextFloat(-40f, 40f));
                    Vector2 splitDir = direction.RotatedBy(splitAngle);
                    if (Projectile.owner == Main.myPlayer)
                    {
                        int splitProj = Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            next,
                            Vector2.Zero,
                            Projectile.type,
                            Projectile.damage / 2,
                            Projectile.knockBack,
                            Projectile.owner
                        );
                        // The new projectile will generate its own path, starting from the split point
                        if (splitProj >= 0 && splitProj < Main.maxProjectiles)
                        {
                            var proj = Main.projectile[splitProj].ModProjectile as LightningProjectile;
                            if (proj != null)
                            {
                                proj.GenerateLightningPathFrom(next, splitDir, segments - i);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Helper for split bolts to continue from a given point and direction.
        /// Also stops at solid tiles.
        /// </summary>
        public void GenerateLightningPathFrom(Vector2 start, Vector2 direction, int remainingSegments)
        {
            points.Clear(); // This clear previous points to avoid overlap
            points.Add(start); // This add the starting point
            float segmentLength = 24f;
            for (int i = 1; i <= remainingSegments; i++)
            {
                float xOffset = Main.rand.NextFloat(-16f, 16f);
                float yOffset = Main.rand.NextFloat(-2f, 2f);
                Vector2 next = start + direction * (i * segmentLength) + new Vector2(xOffset, yOffset);

                // Collision check: stop if we hit a solid, non-platform tile
                Point tileCoords = next.ToTileCoordinates();
                Tile tile = Framing.GetTileSafely(tileCoords.X, tileCoords.Y);
                if (tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                {
                    points.Add(next);
                    break;
                }

                points.Add(next);
            }
        }

        /// <summary>
        /// Draws the lightning bolt as a series of stretched dots between the generated points.
        /// </summary>
        public override bool PreDraw(ref Color lightColor)
        {
            if (points.Count < 2)
                return true; // it would look weird if there is no points to draw

            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value; // Get the projectile texture ( This may not be the best way to do it )
            for (int i = 1; i < points.Count; i++)
            {
                Vector2 start = points[i - 1] - Main.screenPosition;
                Vector2 end = points[i] - Main.screenPosition;
                float rotation = (end - start).ToRotation();
                float length = Vector2.Distance(start, end);

                // Draw a stretched dot between points to form the lightning segment
                Main.spriteBatch.Draw(
                    tex,
                    start,
                    null,
                    Color.White,
                    rotation,
                    new Vector2(0, tex.Height / 2f),
                    new Vector2(length, 2f) / tex.Width,
                    SpriteEffects.None,
                    0f
                );
            }
            return false; // We handled drawing
        }
    }
}