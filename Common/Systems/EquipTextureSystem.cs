using Terraria.ID;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Common.Systems;
public class EquipTextureSystem : ModSystem
{
    public override void PostSetupContent()
    {
        foreach ((var name, var type) in MarvelTerrariaUniverse.TransformationTextures)
        {
            int slot = EquipLoader.GetEquipSlot(Mod, name[0], type);

            switch (type)
            {
                case EquipType.Head:
                    ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[slot] = false;
                    break;
                case EquipType.Body:
                    ArmorIDs.Body.Sets.HidesTopSkin[slot] = true;
                    ArmorIDs.Body.Sets.HidesArms[slot] = true;
                    break;
                case EquipType.Legs:
                    ArmorIDs.Legs.Sets.HidesBottomSkin[slot] = true;
                    break;
            }
        }
    }
}
