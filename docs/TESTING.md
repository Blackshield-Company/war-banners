# Testing checklist (v0.1.0, early beta)

Automated (every `./build.sh`):

- [x] Papyrus compiles.
- [x] Plugin validation: ESL range, Skyrim.esm only, links, script properties and base types (ActiveMagicEffect, ObjectReference for the pole, ReferenceAlias), alias array properties point at existing aliases of the rally quest, SEQ contents, byte-exact round trip.
- [x] FOMOD schema.
- [x] Cross-mod check (`tools/modkit/Modkit.Compat`) with every released mod of the series.

In game (not done yet; please help):

1. **Crafting and power**
   - [ ] Both standards are listed at a tanning rack with the right materials (`player.additem 0DB5D2 10`, `player.additem 0800E4 10`, `player.additem 05ACE4 5`, `player.additem 06F993 5`).
   - [ ] Making the first one shows the War Banners note and adds the lesser power "Plant War Banner".
   - [ ] Item names to look for: "Shieldwall Standard", "Long March Standard". `help "Standard" 4` lists their FormIDs.
2. **Planting**
   - [ ] The power plants the standard about two steps ahead, crossbar facing you. **Does the cloth hang from the crossbar?** If not, try `set WBN_ClothOffsetZ to <value>` and plant again; please report the value that looks right.
   - [ ] With both standards, a menu asks which one; Cancel plants nothing.
   - [ ] Without a standard: "You carry no war standard ..." notification.
   - [ ] On horseback: refused.
   - [ ] Within 4 game hours of planting: cooldown notification with the hours left.
3. **Rally**
   - [ ] Active effects show "Rally: Shieldwall" (+60 armor in the inventory's armor rating) or "Rally: Long March", and "Rally: Valor", while within about 14 m.
   - [ ] Walking away: the effects end a few seconds later. Walking back: they return.
   - [ ] A follower standing close gets the same effects (check the follower's armor rating in the trade menu, or the follower's active effects with a mod of your choice).
   - [ ] After 90 seconds: "The rally fades" notification and no more effects.
4. **Taking down**
   - [ ] Activating the pole ("Take down") removes pole and cloth and returns the standard.
   - [ ] Planting a second standard while one is planted: the first disappears and returns to your pack.
   - [ ] Save with a planted standard, reload: it's still there and can still be taken down.
