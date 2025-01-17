﻿using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using HarmonyLib;
using Unity.Entities;
using Colossal.IO.AssetDatabase.Internal;

namespace SilenceXP
{
    public partial class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(SilenceXP)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        private Setting m_Setting;
        public static Mod Instance { get; private set; }

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));
            Instance = this;

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

            AssetDatabase.global.LoadSettings(nameof(SilenceXP), m_Setting, new Setting(this));

            // Actual method patching
            Harmony.CreateAndPatchAll(typeof(Mod));
            Harmony.GetAllPatchedMethods().ForEach(m => {
                if (m.GetType() == typeof(Mod))
                    log.Info($"Patched: {m.Name}");
            });
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
            Instance = null;
        }

        public Setting Setting => m_Setting;

        // namespace 
        // 	public class MilestoneUISystem : UISystemBase, IXPMessageHandler
        // public void AddMessage(XPMessage message)
        [HarmonyPatch(typeof(Game.UI.InGame.MilestoneUISystem), "AddMessage")]
        [HarmonyPrefix]
        static bool PatchedAddMessage(Game.UI.InGame.MilestoneUISystem __instance)
        {
            log.Debug("PatchedAddMessage");
            var mod = Mod.Instance;
            if (mod == null) {
                log.Info("mod does not exist");
                return true;
            }
            log.Debug($"mod.Setting.Toggle: {mod.Setting.Toggle}");
            return !mod.Setting.Toggle;
        }
    }
}
