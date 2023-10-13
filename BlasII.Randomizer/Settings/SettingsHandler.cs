using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using UnityEngine;

namespace BlasII.Randomizer.Settings
{
    public class SettingsHandler
    {
        private GameObject _settingsMenu;
        private GameObject _slotsMenu;

        private bool PressedEnter => CoreCache.Input.GetButtonDown("UI Confirm");
        private bool PressedCancel => CoreCache.Input.GetButtonDown("UI Cancel");

        private bool SettingsMenuActive => _settingsMenu?.activeInHierarchy ?? false;

        public void Update()
        {
            if (!SettingsMenuActive)
                return;

            if (PressedEnter)
            {
                // Start game
            }
            else if (PressedCancel)
            {
                CloseSettingsMenu();
            }
        }

        public void OpenSettingsMenu()
        {
            if (SettingsMenuActive)
                return;

            if (_settingsMenu == null)
                CreateSettingsMenu();

            _settingsMenu.SetActive(true);
            _slotsMenu.SetActive(false);
            Main.Randomizer.LogWarning("Settings menu opened");
        }

        private void CloseSettingsMenu()
        {
            if (!SettingsMenuActive)
                return;

            _settingsMenu.SetActive(false);
            _slotsMenu.SetActive(true);
            Main.Randomizer.LogWarning("Settings menu closed");
        }

        private void CreateSettingsMenu()
        {
            Main.Randomizer.Log("Creating settings menu");

            var mainMenu = Object.FindObjectOfType<MainMenuWindowLogic>();
            var slotsMenu = mainMenu.slotsMenuView.transform.parent.gameObject;

            var settingsMenu = Object.Instantiate(slotsMenu, slotsMenu.transform.parent);
            Object.Destroy(settingsMenu.transform.Find("SlotsList").gameObject);
            settingsMenu.transform.Find("Header").GetComponent<UIPixelTextWithShadow>().SetText("RANDOMIZER SETTINGS");

            _settingsMenu = settingsMenu;
            _slotsMenu = slotsMenu;
        }
    }
}
