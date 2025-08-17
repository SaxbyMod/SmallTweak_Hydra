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
            return newDict.Count;
        }

        // Make a Handler for Health
        public static int GetHealthNum()
        {
            List<CardInfo> list = new List<CardInfo>(RunState.Run.playerDeck.Cards);
            List<int> health = new List<int>();
            
            // Fill the Health list
            list.Sort((CardInfo a, CardInfo b) => a.Health - b.Health);
            foreach (CardInfo currentCard in list)
            {
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
            list.Sort((CardInfo a, CardInfo b) => a.Attack - b.Attack);
            foreach (CardInfo currentCard in list)
            {
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
            if (GetHealthNum() <= 5 || GetPowerNum() <= 5)
            {
                __result = false;
                return false;
            }
            
            __result = GetNumberOfTribesInDeck() >= 5;
            return false;
        }
    }
}