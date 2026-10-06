Scriptname WBN_PowerEffectScript extends ActiveMagicEffect
{War Banners - "Plant War Banner" lesser power.}

WBN_BannerScript Property Banners Auto

Event OnEffectStart(Actor akTarget, Actor akCaster)
	If akCaster == Game.GetPlayer()
		Banners.UsePower()
	EndIf
EndEvent
