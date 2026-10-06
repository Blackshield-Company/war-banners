Scriptname WBN_BannerScript extends Quest
{War Banners - plants a carried standard (lesser power "Plant War Banner"), rallies
the player and nearby followers for a short time, and takes the standard down again.
The planted banner is a pole (this mod's activator, vanilla mesh) with a vanilla
banner cloth hung from it, both placed by script. No world edits.}

Spell Property WBN_PowerPlant Auto
GlobalVariable Property WBN_PowerGranted Auto
GlobalVariable Property WBN_RallySeconds Auto
GlobalVariable Property WBN_RallyRadius Auto
GlobalVariable Property WBN_PulseSeconds Auto
GlobalVariable Property WBN_CooldownHours Auto
GlobalVariable Property WBN_PoleScale Auto
GlobalVariable Property WBN_ClothScale Auto
GlobalVariable Property WBN_ClothOffsetZ Auto
GlobalVariable Property WBN_PlantDistance Auto

MiscObject[] Property Standards Auto
{Inventory items, one per standard type.}
Activator[] Property Poles Auto
{Planted pole per type (activate to take the standard down).}
Form[] Property Cloths Auto
{Vanilla banner cloth (moveable static) hung from the pole, per type.}
Spell[] Property Rallies Auto
{Short buff per type, re-applied every pulse to everyone in range.}

Quest Property WBN_RallyQuest Auto
ReferenceAlias[] Property Followers Auto
{WBN_RallyQuest's follower aliases (nearest player teammates in the loaded area).}

Message Property WBN_MsgChoose Auto
Message Property WBN_MsgNoStandard Auto
Message Property WBN_MsgCooldown Auto
Message Property WBN_MsgMounted Auto
Message Property WBN_MsgFirstStandard Auto
Message Property WBN_MsgPlanted Auto
Message Property WBN_MsgFaded Auto

ObjectReference pole
ObjectReference cloth
Int planted = -1
Bool rallying
Float rallyEndsAt
Float cooldownUntil

; ---------------------------------------------------------------------------
; Power
; ---------------------------------------------------------------------------

Function StandardAcquired()
	If WBN_PowerGranted.GetValueInt() == 0
		WBN_PowerGranted.SetValueInt(1)
		Game.GetPlayer().AddSpell(WBN_PowerPlant)
		WBN_MsgFirstStandard.Show()
		Debug.Trace("WBN_ power granted")
	EndIf
EndFunction

Function GameLoaded()
	Actor player = Game.GetPlayer()
	If !player.HasSpell(WBN_PowerPlant) && CarriedCount() > 0
		WBN_PowerGranted.SetValueInt(1)
		player.AddSpell(WBN_PowerPlant)
	EndIf
	If rallying
		RegisterForSingleUpdate(1.0)
	EndIf
	Debug.Trace("WBN_ loaded: planted=" + planted + " rallying=" + rallying)
EndFunction

Function UsePower()
	Actor player = Game.GetPlayer()
	If player.IsOnMount()
		WBN_MsgMounted.Show()
		Return
	EndIf
	Float now = Utility.GetCurrentGameTime()
	If now < cooldownUntil
		WBN_MsgCooldown.Show((cooldownUntil - now) * 24.0)
		Return
	EndIf
	Int kind = ChooseStandard()
	If kind >= 0
		Plant(kind)
	EndIf
EndFunction

Int Function CarriedCount()
	Actor player = Game.GetPlayer()
	Int n = 0
	Int i = 0
	While i < Standards.Length
		If player.GetItemCount(Standards[i]) > 0
			n += 1
		EndIf
		i += 1
	EndWhile
	Return n
EndFunction

Int Function ChooseStandard()
	Actor player = Game.GetPlayer()
	Int carried = CarriedCount()
	If carried == 0
		WBN_MsgNoStandard.Show()
		Return -1
	EndIf
	If carried == 1
		Int i = 0
		While i < Standards.Length
			If player.GetItemCount(Standards[i]) > 0
				Return i
			EndIf
			i += 1
		EndWhile
	EndIf
	Int choice = WBN_MsgChoose.Show()
	If choice >= 0 && choice < Standards.Length && player.GetItemCount(Standards[choice]) > 0
		Return choice
	EndIf
	Return -1
EndFunction

; ---------------------------------------------------------------------------
; Planting and taking down
; ---------------------------------------------------------------------------

Function Plant(Int aiKind)
	Actor player = Game.GetPlayer()
	If pole != None
		TakeDown(True) ; one standard at a time: the old one comes back to your pack
	EndIf
	player.RemoveItem(Standards[aiKind], 1, True)

	Float angle = player.GetAngleZ()
	Float dist = WBN_PlantDistance.GetValue()
	pole = player.PlaceAtMe(Poles[aiKind], 1, False, True)
	pole.MoveTo(player, dist * Math.Sin(angle), dist * Math.Cos(angle), 0.0, False)
	pole.SetAngle(0.0, 0.0, angle + 90.0) ; crossbar across your line of sight
	pole.SetScale(WBN_PoleScale.GetValue())
	pole.Enable()
	cloth = pole.PlaceAtMe(Cloths[aiKind], 1, False, True)
	cloth.MoveTo(pole, 0.0, 0.0, WBN_ClothOffsetZ.GetValue(), False)
	cloth.SetAngle(0.0, 0.0, angle + 90.0)
	cloth.SetScale(WBN_ClothScale.GetValue())
	cloth.Enable()

	planted = aiKind
	cooldownUntil = Utility.GetCurrentGameTime() + WBN_CooldownHours.GetValue() / 24.0
	rallyEndsAt = Utility.GetCurrentRealTime() + WBN_RallySeconds.GetValue()
	rallying = True
	WBN_RallyQuest.Stop()
	WBN_RallyQuest.Start()
	WBN_MsgPlanted.Show(WBN_RallySeconds.GetValue())
	Debug.Trace("WBN_ planted kind=" + aiKind)
	Pulse()
	RegisterForSingleUpdate(PulseSeconds())
EndFunction

; Called by WBN_PlantedScript when the player activates a planted pole.
Function PoleActivated(ObjectReference akPole)
	If akPole == pole
		TakeDown(False)
	Else
		; a pole this quest no longer tracks (should not happen): just clear it
		akPole.Disable()
		akPole.Delete()
	EndIf
EndFunction

Function TakeDown(Bool abSilent)
	If cloth != None
		cloth.Disable()
		cloth.Delete()
		cloth = None
	EndIf
	If pole != None
		pole.Disable()
		pole.Delete()
		pole = None
	EndIf
	If planted >= 0
		Game.GetPlayer().AddItem(Standards[planted], 1, abSilent)
		Debug.Trace("WBN_ taken down kind=" + planted)
	EndIf
	planted = -1
	StopRally(False)
EndFunction

; ---------------------------------------------------------------------------
; Rally
; ---------------------------------------------------------------------------

Event OnUpdate()
	If !rallying || planted < 0 || pole == None
		Return
	EndIf
	If Utility.GetCurrentRealTime() >= rallyEndsAt
		StopRally(True)
		Return
	EndIf
	Pulse()
	RegisterForSingleUpdate(PulseSeconds())
EndEvent

Float Function PulseSeconds()
	Float s = WBN_PulseSeconds.GetValue()
	If s < 1.0
		s = 1.0
	EndIf
	Return s
EndFunction

Function Pulse()
	Spell rally = Rallies[planted]
	Float radius = WBN_RallyRadius.GetValue()
	Int n = 0
	If Rally(Game.GetPlayer(), rally, radius)
		n += 1
	EndIf
	Int i = 0
	While i < Followers.Length
		If Rally(Followers[i].GetActorReference(), rally, radius)
			n += 1
		EndIf
		i += 1
	EndWhile
EndFunction

Bool Function Rally(Actor akActor, Spell akRally, Float afRadius)
	If akActor == None || akActor.IsDead() || !akActor.Is3DLoaded()
		Return False
	EndIf
	If akActor.GetDistance(pole) > afRadius
		Return False
	EndIf
	akRally.Cast(akActor, akActor)
	Return True
EndFunction

Function StopRally(Bool abNotify)
	If rallying && abNotify
		WBN_MsgFaded.Show()
	EndIf
	rallying = False
	UnregisterForUpdate()
	WBN_RallyQuest.Stop()
EndFunction
