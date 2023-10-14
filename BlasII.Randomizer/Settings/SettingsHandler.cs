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

            MenuSettings = RandomizerSettings.DefaultSettings;
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

            Main.Randomizer.CurrentSettings = MenuSettings;
            NewGame_Settings_Patch.NewGameFlag = true;
            Object.FindObjectOfType<MainMenuWindowLogic>().NewGame(_currentSlot);
            NewGame_Settings_Patch.NewGameFlag = false;
        }

        /// <summary>
        /// Stores or loads the entire settings menu into or from a settings object
        /// </summary>
        private RandomizerSettings MenuSettings
        {
            get
            {
                int startingWeapon = _setStartingWeapon.CurrentOption;
                int logicDifficulty = _setLogicDifficulty.CurrentOption;
                bool shuffleLongQuests = _setShuffleLongQuests.Toggled;
                bool shuffleShops = _setShuffleShops.Toggled;

                int seed = _setSeed.CurrentNumericValue == 0 ? RandomizerSettings.RandomSeed : _setSeed.CurrentNumericValue;
                return new RandomizerSettings(seed, logicDifficulty, 0, startingWeapon, 0, shuffleLongQuests, shuffleShops, true, 0, 0);
            }
            set
            {
                _setStartingWeapon.CurrentOption = value.startingWeapon;
                _setLogicDifficulty.CurrentOption = 0;
                _setShuffleLongQuests.Toggled = value.shuffleLongQuests;
                _setShuffleShops.Toggled = value.shuffleShops;

                _setSeed.CurrentValue = string.Empty;
            }
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

            _setSeed = CreateTextOption("Seed", mainSection, new Vector2(0, 300), 150, "Seed:", true, false, 6);

            _setStartingWeapon = CreateArrowOption("SW", mainSection, new Vector2(-300, 80), "Starting weapon", new string[]
            {
                "Random", "Veredicto", "Ruego", "Sarmiento"
            });

            _setLogicDifficulty = CreateArrowOption("LD", mainSection, new Vector2(-300, -80), "Logic difficulty", new string[]
            {
                "Normal" // "Easy", "Normal", "Hard"
            });

            _setShuffleLongQuests = CreateToggleOption("SLQ", mainSection, new Vector2(150, 70), "Shuffle long quests");

            _setShuffleShops = CreateToggleOption("SS", mainSection, new Vector2(150, -10), "Shuffle shops");

            _mainMenu = mainMenu;
            _settingsMenu = settingsMenu;
            _slotsMenu = slotsMenu;
        }

        private UIPixelTextWithShadow CreateShadowText(string name, Transform parent, Vector2 position, int size, Color color, Vector2 pivot, TextAlignmentOptions alignment, string text)
        {
            // Create shadow
            var shadow = UIModder.CreateRect(name, parent)
                .SetPosition(position)
                .SetPivot(pivot)
                .AddText()
                .SetAlignment(alignment)
                .SetColor(new Color(0.004f, 0.008f, 0.008f))
                .SetFontSize(size)
                .SetContents(text);

            // Create normal
            var normal = UIModder.CreateRect(name, shadow.transform)
                .SetPosition(0, 4)
                .AddText()
                .SetAlignment(alignment)
                .SetColor(color)
                .SetFontSize(size)
                .SetContents(text);

            // Create component
            var pixelText = shadow.gameObject.AddComponent<UIPixelTextWithShadow>();
            pixelText.normalText = normal;
            pixelText.shadowText = shadow;

            return pixelText;
        }

        private ToggleOption CreateToggleOption(string name, Transform parent, Vector2 position, string header)
        {
            // Create ui holder
            var holder = UIModder.CreateRect(name, parent).SetPosition(position);

            // Create text and images
            CreateShadowText("header", holder, position + Vector2.right * 12 + Vector2.down * 3,
                TEXT_SIZE, SILVER,
                new Vector2(0, 0.5f), TextAlignmentOptions.Left, header);

            var toggleBox = CreateToggleImage("box", holder, position);

            // Initialize toggle option
            var selectable = holder.gameObject.AddComponent<ToggleOption>();
            selectable.Initialize(toggleBox);

            // Add click events
            AddClickHandler(toggleBox.gameObject, () => selectable.Toggle());

            return selectable;

            // Creates the toggle box
            Image CreateToggleImage(string name, Transform parent, Vector2 position)
            {
                return UIModder.CreateRect(name, parent)
                    .SetPosition(position)
                    .SetPivot(1, 0.5f)
                    .SetSize(55, 55)
                    .AddImage();
            }
        }

        private ArrowOption CreateArrowOption(string name, Transform parent, Vector2 position, string header, string[] options)
        {
            // Create ui holder
            var holder = UIModder.CreateRect(name, parent).SetPosition(position);

            // Create text and images
            CreateShadowText("header", holder, Vector2.up * 60,
                TEXT_SIZE, SILVER,
                new Vector2(0.5f, 0.5f), TextAlignmentOptions.Center, header);

            var optionText = CreateShadowText("option", holder, Vector2.zero,
                TEXT_SIZE - 5, YELLOW,
                new Vector2(0.5f, 0.5f), TextAlignmentOptions.Center, string.Empty);

            var leftArrow = CreateArrowImage("left", holder, Vector2.left * 150);
            var rightArrow = CreateArrowImage("right", holder, Vector2.right * 150);

            // Initialize arrow option
            var selectable = holder.gameObject.AddComponent<ArrowOption>();
            selectable.Initialize(optionText, leftArrow, rightArrow, options);

            // Add click events
            AddClickHandler(leftArrow.gameObject, () => selectable.ChangeOption(-1));
            AddClickHandler(rightArrow.gameObject, () => selectable.ChangeOption(1));

            return selectable;

            // Creates the left and right arrows
            Image CreateArrowImage(string name, Transform parent, Vector2 position)
            {
                return UIModder.CreateRect(name, parent)
                    .SetPosition(position + Vector2.up * 5)
                    .SetSize(55, 55)
                    .AddImage();
            }
        }

        private TextOption CreateTextOption(string name, Transform parent, Vector2 position, int lineSize, string header, bool numeric, bool allowZero, int max)
        {
            // Create ui holder
            var holder = UIModder.CreateRect(name, parent).SetPosition(position);

            // Create text and images
            var headerText = CreateShadowText("header", holder, Vector2.left * 10,
                TEXT_SIZE, SILVER,
                new Vector2(1, 0.5f), TextAlignmentOptions.Right, header);

            var valueText = CreateShadowText("value", holder, Vector2.right * lineSize / 2,
                TEXT_SIZE - 5, YELLOW,
                new Vector2(0.5f, 0.5f), TextAlignmentOptions.Center, string.Empty);

            var underline = CreateLineImage("line", holder, Vector2.zero, lineSize);

            // Initialize text option
            var selectable = holder.gameObject.AddComponent<TextOption>();
            selectable.Initialize(underline, valueText, numeric, allowZero, max);

            // Add click events
            AddClickHandler(underline.gameObject, () => selectable.ToggleSelected());

            return selectable;

            // Creates the underline image
            Image CreateLineImage(string name, Transform parent, Vector2 position, int size)
            {
                return UIModder.CreateRect(name, parent)
                    .SetPosition(position)
                    .SetSize(size, 50)
                    .SetPivot(0, 0.5f)
                    .AddImage();
            }
        }

        private void AddClickHandler(GameObject obj, System.Action onClick)
        {
            var button = obj.AddComponent<Button>();
            button.interactable = true;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(onClick);
        }

        private const int TEXT_SIZE = 55;
        private readonly Color SILVER = new Color32(192, 192, 192, 255);
        private readonly Color YELLOW = new Color32(255, 231, 65, 255);

        private ArrowOption _setStartingWeapon;
        private ArrowOption _setLogicDifficulty;

        private ToggleOption _setShuffleLongQuests;
        private ToggleOption _setShuffleShops;

        private TextOption _setSeed;
    }
}
