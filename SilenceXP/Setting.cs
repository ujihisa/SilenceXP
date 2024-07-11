using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using System.Collections.Generic;

namespace SilenceXP
{
    [FileLocation(nameof(SilenceXP))]
    [SettingsUIGroupOrder(kToggleGroup)]
    [SettingsUIShowGroupName( kToggleGroup)]
    public class Setting : ModSetting
    {
        public const string kSection = "Main";

        public const string kToggleGroup = "Toggle";

        public Setting(IMod mod) : base(mod) {
        }

        [SettingsUISection(kSection, kToggleGroup)]
        public bool Toggle { get; set; }

        public override void SetDefaults()
        {
            Toggle = true;
        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;
        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Silence XP" },
                { m_Setting.GetOptionTabLocaleID(Setting.kSection), "Main" },

                { m_Setting.GetOptionGroupLocaleID(Setting.kToggleGroup), "Toggle enable" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Toggle)), "Toggle enable" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Toggle)), $"Use bool property with setter and getter to get toggable option" },
            };
        }

        public void Unload() {
        }
    }
}
