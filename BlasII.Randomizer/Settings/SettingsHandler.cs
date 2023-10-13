using BlasII.ModdingAPI.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTMPro;
using UnityEngine;

namespace BlasII.Randomizer.Settings
{
    public class SettingsHandler
    {
        private MainMenuWindowLogic _mainMenu;
        private GameObject _slotsMenu;
        private GameObject _settingsMenu;

        private int _currentSlot;

        private bool PressedEnter => CoreCache.Input.GetButtonDown("UI Confirm");
        private bool PressedCancel => CoreCache.Input.GetButtonDown("UI Cancel");

        // Forgot we cant use null coalescing  :(
        private bool SettingsMenuActive => _settingsMenu != null && _settingsMenu.activeInHierarchy;

        public void Update()
        {
            if (!SettingsMenuActive)
                return;

            if (PressedEnter)
            {
                StartNewGame();
            }
            else if (PressedCancel)
            {
                CloseSettingsMenu();
            }
        }

        /// <summary>
        /// Displays the settings menu and stores the current slot
        /// </summary>
        public void OpenSettingsMenu(int slot)
        {
            if (SettingsMenuActive)
                return;

            if (_settingsMenu == null)
                CreateSettingsMenu();

            Main.Randomizer.Log("Opening settings menu");
            _settingsMenu.SetActive(true);
            _slotsMenu.SetActive(false);

            UpdateSettingsMenu(Main.Randomizer.TempConfig);
            CoreCache.Input.ClearAllInputBlocks();
            _currentSlot = slot;
        }

        /// <summary>
        /// Closes the settings menu
        /// </summary>
        private void CloseSettingsMenu()
        {
            if (!SettingsMenuActive)
                return;

            Main.Randomizer.Log("Closing settings menu");
            _settingsMenu.SetActive(false);
            _mainMenu.OpenSlotMenu();
            _mainMenu.CloseSlotMenu();
        }

        /// <summary>
        /// Begins the game with the stored slot
        /// </summary>
        private void StartNewGame()
        {
            Main.Randomizer.LogWarning("Starting new game");
            NewGame_Settings_Patch.NewGameFlag = true;
            Object.FindObjectOfType<MainMenuWindowLogic>().NewGame(_currentSlot);
            NewGame_Settings_Patch.NewGameFlag = false;
        }

        /// <summary>
        /// Updates all of the settings in the menu based on the config
        /// </summary>
        private void UpdateSettingsMenu(TempConfig config)
        {
            string weaponName = config.startingWeapon switch
            {
                0 => "Censer", 1 => "Blade", 2 => "Rapier", _ => "Random"
            };

            _settingSW.SetText("Starting weapon: " + weaponName);
            _settingLD.SetText("Logic difficulty: Normal");
            _settingSLQ.SetText("Shuffle long quests: " + (config.shuffleLongQuests ? "Yes" : "No"));
            _settingSS.SetText("Shuffle shops: " + (config.shuffleShops ? "Yes" : "No"));
        }

        /// <summary>
        /// Creates the ui for the settings menu
        /// </summary>
        private void CreateSettingsMenu()
        {
            Main.Randomizer.LogWarning("Creating settings menu");

            var mainMenu = Object.FindObjectOfType<MainMenuWindowLogic>();
            var slotsMenu = mainMenu.slotsMenuView.transform.parent.gameObject;

            var settingsMenu = Object.Instantiate(slotsMenu, slotsMenu.transform.parent);
            settingsMenu.transform.Find("Header").GetComponent<UIPixelTextWithShadow>().SetText("RANDOMIZER SETTINGS");
            settingsMenu.transform.Find("Buttons/Button A/New/label").GetComponent<UIPixelTextWithShadow>().SetText("Begin");
            settingsMenu.transform.Find("Buttons/Back/label").GetComponent<UIPixelTextWithShadow>().SetText("Cancel");
            Object.Destroy(settingsMenu.transform.Find("SlotsList").gameObject);

            RectTransform mainSection = UIModder.CreateRect("Main Section", settingsMenu.transform)
                .SetSize(1800, 750)
                .SetPosition(0, -30);
            //.AddImage()
            //.SetColor(Color.red).rectTransform;

            _settingSW = CreateShadowText("SW", mainSection, new Vector2(0, 200), TEXT_COLOR, string.Empty);
            _settingLD = CreateShadowText("LD", mainSection, new Vector2(0, 100), TEXT_COLOR, string.Empty);
            _settingSLQ = CreateShadowText("SLQ", mainSection, new Vector2(0, 0), TEXT_COLOR, string.Empty);
            _settingSS = CreateShadowText("SS", mainSection, new Vector2(0, -100), TEXT_COLOR, string.Empty);

            //TMP_Text startingWeapon = UIModder.CreateRect("StartingWeapon", mainSection)
            //    .SetPosition(0, 200)
            //    .AddText()
            //    .SetContents("Starting weapon: Blade")
            //    .SetFontSize(TEXT_SIZE);

            //TMP_Text logicDifficulty = UIModder.CreateRect("LogicDifficulty", mainSection)
            //    .SetPosition(0, 100)
            //    .AddText()
            //    .SetContents("Logic difficulty: Normal")
            //    .SetFontSize(TEXT_SIZE);

            //TMP_Text shuffleLongQuests = UIModder.CreateRect("ShuffleLongQuests", mainSection)
            //    .SetPosition(0, 0)
            //    .AddText()
            //    .SetContents("Shuffle long quests: No")
            //    .SetFontSize(TEXT_SIZE);

            //TMP_Text shuffleShops = UIModder.CreateRect("ShuffleShops", mainSection)
            //    .SetPosition(0, -100)
            //    .AddText()
            //    .SetContents("Shuffle shops: Yes")
            //    .SetFontSize(TEXT_SIZE);

            _mainMenu = mainMenu;
            _settingsMenu = settingsMenu;
            _slotsMenu = slotsMenu;
        }

        private UIPixelTextWithShadow CreateShadowText(string name, Transform parent, Vector2 position, Color color, string text)
        {
            // Create shadow
            var shadow = UIModder.CreateRect(name, parent)
                .SetPosition(position)
                .AddText()
                .SetAlignment(TextAlignmentOptions.Center)
                .SetColor(new Color(0.004f, 0.008f, 0.008f))
                .SetFontSize(TEXT_SIZE)
                .SetContents(text);

            // Create normal
            var normal = UIModder.CreateRect(name, shadow.transform)
                .SetPosition(0, 4)
                .AddText()
                .SetAlignment(TextAlignmentOptions.Center)
                .SetColor(color)
                .SetFontSize(TEXT_SIZE)
                .SetContents(text);

            // Create component
            var pixelText = shadow.gameObject.AddComponent<UIPixelTextWithShadow>();
            pixelText.normalText = normal;
            pixelText.shadowText = shadow;

            return pixelText;
        }

        private const int TEXT_SIZE = 55;
        private readonly Color TEXT_COLOR = new Color32(192, 192, 192, 255);

        private UIPixelTextWithShadow _settingSW;
        private UIPixelTextWithShadow _settingLD;
        private UIPixelTextWithShadow _settingSLQ;
        private UIPixelTextWithShadow _settingSS;
    }
}
