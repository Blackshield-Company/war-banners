Scriptname WBN_PlayerAliasScript extends ReferenceAlias
{War Banners - grants the "Plant War Banner" power the first time you get a standard.}

WBN_BannerScript Property Banners Auto
FormList Property WBN_StandardList Auto

Event OnInit()
	AddInventoryEventFilter(WBN_StandardList)
EndEvent

Event OnPlayerLoadGame()
	AddInventoryEventFilter(WBN_StandardList)
	Banners.GameLoaded()
EndEvent

Event OnItemAdded(Form akBaseItem, Int aiItemCount, ObjectReference akItemReference, ObjectReference akSourceContainer)
	Banners.StandardAcquired()
EndEvent
