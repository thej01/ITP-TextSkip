using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ITP_TextSkip.Patches;

namespace ITP_TextSkip
{
    public class Logger
    {
        internal ManualLogSource MLS;

        private string modGUID = null;
        private string modName = null;
        private string modVersion = null;

        public void Init(string _modName, string _modVersion, string _modGUID)
        {
            modGUID = _modGUID;
            modName = _modName;
            modVersion = _modVersion;

            MLS = BepInEx.Logging.Logger.CreateLogSource(modGUID);
        }

        public void Log(string text = "", LogLevel level = LogLevel.Info)
        {
            string resultText = string.Format("[{0} v{1}] - {2}", modName, modVersion, text);

            MLS.Log(level, resultText);
        }
    }

    [BepInPlugin(modGUID, modName, modVersion)]
    public class TextSkip : BaseUnityPlugin
    {
        private const string modGUID = "thej01.itp.TextSkip";
        private const string modName = "Text Skip";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static TextSkip Instance;

        public static Logger logger = new Logger();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            logger.Init(modName, modVersion, modGUID);

            logger.Log("logger Initialised!");

            logger.Log("Patching TextSkip...");
            harmony.PatchAll(typeof(TextSkip));
            logger.Log("Patched TextSkip.");

            logger.Log("Patching TextBoxPatch...");
            harmony.PatchAll(typeof(TextBoxPatch));
            logger.Log("Patched TextBoxPatch.");
        }
    }
}
