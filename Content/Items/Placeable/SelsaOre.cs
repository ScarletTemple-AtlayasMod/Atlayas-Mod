﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlayasMod.Content.Items.Placeable
{
	public class SelsaOre : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 100;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;

			// This ore can spawn in slime bodies like other pre-boss ores. (copper, tin, iron, etch)
			// It will drop in amount from 45 to 105
			ItemID.Sets.OreDropsFromSlime[Type] = (45, 105);
		}

		public override void SetDefaults() {
			//Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SelsaOre>());
			Item.width = 12;
			Item.height = 12;
			Item.value = 3000;
		}
	}
}