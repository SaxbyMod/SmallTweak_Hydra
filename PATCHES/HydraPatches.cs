using DiskCardGame;
using HarmonyLib;
using System.Collections.Generic;

namespace SmallTweak_Hydra.PATCHES
{
    [HarmonyPatch]
    internal static class HydraPatches
    {
        // Make a new Tribe Handler
        public static int GetNumberOfTribesInDeck()
        {
            List<CardInfo> list = new List<CardInfo>(RunState.Run.playerDeck.Cards);
            list.Sort((a, b) => a.tribes.Count - b.tribes.Count);
            HashSet<Tribe> uniqueTribes = new HashSet<Tribe>();

            foreach (CardInfo card in list)
            {
                if (card.HasAbility(Ability.HydraEgg))
                {
                    continue;
                }
                foreach (Tribe tribe in card.tribes)
                {
                    if (tribe != Tribe.None)
                    {
                        uniqueTribes.Add(tribe);
                    }
                }
            }

            return uniqueTribes.Count;
        }

        // Make a Handler for Health
        public static int GetHealthNum()
        {
            List<CardInfo> list = new List<CardInfo>(RunState.Run.playerDeck.Cards);
            List<int> health = new List<int>();
            
            // Fill the Health list
            list.Sort((a, b) => a.Health - b.Health);
            foreach (CardInfo currentCard in list)
            {
                if (currentCard.HasAbility(Ability.HydraEgg))
                {
                    continue;
                }
                if (!health.Contains(currentCard.Health))
                {
                    health.Add(currentCard.Health);
                }
            }
            
            return health.Count;
        }
        
        // Make a Handler for Power
        public static int GetPowerNum()
        {
            List<CardInfo> list = new List<CardInfo>(RunState.Run.playerDeck.Cards);
            List<int> power = new List<int>();
            
            // Fill the Power List
            list.Sort((a, b) => a.Attack - b.Attack);
            foreach (CardInfo currentCard in list)
            {
                if (currentCard.HasAbility(Ability.HydraEgg))
                {
                    continue;
                }
                if (!power.Contains(currentCard.Attack))
                {
                    power.Add(currentCard.Attack);
                }
            }

            return power.Count;
        }
        
        [HarmonyPrefix, HarmonyPatch(typeof(HydraEgg), nameof(HydraEgg.RespondsToDrawn))]
        public static bool PrefixRespondsToDrawn(ref bool __result)
        {
            // Health/Power Handler
            if (GetHealthNum() < 5 || GetPowerNum() < 5)
            {
                __result = false;
                return false;
            }
            
            __result = GetNumberOfTribesInDeck() >= 5;
            return false;
        }
    }
}