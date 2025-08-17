using DiskCardGame;
using UnityEngine;
using HarmonyLib;

namespace SmallTweak_Hydra.PATCHES
{
    [HarmonyPatch]
    internal static class HydraPortraitPatches
    {
        [HarmonyPrefix, HarmonyPatch(typeof(HydraEggPortrait), nameof(HydraEggPortrait.ApplyCardInfo))]
        public static void ApplyCardInfo(CardInfo card, HydraEggPortrait __instance)
        {
            // Handle Health Lights
            __instance.healthLightRenderers.ForEach(delegate(SpriteRenderer x) { x.enabled = false; });
            for (int j = 0; j < HydraPatches.GetHealthNum(); j++)
            {
                __instance.healthLightRenderers[j].enabled = true;
            }
            
            // Handle Power Lights
            __instance.powerLightRenderers.ForEach(delegate(SpriteRenderer x) { x.enabled = false; });
            for (int j = 0; j < HydraPatches.GetPowerNum(); j++)
            {
                __instance.powerLightRenderers[j].enabled = true;
            }

            // Handle Tribe Lights
            __instance.tribeLightRenderers.ForEach(delegate(SpriteRenderer x) { x.enabled = false; });
            for (int j = 0; j < HydraPatches.GetNumberOfTribesInDeck(); j++)
            {
                __instance.tribeLightRenderers[j].enabled = true;
            }
        }
    }
}