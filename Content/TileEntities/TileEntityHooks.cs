using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.TileEntities;
internal class TileEntityHooks : ILoadable
{
    public void Load(Mod mod)
    {
        On_Main.DrawNPCs += DrawTEs;
    }

    void ILoadable.Unload() { }

    private void DrawTEs(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
    {
        if (behindTiles)
        {
            orig(self, behindTiles);
            return;
        }

        foreach (var item in TileEntity.ByID)
        {
            if (item.Value is DrawableTileEntity te && te.CanDraw()) te.Draw(Main.spriteBatch);
        }

        orig(self, behindTiles);
    }
}