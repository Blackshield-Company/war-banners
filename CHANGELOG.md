# Changelog

All notable changes to this project are documented here. Versions follow [Semantic Versioning](https://semver.org/).

## [0.1.0] - 2026-10-06

Early beta. First public release. It passes every automated check in the build (Papyrus compile, plugin validation, round trip, SEQ check, FOMOD schema, cross-mod compatibility check) but has not been playtested in-game yet. In particular, how the banner cloth sits on its pole has not been seen in game (see Known issues in the README). Please report problems on the Nexus page or on GitHub.

### Added
- Two war standards, made at a tanning rack:
  - Shieldwall Standard (3 Leather, 2 Leather Strips, 2 Iron Ingots, 1 Firewood): +60 armor.
  - Long March Standard (3 Leather, 2 Leather Strips, 1 Iron Ingot, 2 Firewood): stamina regenerates 50% faster.
  - Both: weapon attacks do 10% more damage.
- Lesser power "Plant War Banner", granted the first time you get a standard. It plants the standard in front of you: a Civil War camp banner pole with the Imperial (Shieldwall) or Stormcloak (Long March) cloth. For 90 seconds, you and up to five followers within about 14 m get the standard's effect, refreshed every 4 seconds, so it fades a few seconds after you step away.
- Activate a planted standard to take it down. Planting another standard takes the first one down and returns it to your pack. A standard can be planted once every 4 game hours.
- Settings as console globals.
- `SEQ/WarBanners.seq` for the Start Game Enabled controller quest.
- FOMOD installer with optional Papyrus sources.
