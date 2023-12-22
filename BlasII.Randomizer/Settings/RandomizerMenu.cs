using BlasII.ModdingAPI.Menus;
using BlasII.ModdingAPI.UI;
using Il2CppTGK.Game.Components.UI;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Settings
{
    public class RandomizerMenu : BaseMenu
    {
        public RandomizerMenu() : base("head", 0) { }

        /// <summary>
        /// Stores or loads the entire settings menu into or from a settings object
        /// </summary>
        private RandomizerSettings MenuSettings
        {
            get
            {
                int logicDifficulty = 1;
                int requiredKeys = _setRequiredKeys.CurrentOption;
                int startingWeapon = _setStartingWeapon.CurrentOption;
                bool shuffleLongQuests = _setShuffleLongQuests.Toggled;
                bool shuffleShops = _setShuffleShops.Toggled;

                int seed = _setSeed.CurrentNumericValue == 0 ? RandomizerSettings.RandomSeed : _setSeed.CurrentNumericValue;
                return new RandomizerSettings(seed, logicDifficulty, requiredKeys, 0, startingWeapon, 0, shuffleLongQuests, shuffleShops, true, 0, 0);
            }
            set
            {
                _setLogicDifficulty.CurrentOption = 0;
                _setRequiredKeys.CurrentOption = value.requiredKeys;
                _setStartingWeapon.CurrentOption = value.startingWeapon;
                _setShuffleLongQuests.Toggled = value.shuffleLongQuests;
                _setShuffleShops.Toggled = value.shuffleShops;

                _setSeed.CurrentValue = string.Empty;
            }
        }

        /// <summary>
        /// Set the menu options to default when opening it
        /// </summary>
        public override void OnStart()
        {
            MenuSettings = RandomizerSettings.DefaultSettings;
        }

        /// <summary>
        /// Update the randomizer settings when closing it
        /// </summary>
        public override void OnFinish()
        {
            Main.Randomizer.CurrentSettings = MenuSettings;
        }

        protected override void CreateUI(Transform ui)
        {
            _setSeed = CreateTextOption("Seed", ui, new Vector2(0, 300), 150,
                "seed", true, false, 6);

            _setLogicDifficulty = CreateArrowOption("LD", ui, new Vector2(-300, 80),
                "opld", _opLogic);

            _setRequiredKeys = CreateArrowOption("RQ", ui, new Vector2(-300, -80),
                "oprq", _opKeys);

            _setStartingWeapon = CreateArrowOption("SW", ui, new Vector2(-300, -240),
                "opsw", _opWeapon);

            _setShuffleLongQuests = CreateToggleOption("SL", ui, new Vector2(150, 70),
                "opsl");

            _setShuffleShops = CreateToggleOption("SS", ui, new Vector2(150, -10),
                "opss");
        }

        /// <summary>
        /// Adds a shadow text to the UI
        /// </summary>
        private UIPixelTextWithShadow CreateShadowText(string name, Transform parent, Vector2 position, int size, Color color, Vector2 pivot, TextAlignmentOptions alignment, string text)
        {
            return UIModder.CreateRect(name, parent)
                .SetPosition(position)
                .SetPivot(pivot)
                .AddText()
                .SetAlignment(alignment)
                .SetColor(color)
                .SetFontSize(size)
                .SetContents(text)
                .AddShadow();
        }

        /// <summary>
        /// Adds a true/false setting to the UI
        /// </summary>
        private ToggleOption CreateToggleOption(string name, Transform parent, Vector2 position, string header)
        {
            // Create ui holder
            var holder = UIModder.CreateRect(name, parent).SetPosition(position);

            // Create text and images
            var headerText = CreateShadowText("header", holder, position + Vector2.right * 12 + Vector2.down * 3,
                TEXT_SIZE, SILVER,
                new Vector2(0, 0.5f), TextAlignmentOptions.Left, string.Empty);
            Main.Randomizer.LocalizationHandler.AddPixelTextLocalizer(headerText, header);

            var toggleBox = CreateToggleImage("box", holder, position);

            // Initialize toggle option
            var selectable = holder.gameObject.AddComponent<ToggleOption>();
            selectable.Initialize(toggleBox);

            // Add click events
            AddClickable(toggleBox.rectTransform, () => selectable.Toggle());

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

        /// <summary>
        /// Adds a multi-option setting to the UI
        /// </summary>
        private ArrowOption CreateArrowOption(string name, Transform parent, Vector2 position, string header, string[] options)
        {
            // Create ui holder
            var holder = UIModder.CreateRect(name, parent).SetPosition(position);

            // Create text and images
            var headerText = CreateShadowText("header", holder, Vector2.up * 60,
                TEXT_SIZE, SILVER,
                new Vector2(0.5f, 0.5f), TextAlignmentOptions.Center, string.Empty);
            Main.Randomizer.LocalizationHandler.AddPixelTextLocalizer(headerText, header);

            var optionText = CreateShadowText("option", holder, Vector2.zero,
                TEXT_SIZE - 5, YELLOW,
                new Vector2(0.5f, 0.5f), TextAlignmentOptions.Center, string.Empty);

            var leftArrow = CreateArrowImage("left", holder, Vector2.left * 150);
            var rightArrow = CreateArrowImage("right", holder, Vector2.right * 150);

            // Initialize arrow option
            var selectable = holder.gameObject.AddComponent<ArrowOption>();
            selectable.Initialize(optionText, leftArrow, rightArrow, options);

            // Add click events
            AddClickable(leftArrow.rectTransform, () => selectable.ChangeOption(-1));
            AddClickable(rightArrow.rectTransform, () => selectable.ChangeOption(1));

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

        /// <summary>
        /// Adds a text-entry setting to the UI
        /// </summary>
        private TextOption CreateTextOption(string name, Transform parent, Vector2 position, int lineSize, string header, bool numeric, bool allowZero, int max)
        {
            // Create ui holder
            var holder = UIModder.CreateRect(name, parent).SetPosition(position);

            // Create text and images
            var headerText = CreateShadowText("header", holder, Vector2.left * 10,
                TEXT_SIZE, SILVER,
                new Vector2(1, 0.5f), TextAlignmentOptions.Right, string.Empty);
            Main.Randomizer.LocalizationHandler.AddPixelTextLocalizer(headerText, header);

            var valueText = CreateShadowText("value", holder, Vector2.right * lineSize / 2,
                TEXT_SIZE - 5, YELLOW,
                new Vector2(0.5f, 0.5f), TextAlignmentOptions.Center, string.Empty);

            var underline = CreateLineImage("line", holder, Vector2.zero, lineSize);

            // Initialize text option
            var selectable = holder.gameObject.AddComponent<TextOption>();
            selectable.Initialize(underline, valueText, numeric, allowZero, max);

            // Add click events
            AddClickable(underline.rectTransform,
                () => selectable.SetSelected(true), () => selectable.SetSelected(false));

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

        private const int TEXT_SIZE = 55;
        private readonly Color SILVER = new Color32(192, 192, 192, 255);
        private readonly Color YELLOW = new Color32(255, 231, 65, 255);

        private readonly string[] _opLogic = new string[] { "o2ld" }; // "Easy", "Normal", "Hard"
        private readonly string[] _opKeys = new string[] { "rand", "o1rq", "o2rq", "o3rq", "o4rq", "o5rq", "o6rq" };
        private readonly string[] _opWeapon = new string[] { "rand", "o1sw", "o2sw", "o3sw" };

        private ArrowOption _setLogicDifficulty;
        private ArrowOption _setRequiredKeys;
        private ArrowOption _setStartingWeapon;

        private ToggleOption _setShuffleLongQuests;
        private ToggleOption _setShuffleShops;

        private TextOption _setSeed;

    }
}
