namespace WarBanners.Generator;

internal static class Texts
{
    public const string Choose = "Which standard do you plant?";
    public const string NoStandard =
        "You carry no war standard. Banner of the Shield: +60 armor. Banner of the Oath: stamina regenerates 50% faster. " +
        "Banner of the Claw: weapons do 10% more damage. Make one at a tanning rack or a forge.";
    public const string Cooldown = "Your arm is still tired from the last standard. Ready again in %.0f hours.";
    public const string Mounted = "You can't plant a standard from the saddle.";
    public const string Planted = "The standard is planted. You and your companions nearby rally for %.0f seconds.";
    public const string Faded = "The rally fades. Activate the standard to take it down.";
    public const string FirstStandard =
        "War Banners\n\nYou carry a war standard. Use the lesser power Plant War Banner to plant it in front of you: " +
        "for a short time you and any followers close to it rally around it.\n\n" +
        "Banner of the Shield: +60 armor. Banner of the Oath: stamina regenerates 50% faster. " +
        "Banner of the Claw: weapons do 10% more damage. Each banner has one buff.\n\n" +
        "Make one at a tanning rack or a forge. Activate a planted standard to take it down. A standard can be planted once every few hours.";
}
