using Modkit.Mutagen;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Noggog;
using static WarBanners.Generator.Vanilla;

namespace WarBanners.Generator;

internal sealed class Builder
{
    private readonly SkyrimMod _mod;
    private readonly FormIdRegistry _ids;
    private static readonly SkyrimRelease R = SkyrimRelease.SkyrimSE;
    private const byte VanillaQuestFormVersion = 65;
    public const int AliasPlayer = 0;
    public const int FollowerAliases = 5;

    public Builder(SkyrimMod mod, FormIdRegistry ids)
    {
        _mod = mod;
        _ids = ids;
    }

    private FormKey Id(string editorId) => _ids.Get(editorId);
    private static FormLink<T> L<T>(FormKey k) where T : class, IMajorRecordGetter => new(k);

    private sealed record StandardSpec(string Key, string Name, FormKey Cloth, FormKey Effect, float Magnitude,
        string Rally, string RallyName, (FormKey Item, int Count)[] Recipe);

    public void Build()
    {
        var qBanner = Id("WBN_BannerQuest");
        var qRally = Id("WBN_RallyQuest");

        // --- Settings ------------------------------------------------------------------------------
        var gGranted = Global("WBN_PowerGranted", 0);
        var gSeconds = Global("WBN_RallySeconds", 90);
        var gRadius = Global("WBN_RallyRadius", 1000);   // units; about 14 m
        var gPulse = Global("WBN_PulseSeconds", 4);
        var gCooldown = Global("WBN_CooldownHours", 4);
        var gPoleScale = Global("WBN_PoleScale", 0.65f);
        var gClothScale = Global("WBN_ClothScale", 1.0f);
        var gClothZ = Global("WBN_ClothOffsetZ", 262);   // units above the pole's base
        var gDistance = Global("WBN_PlantDistance", 150);

        // --- Rally effects (short, re-applied every pulse while in range) ---------------------------
        const string rallyShield = "Rally: Shield";
        const string rallyOath = "Rally: Oath";
        const string rallyClaw = "Rally: Claw";
        var effArmor = Effect("WBN_EffShieldwall", rallyShield, ActorValue.DamageResist,
            "Armor rating is increased by <mag> while you stand near the standard.");
        var effStamina = Effect("WBN_EffLongMarch", rallyOath, ActorValue.StaminaRateMult,
            "Stamina regenerates <mag>% faster while you stand near the standard.");
        var effValor = Effect("WBN_EffValor", rallyClaw, ActorValue.AttackDamageMult,
            "Weapon attacks do 10% more damage while you stand near the standard.");
        const int rallyDuration = 6; // pulse (4 s) + 2 s grace

        var specs = new[]
        {
            new StandardSpec("Shieldwall", "Banner of the Shield", CivilWarBannerImp01, effArmor, 60f, "WBN_RallyShieldwall", rallyShield,
                new[] { (Leather01, 3), (LeatherStrips, 2), (IngotIron, 2), (Firewood01, 1) }),
            new StandardSpec("LongMarch", "Banner of the Oath", CivilWarBannerStorm01, effStamina, 50f, "WBN_RallyLongMarch", rallyOath,
                new[] { (Leather01, 3), (LeatherStrips, 2), (IngotIron, 1), (Firewood01, 2) }),
            new StandardSpec("Claw", "Banner of the Claw", GenericBannerRed01, effValor, 0.1f, "WBN_RallyClaw", rallyClaw,
                new[] { (Leather01, 2), (LeatherStrips, 3), (IngotIron, 2), (Firewood01, 1) }),
        };

        var standards = new List<FormKey>();
        var poles = new List<FormKey>();
        var cloths = new List<FormKey>();
        var rallies = new List<FormKey>();
        foreach (var s in specs)
        {
            var item = Add(new MiscItem(Id($"WBN_Standard{s.Key}"), R)
            {
                EditorID = $"WBN_Standard{s.Key}",
                Name = s.Name,
                ObjectBounds = Bounds(-8, -47, -156, 8, 47, 0),
                Model = new Model { File = MeshBannerCloth },
                PickUpSound = L<ISoundDescriptorGetter>(ITMGenericUpSD).AsNullable(),
                PutDownSound = L<ISoundDescriptorGetter>(ITMGenericDownSD).AsNullable(),
                Keywords = new() { L<IKeywordGetter>(VendorItemClutter) },
                Value = 80,
                Weight = 4,
            });
            standards.Add(item.FormKey);

            var pole = Add(new Mutagen.Bethesda.Skyrim.Activator(Id($"WBN_Pole{s.Key}"), R)
            {
                EditorID = $"WBN_Pole{s.Key}",
                Name = s.Name,
                ObjectBounds = Bounds(-14, -81, 0, 14, 81, 419),
                Model = new Model { File = MeshBannerPole },
                ActivateTextOverride = "Take down",
                VirtualMachineAdapter = Vmad.Adapter(Vmad.Script("WBN_PlantedScript", Vmad.Obj("Banners", qBanner))),
            });
            poles.Add(pole.FormKey);
            cloths.Add(s.Cloth);

            var rally = Add(new Spell(Id(s.Rally), R)
            {
                EditorID = s.Rally,
                Name = s.RallyName,
                Type = SpellType.Spell,
                CastType = CastType.FireAndForget,
                TargetType = TargetType.Self,
                Flags = SpellDataFlag.ManualCostCalc | SpellDataFlag.NoAbsorbOrReflect,
                EquipmentType = L<IEquipTypeGetter>(EquipEitherHand).AsNullable(),
                Effects = { Fx(s.Effect, s.Magnitude, rallyDuration) },
            });
            rallies.Add(rally.FormKey);

            Recipe($"WBN_Recipe{s.Key}", item.FormKey, CraftingTanningRack, s.Recipe);
            Recipe($"WBN_Forge{s.Key}", item.FormKey, CraftingSmithingForge, s.Recipe);
        }

        var list = Add(new FormList(Id("WBN_StandardList"), R) { EditorID = "WBN_StandardList" });
        foreach (var s in standards) list.Items.Add(L<ISkyrimMajorRecordGetter>(s));

        // --- Messages --------------------------------------------------------------------------------
        var msgChoose = Msg("WBN_MsgChoose", Texts.Choose,
            specs.Select(s => Btn(s.Name)).Append(Btn("Cancel")).ToArray());
        var msgNone = Notice("WBN_MsgNoStandard", Texts.NoStandard);
        var msgCooldown = Notice("WBN_MsgCooldown", Texts.Cooldown);
        var msgMounted = Notice("WBN_MsgMounted", Texts.Mounted);
        var msgFirst = Msg("WBN_MsgFirstStandard", Texts.FirstStandard, Btn("Ok"));
        var msgPlanted = Notice("WBN_MsgPlanted", Texts.Planted);
        var msgFaded = Notice("WBN_MsgFaded", Texts.Faded);

        // --- Power -----------------------------------------------------------------------------------
        var mgefPower = Add(new MagicEffect(Id("WBN_PowerEffect"), R)
        {
            EditorID = "WBN_PowerEffect",
            Name = "Plant War Banner",
            Flags = MagicEffect.Flag.NoDuration | MagicEffect.Flag.NoMagnitude | MagicEffect.Flag.NoArea
                    | MagicEffect.Flag.Painless | MagicEffect.Flag.NoHitEvent | MagicEffect.Flag.HideInUI,
            MagicSkill = ActorValue.None, ResistValue = ActorValue.None, SecondActorValue = ActorValue.None,
            Archetype = new MagicEffectArchetype { Type = MagicEffectArchetype.TypeEnum.Script, ActorValue = ActorValue.None },
            CastType = CastType.FireAndForget,
            TargetType = TargetType.Self,
            CastingSoundLevel = SoundLevel.Normal,
            Description = "Plant a war standard you carry.",
            VirtualMachineAdapter = Vmad.Adapter(Vmad.Script("WBN_PowerEffectScript", Vmad.Obj("Banners", qBanner))),
        });
        var power = Add(new Spell(Id("WBN_PowerPlant"), R)
        {
            EditorID = "WBN_PowerPlant",
            Name = "Plant War Banner",
            Description = "Plant a war standard you carry. You and nearby followers rally around it for a short time. Activate the standard to take it down.",
            Type = SpellType.LesserPower,
            CastType = CastType.FireAndForget,
            TargetType = TargetType.Self,
            Flags = SpellDataFlag.ManualCostCalc | SpellDataFlag.NoAbsorbOrReflect,
            EquipmentType = L<IEquipTypeGetter>(EquipVoice).AsNullable(),
            Effects = { Fx(mgefPower.FormKey, 0, 0) },
        });

        // --- Rally quest: nearest followers in the loaded area -----------------------------------------
        var rallyQuest = Add(new Quest(qRally, R)
        {
            EditorID = "WBN_RallyQuest",
            Name = "War Banners: Rally",
            Priority = 30,
            Type = Quest.TypeEnum.None,
            QuestFormVersion = VanillaQuestFormVersion,
        });
        for (int i = 0; i < FollowerAliases; i++)
        {
            var a = new QuestAlias
            {
                ID = (uint)i, Type = QuestAlias.TypeEnum.Reference, Name = $"Follower{i + 1}",
                Flags = QuestAlias.Flag.Optional | QuestAlias.Flag.MatchingRefInLoadedArea | QuestAlias.Flag.MatchingRefClosest,
            };
            a.Conditions.Add(Cond.Make(new GetPlayerTeammateConditionData(), 1, CompareOperator.EqualTo));
            a.Conditions.Add(Cond.Make(new GetDeadConditionData(), 0, CompareOperator.EqualTo));
            rallyQuest.Aliases.Add(a);
        }
        rallyQuest.NextAliasID = FollowerAliases;

        // --- Controller ------------------------------------------------------------------------------
        var quest = Add(new Quest(qBanner, R)
        {
            EditorID = "WBN_BannerQuest",
            Name = "War Banners",
            Flags = Quest.Flag.StartGameEnabled | Quest.Flag.RunOnce,
            Priority = 30,
            Type = Quest.TypeEnum.None,
            QuestFormVersion = VanillaQuestFormVersion,
        });
        quest.Aliases.Add(new QuestAlias
        {
            ID = AliasPlayer, Type = QuestAlias.TypeEnum.Reference, Name = "Player",
            ForcedReference = new FormLinkNullable<IPlacedGetter>(PlayerRef),
        });
        quest.NextAliasID = 1;
        quest.VirtualMachineAdapter = Vmad.QuestAdapter(Vmad.Script("WBN_BannerScript",
                Vmad.Obj("WBN_PowerPlant", power.FormKey), Vmad.Obj("WBN_PowerGranted", gGranted),
                Vmad.Obj("WBN_RallySeconds", gSeconds), Vmad.Obj("WBN_RallyRadius", gRadius), Vmad.Obj("WBN_PulseSeconds", gPulse),
                Vmad.Obj("WBN_CooldownHours", gCooldown), Vmad.Obj("WBN_PoleScale", gPoleScale), Vmad.Obj("WBN_ClothScale", gClothScale),
                Vmad.Obj("WBN_ClothOffsetZ", gClothZ), Vmad.Obj("WBN_PlantDistance", gDistance),
                Vmad.ObjList("Standards", standards.ToArray()), Vmad.ObjList("Poles", poles.ToArray()),
                Vmad.ObjList("Cloths", cloths.ToArray()), Vmad.ObjList("Rallies", rallies.ToArray()),
                Vmad.Obj("WBN_StandardClaw", standards[2]), Vmad.Obj("WBN_PoleClaw", poles[2]),
                Vmad.Obj("WBN_ClothClaw", cloths[2]), Vmad.Obj("WBN_RallyClaw", rallies[2]),
                Vmad.Obj("WBN_RallyQuest", qRally),
                Vmad.AliasList("Followers", qRally, Enumerable.Range(0, FollowerAliases).ToArray()),
                Vmad.Obj("WBN_MsgChoose", msgChoose), Vmad.Obj("WBN_MsgNoStandard", msgNone), Vmad.Obj("WBN_MsgCooldown", msgCooldown),
                Vmad.Obj("WBN_MsgMounted", msgMounted), Vmad.Obj("WBN_MsgFirstStandard", msgFirst),
                Vmad.Obj("WBN_MsgPlanted", msgPlanted), Vmad.Obj("WBN_MsgFaded", msgFaded)))
            .WithAliasScripts(qBanner, AliasPlayer, Vmad.Script("WBN_PlayerAliasScript",
                Vmad.Obj("Banners", qBanner), Vmad.Obj("WBN_StandardList", list.FormKey)));
    }

    // ----------------------------------------------------------------------------------------
    private T Add<T>(T record) where T : SkyrimMajorRecord
    {
        _mod.GetTopLevelGroup<T>().Add(record);
        return record;
    }

    private FormKey Global(string edid, float value)
    {
        var g = new GlobalFloat(Id(edid), R) { EditorID = edid, Data = value };
        _mod.Globals.Add(g);
        return g.FormKey;
    }

    private FormKey Effect(string edid, string name, ActorValue av, string description)
    {
        var m = new MagicEffect(Id(edid), R)
        {
            EditorID = edid,
            Name = name,
            Flags = MagicEffect.Flag.Recover | MagicEffect.Flag.NoArea | MagicEffect.Flag.NoHitEvent,
            MagicSkill = ActorValue.None, ResistValue = ActorValue.None, SecondActorValue = ActorValue.None,
            Archetype = new MagicEffectArchetype { Type = MagicEffectArchetype.TypeEnum.ValueModifier, ActorValue = av },
            CastType = CastType.FireAndForget,
            TargetType = TargetType.Self,
            CastingSoundLevel = SoundLevel.Normal,
            Description = description,
        };
        _mod.MagicEffects.Add(m);
        return m.FormKey;
    }

    private void Recipe(string edid, FormKey created, FormKey bench, (FormKey Item, int Count)[] items)
    {
        _mod.ConstructibleObjects.Add(new ConstructibleObject(Id(edid), R)
        {
            EditorID = edid,
            CreatedObject = new FormLinkNullable<IConstructibleGetter>(created),
            WorkbenchKeyword = new FormLinkNullable<IKeywordGetter>(bench),
            CreatedObjectCount = 1,
            Items = items.Select(i => new ContainerEntry
            {
                Item = new ContainerItem { Item = L<IItemGetter>(i.Item), Count = i.Count },
            }).ToExtendedList(),
        });
    }

    private static Effect Fx(FormKey mgef, float magnitude, int duration) => new()
    {
        BaseEffect = new FormLinkNullable<IMagicEffectGetter>(mgef),
        Data = new EffectData { Magnitude = magnitude, Area = 0, Duration = duration },
    };

    private static MessageButton Btn(string text) => new() { Text = text };

    private FormKey Msg(string edid, string text, params MessageButton[] buttons)
    {
        var m = new Message(Id(edid), R) { EditorID = edid, Description = text, Flags = Message.Flag.MessageBox, INAM = new byte[4] };
        m.MenuButtons.AddRange(buttons);
        _mod.Messages.Add(m);
        return m.FormKey;
    }

    private FormKey Notice(string edid, string text)
    {
        var m = new Message(Id(edid), R) { EditorID = edid, Description = text, Flags = 0, DisplayTime = 2, INAM = new byte[4] };
        _mod.Messages.Add(m);
        return m.FormKey;
    }

    private static ObjectBounds Bounds(short x1, short y1, short z1, short x2, short y2, short z2)
        => new() { First = new P3Int16(x1, y1, z1), Second = new P3Int16(x2, y2, z2) };
}
