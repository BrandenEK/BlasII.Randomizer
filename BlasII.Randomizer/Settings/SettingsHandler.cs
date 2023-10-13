using BlasII.ModdingAPI.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.UI;

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
            Cursor.visible = SettingsMenuActive;
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
            _setStartingWeapon.SetOption(config.startingWeapon);
            _setLogicDifficulty.SetOption(1);
            _setShuffleLongQuests.SetOption(0);
            _setShuffleShops.SetOption(1);

            _setSeed.SetText("Seed: " + config.seed);
        }

        /// <summary>
        /// Creates the ui for the settings menu
        /// </summary>
        private void CreateSettingsMenu()
        {
            Main.Randomizer.LogWarning("Creating settings menu");

            Object.FindObjectOfType<CanvasScaler>().gameObject.AddComponent<GraphicRaycaster>();
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

            _setSeed = CreateShadowText("Seed", mainSection, new Vector2(0, 300), TEXT_SIZE, TEXT_COLOR, string.Empty);

            _setStartingWeapon = CreateArrowOption("SW", mainSection, new Vector2(-300, 80), "Starting weapon:", new string[]
            {
                "Random", "Veredicto", "Ruego", "Sarmiento"
            });

            _setLogicDifficulty = CreateArrowOption("LD", mainSection, new Vector2(-300, -80), "Logic difficulty:", new string[]
            {
                "Easy", "Normal", "Hard"
            });

            _setShuffleLongQuests = CreateArrowOption("SLQ", mainSection, new Vector2(300, 80), "Shuffle long quests:", new string[]
            {
                "No", "Yes"
            });

            _setShuffleShops = CreateArrowOption("SS", mainSection, new Vector2(300, -80), "Shuffle shops:", new string[]
            {
                "No", "Yes"
            });

            _mainMenu = mainMenu;
            _settingsMenu = settingsMenu;
            _slotsMenu = slotsMenu;
        }

        private UIPixelTextWithShadow CreateShadowText(string name, Transform parent, Vector2 position, int size, Color color, string text)
        {
            // Create shadow
            var shadow = UIModder.CreateRect(name, parent)
                .SetPosition(position)
                .AddText()
                .SetAlignment(TextAlignmentOptions.Center)
                .SetColor(new Color(0.004f, 0.008f, 0.008f))
                .SetFontSize(size)
                .SetContents(text);

            // Create normal
            var normal = UIModder.CreateRect(name, shadow.transform)
                .SetPosition(0, 4)
                .AddText()
                .SetAlignment(TextAlignmentOptions.Center)
                .SetColor(color)
                .SetFontSize(size)
                .SetContents(text);

            // Create component
            var pixelText = shadow.gameObject.AddComponent<UIPixelTextWithShadow>();
            pixelText.normalText = normal;
            pixelText.shadowText = shadow;

            return pixelText;
        }

        private ArrowOption CreateArrowOption(string name, Transform parent, Vector2 position, string header, string[] options)
        {
            // Create ui holder
            var holder = UIModder.CreateRect(name, parent).SetPosition(position);

            // Create text and images
            CreateShadowText("header", holder, Vector2.up * 60, TEXT_SIZE, TEXT_COLOR, header);
            var optionText = CreateShadowText("option", holder, Vector2.zero, TEXT_SIZE - 5, new Color32(255, 231, 65, 255), "Option");
            var leftArrow = CreateArrowImage("left", holder, Vector2.left * 150);
            var rightArrow = CreateArrowImage("right", holder, Vector2.right * 150);

            // Initialize arrow option
            var selectable = holder.gameObject.AddComponent<ArrowOption>();
            selectable.Initialize(optionText, leftArrow, rightArrow, options);

            // Add click events
            AddClickHandler(leftArrow.gameObject, () => selectable.ChangeOption(-1));
            AddClickHandler(rightArrow.gameObject, () => selectable.ChangeOption(1));

            return selectable;
        }

        private Image CreateArrowImage(string name, Transform parent, Vector2 position)
        {
            return UIModder.CreateRect(name, parent)
                .SetPosition(position + Vector2.up * 5)
                .SetSize(55, 55)
                .AddImage();
        }

        private void AddClickHandler(GameObject obj, System.Action onClick)
        {
            var button = obj.AddComponent<Button>();
            button.interactable = true;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(onClick);
        }

        private const int TEXT_SIZE = 55;
        private readonly Color TEXT_COLOR = new Color32(192, 192, 192, 255);

        private ArrowOption _setStartingWeapon;
        private ArrowOption _setLogicDifficulty;

        private ArrowOption _setShuffleLongQuests;
        private ArrowOption _setShuffleShops;

        private UIPixelTextWithShadow _setSeed;
    }
}
