using MarvelTerrariaUniverse.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Common.CustomLoadout;
public abstract class BaseCustomLoadoutAccessorySlot : ModAccessorySlot
{
    public string SlotBackgroundTexture = $"{nameof(MarvelTerrariaUniverse)}/Assets/UI/InventoryBack_IronMan";

    public override bool DrawFunctionalSlot => Player.GetModPlayer<LoadoutPlayer>().UsingCustomLoadout;
    public override bool DrawVanitySlot => false;
    public override bool DrawDyeSlot => false;

    public override string FunctionalBackgroundTexture => SlotBackgroundTexture;

    public override bool IsHidden()
    {
        return FunctionalItem.IsAir;
    }
}

public class CustomLoadoutAccessorySlot1 : BaseCustomLoadoutAccessorySlot { }
public class CustomLoadoutAccessorySlot2 : BaseCustomLoadoutAccessorySlot { }
public class CustomLoadoutAccessorySlot3 : BaseCustomLoadoutAccessorySlot { }
public class CustomLoadoutAccessorySlot4 : BaseCustomLoadoutAccessorySlot { }
public class CustomLoadoutAccessorySlot5 : BaseCustomLoadoutAccessorySlot { }
public class CustomLoadoutAccessorySlot6 : BaseCustomLoadoutAccessorySlot { }
public class CustomLoadoutAccessorySlot7 : BaseCustomLoadoutAccessorySlot { }
public class CustomLoadoutAccessorySlot8 : BaseCustomLoadoutAccessorySlot { }