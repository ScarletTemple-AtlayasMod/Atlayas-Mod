using AtlayasMod.Content.Items.Placeable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AtlayasMod.Common.ModPlayers
{
    // This is so ??? Like why we need a dame music box item to start with? But oh well,as you wish.
    public class MusicBoxStartingItem : ModPlayer
    {
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            
                return new List<Item>
               {
                   new(ModContent.ItemType<AtlayasTitleBox>()),
                   new(ModContent.ItemType<SirenHeadBox>())
               };
            
        }

    }
}
