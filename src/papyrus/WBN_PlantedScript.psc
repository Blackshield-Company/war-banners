Scriptname WBN_PlantedScript extends ObjectReference
{War Banners - a planted standard's pole. Activate it to take the standard down.}

WBN_BannerScript Property Banners Auto

Event OnActivate(ObjectReference akActionRef)
	If akActionRef == Game.GetPlayer()
		Banners.PoleActivated(Self)
	EndIf
EndEvent
