using DiskCardGame;
using HarmonyLib;
using System.Collections.Generic;

namespace SmallTweak_Hydra.PATCHES
{
    [HarmonyPatch]
    internal static class HydraPatches
    {
        // Patch for Tribe Num
        [HarmonyPrefix, HarmonyPatch(typeof(HydraEgg), nameof(HydraEgg.GetNumTribesInDeck))]
        public static int PrefixGetNumTribesInDeck(CardInfo card, ref bool __runOriginal)
        {
            List<CardInfo> list = new List<CardInfo>(RunState.Run.playerDeck.Cards);
            list.Sort((CardInfo a, CardInfo b) => a.tribes.Count - b.tribes.Count);
            
            // Create a dict of tribes including modded {HOPEFULLY}
            List<Tribe> dict = new List<Tribe>();
            for (int count = 1; count <= (int)Tribe.NUM_TRIBES; count++)
            {
                Tribe tribe = (Tribe)count;
                if (tribe == Tribe.NUM_TRIBES || tribe == Tribe.None)
                {
                    continue;
                }
                dict.Add(tribe);
            }
            
            // Make a Handler for determining if tribe is Unique.
            List<Tribe> newDict = new List<Tribe>();
            for (int count = 0; count <= 5; count++)
            {
                foreach (CardInfo cardInfo in list)
                {
                    int i = cardInfo.tribes.Count;
                    if (cardInfo.IsOfTribe(dict[i]) && !newDict.Contains(dict[i]))
                    {
                        newDict.Add(dict[i]);
                        count++;
                    }
                }
            }
            
            // Return our proper count.
            __runOriginal = false;
            return newDict.Count;
        }
        
        // Patch for Drawn Response
        [HarmonyPrefix, HarmonyPatch(typeof(HydraEgg), nameof(HydraEgg.RespondsToDrawn))]
        public static bool PrefixRespondsToDrawn(CardInfo card, ref bool __runOriginal)
        {
            List<CardInfo> list = new List<CardInfo>(RunState.Run.playerDeck.Cards);
            List<int> health = new List<int>();
            List<int> power = new List<int>();
            
            // Fill the Health list
            list.Sort((CardInfo a, CardInfo b) => a.Health - b.Health);
            foreach (CardInfo currentCard in list)
            {
                if (!health.Contains(currentCard.Health))
                {
                    health.Add(currentCard.Health);
                }
            }
            
            // Fill the Power List
            list.Sort((CardInfo a, CardInfo b) => a.Attack - b.Attack);
            foreach (CardInfo currentCard in list)
            {
                if (!power.Contains(currentCard.Attack))
                {
                    power.Add(currentCard.Attack);
                }
            }
            
            // Health/Power Handler
            if (health.Count <= 5 || power.Count <= 5)
            {
                return false;
            }
            
            __runOriginal = false;
            return HydraEgg.GetNumTribesInDeck() >= 5;
        }
    }
}