using BepInEx;
using BepInEx.Logging;
using SmallTweak_Hydra.PATCHES;
using HarmonyLib;

namespace SmallTweak_Hydra
{
    public class SmallTweak_Hydra : BaseUnityPlugin
    {
        // --------------------------------------------------------------------------------------------------------------------------------------------------

        // These are variables that exist everywhere in the entire class.
        public const string PluginGuid = "creator.SmallTweak.Hydra";
        public const string PluginName = "Smalltweak Hydra";
        public const string PluginVersion = "1.0.0";
        public const string PluginPrefix = "Smalltweak_Hydra";

        // Define a Manual Log Source
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        // Declare Harmony here for future Harmony patches. You'll use Harmony to patch the game's code outside of the scope of the API.
        public static Harmony harmony = new(PluginGuid);

        public void Awake()
        {
            harmony.PatchAll(typeof(HydraPatches));
            Logger.LogMessage($"{PluginGuid}: Loaded Mod: {PluginName} - {PluginVersion}");
        }
    }
}