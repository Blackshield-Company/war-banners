# War Banners

Craft a war banner, plant it in the field, and fight beside it. A small Skyrim Special Edition mod: three banners made at a tanning rack or a forge, a lesser power to plant them, and a short rally for you and your followers while you stand close.

**Status: v0.2.0, early beta.** It passes every automated check in the build (Papyrus compile, plugin validation, byte-exact round trip, SEQ check, FOMOD schema, cross-mod compatibility check) but has not been playtested in-game yet. Expect rough edges and please report what you find. See [docs/TESTING.md](docs/TESTING.md).

- Game: Skyrim Special Edition / Anniversary Edition 1.6.x (also fine on 1.5.97). Works on Windows and on Linux through Proton.
- Requirements: none beyond the base game. No SKSE, no SkyUI, no DLC, no Creation Club content.
- Plugin: `WarBanners.esp`, ESL-flagged, Skyrim.esm is the only master. **No vanilla records edited and nothing placed in the world**: banners exist only where you plant them.
- Compatible with the rest of the series ([Blackshield Company](https://github.com/synthalorian/blackshield-company), [Bounties of Oblivion](https://github.com/synthalorian/bounties-of-oblivion), [Contract Hunter](https://github.com/synthalorian/contract-hunter), [Steel Oath](https://github.com/synthalorian/steel-oath), [Ember Smithing](https://github.com/synthalorian/ember-smithing), [The Gray-Mane Debt](https://github.com/synthalorian/the-gray-mane-debt), [Blackshield Codex](https://github.com/synthalorian/blackshield-codex)): own prefix (`WBN_`), records, globals and files. Checked by loading all plugins together (see [tools/modkit](tools/modkit/README.md)).

## How it works

| Banner | Made at a tanning rack or a forge from | Rally effect |
|---|---|---|
| Banner of the Shield (Imperial cloth) | 3 Leather, 2 Leather Strips, 2 Iron Ingots, 1 Firewood | +60 armor |
| Banner of the Oath (Stormcloak cloth) | 3 Leather, 2 Leather Strips, 1 Iron Ingot, 2 Firewood | stamina regenerates 50% faster |
| Banner of the Claw (red cloth) | 2 Leather, 3 Leather Strips, 2 Iron Ingots, 1 Firewood | weapons do 10% more damage |

1. Get a standard. The first time, you learn the lesser power **Plant War Banner** and a short note explains it.
2. Use the power. The banner is planted a couple of steps in front of you (if you carry more than one, you pick one).
3. For **90 seconds**, you and up to **five followers** within about **14 m** of the standard rally: the effect is refreshed every 4 seconds and runs out a few seconds after you step away.
4. **Activate** the standard to take it down and put it back in your pack. Planting another standard takes the first one down for you.
5. You can plant a standard once every **4 game hours**.

Followers are any of your current teammates in the loaded area (vanilla followers, the Blackshield Company's sellsword, hired sellswords from other mods). The mod doesn't change them, it only applies the short effect.

### Settings (console)

```
set WBN_RallySeconds to 90      ; how long a planted standard rallies (real seconds)
set WBN_RallyRadius to 1000     ; reach in units (about 14 m)
set WBN_CooldownHours to 4      ; game hours between plantings
set WBN_PulseSeconds to 4       ; how often the effect is refreshed
set WBN_PlantDistance to 150    ; how far in front of you the standard goes
set WBN_PoleScale to 0.65       ; size of the pole
set WBN_ClothOffsetZ to 262     ; height of the cloth's top above the ground (see Known issues)
set WBN_ClothScale to 1.0
```

Changes apply to the next standard you plant.

## Known issues (early beta)

- **The cloth's height on the pole is computed, not seen.** The pole and the cloth are separate vanilla meshes that the vanilla camps place together. Their sizes were taken from the meshes' bounds (pole 5.98 m tall with a 2.3 m crossbar, cloth 2.23 m), and the pole is planted at 0.65 scale with the cloth's top at 262 units, just under the crossbar. Banner of the Claw uses a different vanilla cloth (`GenericBannerRed01`) at the same height. If a cloth floats above the crossbar or sinks into it in your game, adjust `WBN_ClothOffsetZ` (higher number = higher cloth) and plant again, and please tell me the value that looks right.
- On a slope the pole stands at the height of your feet, so it can float a little or sink into the ground.
- The standard in your inventory uses the banner cloth mesh. Banner meshes have no physics, so a standard dropped from your inventory hangs in the air where you dropped it. Pick it up again with activate.
- Planted standards stay where you leave them until you take them down, even if you walk away.

## Building

`./build.sh` on Linux builds the release zip from this repo: Papyrus is compiled with the open-source [papyrus-compiler](https://github.com/russo-2025/papyrus-compiler), the plugin is generated from C# with [Mutagen](https://github.com/Mutagen-Modding/Mutagen), and everything is validated. Needs .NET SDK 9+, python3, curl and optionally xmllint. See [tools/modkit/README.md](tools/modkit/README.md).

## Layout

```
src/plugin/WarBanners.Generator/   C# generator (items, poles, spells, power, quests, recipes)
src/plugin/formids.json            pinned FormIDs
src/papyrus/                       controller, power, player alias and planted-pole scripts
fomod/                             FOMOD installer config
tools/modkit/                      shared build tooling (Mutagen helpers, validator, Papyrus, FOMOD, compat check)
docs/                              design notes, testing checklist, Nexus page
```

## Credits

- Built with [Mutagen](https://github.com/Mutagen-Modding/Mutagen) and [russo-2025/papyrus-compiler](https://github.com/russo-2025/papyrus-compiler).
- The banner meshes belong to Bethesda Softworks; this mod references them from Skyrim.esm and ships no Bethesda assets. The mesh dimensions were read from a public scene kit's bounds data (jtattersall09403/elder-souls-argonia).

## Support

Donations are optional. Nothing in the mod is gated on them.

- GitHub Sponsors: https://github.com/sponsors/synthalorian
- Ko-fi: https://ko-fi.com/synthalorian

## License

MIT (code and text in this repository). See [LICENSE](LICENSE).
