# Design notes (v0.2, early beta)

## Records (39, FormIDs 0x800-0x826, ESL-flagged, Skyrim.esm only, no overrides)

| EditorID | Type | Purpose |
|---|---|---|
| `WBN_BannerQuest` | QUST (Start Game Enabled, Run Once) | Controller (`WBN_BannerScript`). Alias 0 = player (`WBN_PlayerAliasScript`: inventory filter on `WBN_StandardList`, grants the power on the first standard, re-checks on load). In `SEQ/WarBanners.seq`. |
| `WBN_RallyQuest` | QUST | Started when a standard is planted, stopped when the rally ends. Five optional *Find Matching Reference* aliases (in loaded area, closest; conditions `GetPlayerTeammate == 1`, `GetDead == 0`). No packages, factions or flags: the aliases only let the controller find followers without SKSE. |
| `WBN_StandardShieldwall`, `WBN_StandardLongMarch`, `WBN_StandardClaw` | MISC | Display names Banner of the Shield, Banner of the Oath, Banner of the Claw (vanilla banner cloth mesh `Clutter\CivilWar\CivilWarBannerImp01.nif`), weight 4, value 80. |
| `WBN_StandardList` | FLST | All three banners (inventory event filter). |
| `WBN_PoleShieldwall`, `WBN_PoleLongMarch`, `WBN_PoleClaw` | ACTI | The planted pole (vanilla mesh `Clutter\CivilWar\CivilWarBanner01.nif`), activation text "Take down", script `WBN_PlantedScript`. |
| `WBN_RallyShieldwall`, `WBN_RallyLongMarch`, `WBN_RallyClaw` | SPEL (fire and forget, self, 6 s) | One effect each: `WBN_EffShieldwall` 60, `WBN_EffLongMarch` 50, or `WBN_EffValor` 0.1. Names Rally: Shield, Rally: Oath, Rally: Claw. |
| `WBN_EffShieldwall`, `WBN_EffLongMarch`, `WBN_EffValor` | MGEF (value modifier, recover) | Names Rally: Shield, Rally: Oath, Rally: Claw. DamageResist, StaminaRateMult, AttackDamageMult. Valor's description keeps a literal "10%" because magnitude 0.1 would display as 0.1. |
| `WBN_PowerPlant`, `WBN_PowerEffect` | SPEL (lesser power) + MGEF (script) | "Plant War Banner". |
| `WBN_RecipeShieldwall`, `WBN_RecipeLongMarch`, `WBN_RecipeClaw` | COBJ (tanning rack) | Recipes. |
| `WBN_ForgeShieldwall`, `WBN_ForgeLongMarch`, `WBN_ForgeClaw` | COBJ (forge) | Same ingredients as the matching tanning-rack recipe. |
| `WBN_Msg*` | MESG | Menu, first-standard note, notifications. |
| `WBN_*` settings | GLOB | See the README. `WBN_PowerGranted` remembers that the power was given. |

The banner cloths are the vanilla moveable statics `CivilWarBannerImp01` (097ED9), `CivilWarBannerStorm01` (02BEBB), and `GenericBannerRed01` (0D2009), placed by script.

### Existing saves

Papyrus Auto properties are baked into the save. A 0.1.0 save still has two-entry `Standards`, `Poles`, `Cloths`, and `Rallies` arrays. The Claw records are also filled as `WBN_StandardClaw`, `WBN_PoleClaw`, `WBN_ClothClaw`, and `WBN_RallyClaw` (new properties load from the plugin). `UpgradeArrays()`, called at the start of `GameLoaded()` and `UsePower()`, rebuilds those arrays to length 3 when `Standards.Length < 3`, copying indexes 0 and 1 and setting index 2 from the new properties. Existing FormIDs are unchanged, so a carried or planted standard keeps working. The choice menu's buttons are in array order (Banner of the Shield, Banner of the Oath, Banner of the Claw, Cancel), so the button index is the banner index.

## Planting

`PlaceAtMe` places the pole initially disabled, `MoveTo` puts it `WBN_PlantDistance` units in front of the player (offsets from `Math.Sin/Cos` of the player's heading), `SetAngle` turns the crossbar across the player's line of sight, `SetScale(WBN_PoleScale)`, then `Enable`. The cloth is placed the same way on the pole's position, `WBN_ClothOffsetZ` units up, same heading, `SetScale(WBN_ClothScale)`. Both references are held in script variables, so they persist until taken down (`Disable` + `Delete`).

### Why the cloth offset is a setting

Vanilla camps place the pole (`CivilWarBanner01`) and the cloth (`CivilWarBannerImp01`) as two references. Without the Creation Kit there is no way here to look at a vanilla camp or render the meshes, so the pairing was worked out from the meshes' bounds (published in a public scene kit):

| Mesh | Size (m) | Origin |
|---|---|---|
| civilwarbanner01 (pole) | 0.39 x 2.30 x 5.98 | at the base, centered on the 2.3 m crossbar |
| civilwarbannerimp01 (cloth) | 0.21 x 1.32 x 2.23 | at the top, centered across its width |

The same kit's notes say the vanilla pairs work with the pole at 0.65 scale and the cloth at 1.0. That gives a 3.9 m pole with a 1.5 m crossbar, wide enough for the 1.32 m cloth. The cloth's top is put at 262 units (3.74 m at 70 units per meter), just under the top of the scaled pole. This is a best estimate. It's a global so it can be corrected in game without a new version.

## Rally

While the rally runs, `OnUpdate` fires every `WBN_PulseSeconds` (4) real seconds. The player and each filled follower alias get the rally spell cast on themselves (`Spell.Cast(actor, actor)`) if they're alive, loaded, and within `WBN_RallyRadius` of the pole. The spell lasts 6 seconds, so the effect stays steady while you're close and runs out shortly after you leave. After `WBN_RallySeconds` the rally stops and the rally quest is stopped. The standard stays planted until you take it down.

Cooldown: game time, `WBN_CooldownHours` after planting. Taking a standard down doesn't reset it.

## Scaled down from the concept (documented on purpose)

- **Planted only through the power.** Misc items can't be "used" from the inventory without SKSE, so the lesser power does the planting and picks the banner (a menu if you carry more than one).
- **Three banners, one effect each.** Armor, stamina regeneration, and +10% weapon damage are each a banner's only buff.
- **No banner physics in the inventory.** The banner cloth mesh is the standard's model, as asked ("reuse vanilla banner meshes"), but it has no collision, so a dropped standard floats.
- **Followers found per planting.** The follower aliases are filled when the standard is planted; someone who joins you mid-rally is included the next time.
