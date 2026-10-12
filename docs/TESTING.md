# Testing checklist (v0.2.0, early beta)

Automated (every `./build.sh`):

- [x] Papyrus compiles.
- [x] Plugin validation: ESL range, Skyrim.esm only, links, script properties and base types (ActiveMagicEffect, ObjectReference for the pole, ReferenceAlias), alias array properties point at existing aliases of the rally quest, SEQ contents, byte-exact round trip.
- [x] FOMOD schema.
- [x] Cross-mod check (`tools/modkit/Modkit.Compat`) with every released mod of the series.

In game (not done yet; please help):

1. **Crafting and power**
    - [ ] All three banners are listed at a tanning rack and at a forge, with the same materials at both (`player.additem 0DB5D2 10`, `player.additem 0800E4 10`, `player.additem 05ACE4 5`, `player.additem 06F993 5`). Shield: 3 Leather, 2 Leather Strips, 2 Iron Ingots, 1 Firewood. Oath: 3 Leather, 2 Leather Strips, 1 Iron Ingot, 2 Firewood. Claw: 2 Leather, 3 Leather Strips, 2 Iron Ingots, 1 Firewood.
    - [ ] Making the first one shows the War Banners note and adds the lesser power "Plant War Banner".
    - [ ] Item names to look for: "Banner of the Shield", "Banner of the Oath", "Banner of the Claw". `help "Banner" 4` lists their FormIDs.
2. **Planting**
   - [ ] The power plants the standard about two steps ahead, crossbar facing you. **Does the cloth hang from the crossbar?** If not, try `set WBN_ClothOffsetZ to <value>` and plant again; please report the value that looks right.
    - [ ] With more than one banner, a menu lists Banner of the Shield, Banner of the Oath, Banner of the Claw, and Cancel; Cancel plants nothing.
    - [ ] Without a banner: "You carry no war standard ..." notification, including "Make one at a tanning rack or a forge."
   - [ ] On horseback: refused.
   - [ ] Within 4 game hours of planting: cooldown notification with the hours left.
3. **Rally**
    - [ ] Active effects show only one buff: "Rally: Shield" (+60 armor in the inventory's armor rating), "Rally: Oath" (stamina), or "Rally: Claw" (weapons), while within about 14 m.
   - [ ] Walking away: the effects end a few seconds later. Walking back: they return.
   - [ ] A follower standing close gets the same effects (check the follower's armor rating in the trade menu, or the follower's active effects with a mod of your choice).
   - [ ] After 90 seconds: "The rally fades" notification and no more effects.
4. **Taking down**
   - [ ] Activating the pole ("Take down") removes pole and cloth and returns the standard.
   - [ ] Planting a second standard while one is planted: the first disappears and returns to your pack.
    - [ ] Save with a planted standard, reload: it's still there and can still be taken down.
    - [ ] A save from 0.1.0 that already carries or has planted a standard still works after updating, and Banner of the Claw can be made and planted.
