using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System.Linq;
using System.Collections.Generic;

namespace SmallTweak_Hydra.PATCHES
{
    // Patch adapted from GrimoraMod style: replace HydraEgg rulebook description.
    [HarmonyPatch(typeof(RuleBookInfo))]
    internal static class HydraRulebookPatches
    {
        [HarmonyAfter(InscryptionAPI.InscryptionAPIPlugin.ModGUID)]
        [HarmonyPostfix, HarmonyPatch(nameof(RuleBookInfo.ConstructPageData), typeof(AbilityMetaCategory))]
        public static void PostfixUpdateHydraEggRulebook(AbilityMetaCategory metaCategory)
        {
            var hydraInfo = AbilityManager.AllAbilityInfos.FirstOrDefault(i => i.ability == Ability.HydraEgg);
            if (hydraInfo == null)
                return;
            hydraInfo.rulebookDescription =
                "A card bearing this Sigil hatches when drawn if 5 unique numbers are in the Health of creatures in your deck, and in their Power, and if there is a creature of 5 unique tribes in your deck. [This Excludes all cards bearing this sigil]";
        }
    }
}