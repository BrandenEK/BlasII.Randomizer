using Il2CppTGK.Game;
using Il2CppTGK.Game.PopupMessages;
using UnityEngine;

namespace BlasII.Randomizer.Settings
{
    public class SettingsHandler
    {
        /// <summary>
        /// Displays certain randomizer settings in an info popup
        /// </summary>
        public void DisplaySettings()
        {
            foreach (var mid in Resources.FindObjectsOfTypeAll<PopupMessageID>())
            {
                if (mid.name == "TESTPOPUP_id")
                    CoreCache.UINavigationHelper.ShowPopupMessage(mid);
            }
        }
    }
}
