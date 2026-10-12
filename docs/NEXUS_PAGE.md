# Nexus Mods page: War Banners

Early beta: v0.2.0 passed every automated build check but hasn't been playtested in-game. The page says so up front. Exact upload-form values are in [NEXUS_FIELDS.md](NEXUS_FIELDS.md).

## Page settings

- **Game:** Skyrim Special Edition
- **Mod name:** War Banners
- **Version:** 0.2.0 (early beta)
- **Category:** Gameplay
- **Suggested tags:** Gameplay, Combat, Crafting, Followers, Buffs, Lore-Friendly, ESL, No SKSE, Scripted
- **Requirements:** none.
- **Permissions (suggested):** source on GitHub under MIT, so allow modification and conversions with credit. Don't allow re-uploads of the archive itself.
- **File to upload:** `WarBanners-0.2.0.zip` (FOMOD), main file, named "War Banners".

## Description (paste into the BBCode editor)

```bbcode
[center][size=6][b]WAR BANNERS[/b][/size]
[size=3][i]Plant the colours. Hold the line.[/i][/size]

[size=3][color=#e0a030][b]EARLY BETA (v0.2.0)[/b]: this version passes every automated build check but hasn't been playtested in-game yet. Please report bugs in the Bugs tab or on GitHub, ideally with your Papyrus log.[/color][/size][/center]

[line]

[size=4][b]Summary[/b][/size]
Make a war banner at a tanning rack or a forge, plant it where the fighting is, and rally around it. You and your followers close to the banner fight harder for a short time. Then take it down and carry it to the next fight.

[size=4][b]The banners[/b][/size]
Each banner has one buff. Make them at a tanning rack or a forge.
[list]
[*][b]Banner of the Shield[/b] (Imperial colours): [b]+60 armor[/b]. 3 Leather, 2 Leather Strips, 2 Iron Ingots, 1 Firewood.
[*][b]Banner of the Oath[/b] (Stormcloak colours): [b]stamina regenerates 50% faster[/b]. 3 Leather, 2 Leather Strips, 1 Iron Ingot, 2 Firewood.
[*][b]Banner of the Claw[/b] (red cloth): weapon attacks do [b]10% more damage[/b]. 2 Leather, 3 Leather Strips, 2 Iron Ingots, 1 Firewood.
[/list]

[size=4][b]How it works[/b][/size]
[list]
[*]Your first standard teaches you the lesser power [b]Plant War Banner[/b].
[*]Use it and the standard goes up a couple of steps in front of you: a Civil War camp banner pole with its cloth.
[*]For [b]90 seconds[/b], you and up to [b]five followers[/b] within about 14 m rally. Step away and the effect fades a few seconds later.
[*][b]Activate[/b] the standard to take it down. One standard planted at a time, once every 4 game hours.
[/list]
Followers are whoever is with you: vanilla followers, sellswords, the Blackshield Company's hireling. The mod doesn't change them.

[size=4][b]Requirements[/b][/size]
[list]
[*]Skyrim Special Edition or Anniversary Edition (1.5.97 or 1.6.x). Works on Steam under Proton on Linux.
[*]Nothing else. No SKSE, no SkyUI, no DLC, no Creation Club content.
[/list]

[size=4][b]Installation[/b][/size]
Install with [b]Mod Organizer 2[/b] or [b]Vortex[/b] (FOMOD installer) and enable [b]WarBanners.esp[/b]. It's ESL-flagged, so it doesn't take a full load-order slot. Safe to add mid-playthrough.

[size=4][b]Compatibility[/b][/size]
[list]
[*][b]No edits to vanilla records and nothing placed in the world.[/b] Skyrim.esm is the only master.
[*]Works with any follower mod: followers are found as your teammates, nothing about them is changed.
[*]Compatible with the rest of my mods (Blackshield Company, Bounties of Oblivion, Contract Hunter, Steel Oath, Ember Smithing, The Gray-Mane Debt, Blackshield Codex), checked by loading all plugins together.
[/list]

[size=4][b]Settings (console)[/b][/size]
[code]set WBN_RallySeconds to 90 / set WBN_RallyRadius to 1000 / set WBN_CooldownHours to 4
set WBN_ClothOffsetZ to 262   (height of the cloth on the pole)[/code]

[size=4][b]Uninstalling[/b][/size]
Take your standards down first. As with any scripted mod, prefer not to remove it mid-playthrough.

[size=4][b]Known issues / early beta limits[/b][/size]
[list]
[*][b]The cloth's height on the pole hasn't been seen in game yet.[/b] It was worked out from the meshes' sizes. If the cloth floats or sinks into the crossbar, change [code]WBN_ClothOffsetZ[/code] and plant again, and please tell me the value that looks right.
[*]On slopes the pole can float a little or dip into the ground.
[*]Banner meshes have no physics: a standard dropped from your inventory hangs in the air. Just pick it up.
[*]Planting is through the power only (inventory items can't be "used" without SKSE).
[/list]

[size=4][b]Source[/b][/size]
Open source (MIT): [url=https://github.com/synthalorian/war-banners]github.com/synthalorian/war-banners[/url]

[size=4][b]Credits[/b][/size]
[list]
[*][url=https://github.com/Mutagen-Modding/Mutagen]Mutagen[/url] by Noggog and contributors, which generates the plugin from code.
[*][url=https://github.com/russo-2025/papyrus-compiler]papyrus-compiler[/url] by russo-2025, the open-source Papyrus compiler.
[*][url=https://github.com/GandaG/fomod-schema]FOMOD schema[/url] by GandaG.
[*]Bethesda Game Studios for Skyrim.
[/list]
```
