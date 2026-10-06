using Mutagen.Bethesda.Plugins;

namespace WarBanners.Generator;

/// <summary>
/// Skyrim.esm records this mod links to (FormIDs looked up with Mutagen.Bethesda.FormKeys.SkyrimSE).
/// Mesh paths: the Civil War camp banner pole and cloth (clutter\civilwar\civilwarbanner01.nif and
/// civilwarbannerimp01.nif) as used by the vanilla records CivilWarBanner01 and CivilWarBannerImp01.
/// </summary>
internal static class Vanilla
{
    public static readonly ModKey Skyrim = ModKey.FromFileName("Skyrim.esm");
    public static FormKey K(uint id) => new(Skyrim, id);

    public static readonly FormKey PlayerRef = K(0x000014);
    public static readonly FormKey EquipVoice = K(0x025BEE);
    public static readonly FormKey EquipEitherHand = K(0x013F44);
    public static readonly FormKey VendorItemClutter = K(0x0914E9);
    public static readonly FormKey ITMGenericUpSD = K(0x03C7BA);
    public static readonly FormKey ITMGenericDownSD = K(0x03C7B9);

    // Crafting
    public static readonly FormKey CraftingTanningRack = K(0x07866A);
    public static readonly FormKey Leather01 = K(0x0DB5D2);
    public static readonly FormKey LeatherStrips = K(0x0800E4);
    public static readonly FormKey IngotIron = K(0x05ACE4);
    public static readonly FormKey Firewood01 = K(0x06F993);

    // Banner cloths placed on the planted pole (vanilla moveable statics, placed by script)
    public static readonly FormKey CivilWarBannerImp01 = K(0x097ED9);
    public static readonly FormKey CivilWarBannerStorm01 = K(0x02BEBB);

    public const string MeshBannerPole = @"Clutter\CivilWar\CivilWarBanner01.nif";
    public const string MeshBannerCloth = @"Clutter\CivilWar\CivilWarBannerImp01.nif";
}
