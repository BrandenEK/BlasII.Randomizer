using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using UnityEngine;

namespace BlasII.Randomizer.Settings
{
    public class SettingsHandler
    {
        private MainMenuWindowLogic _mainMenu;
        private GameObject _slotsMenu;
        private GameObject _settingsMenu;

        private int _currentSlot;

        private bool PressedEnter => Input.GetKeyDown(KeyCode.Return); // CoreCache.Input.GetButtonDown("UI Confirm");
        private bool PressedCancel => Input.GetKeyDown(KeyCode.Escape); // CoreCache.Input.GetButtonDown("UI Cancel");

        // Forgot we cant use null coalescing  :(
        private bool SettingsMenuActive => _settingsMenu != null && _settingsMenu.activeInHierarchy;

        public void Update()
        {
            if (!SettingsMenuActive)
                return;

            if (PressedEnter)
            {
                // Start game
                NewGame_Settings_Patch.NewGameFlag = true;
                Object.FindObjectOfType<MainMenuWindowLogic>().NewGame(_currentSlot);
                NewGame_Settings_Patch.NewGameFlag = false;
                Main.Randomizer.Log("Starting game");
            }
            else if (PressedCancel)
            {
                CloseSettingsMenu();
            }
        }

        public void OpenSettingsMenu(int slot)
        {
            if (SettingsMenuActive)
                return;

            if (_settingsMenu == null)
                CreateSettingsMenu();

            _settingsMenu.SetActive(true);
            _slotsMenu.SetActive(false);
            _currentSlot = slot;
            Main.Randomizer.LogWarning("Settings menu opened");
        }

        private void CloseSettingsMenu()
        {
            if (!SettingsMenuActive)
                return;

            _settingsMenu.SetActive(false);
            _mainMenu.OpenSlotMenu();
            _mainMenu.CloseSlotMenu();
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
            settingsMenu.transform.Find("Buttons/Button A/New/label").GetComponent<UIPixelTextWithShadow>().SetText("Begin");
            settingsMenu.transform.Find("Buttons/Back/label").GetComponent<UIPixelTextWithShadow>().SetText("Cancel");

            _mainMenu = mainMenu;
            _settingsMenu = settingsMenu;
            _slotsMenu = slotsMenu;
        }
    }
}
