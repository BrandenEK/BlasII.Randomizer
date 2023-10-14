using Il2CppTGK.Game.Components.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Settings
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class TextOption : MonoBehaviour
    {
        private Image _underline;
        private UIPixelTextWithShadow _text;

        private bool _numeric;
        private bool _allowZero;
        private int _maxLength;

        private string _currentValue;
        private bool _selected;

        public string CurrentValue
        {
            get => _currentValue.Length > 0 ? _currentValue : string.Empty;
            set
            {
                _currentValue = value;
                UpdateStatus();
            }
        }

        public int CurrentNumericValue => int.TryParse(CurrentValue, out int value) ? value : 0;

        public void ToggleSelected()
        {
            _selected = !_selected;
            UpdateStatus();
        }

        private void Update()
        {
            if (!_selected)
                return;

            foreach (char c in Input.inputString)
            {
                ProcessCharacter(c);
            }
        }

        public void Initialize(Image underline, UIPixelTextWithShadow text, bool numeric, bool allowZero, int maxLength)
        {
            _underline = underline;
            _text = text;

            _numeric = numeric;
            _allowZero = allowZero;
            _maxLength = maxLength;

            _currentValue = string.Empty;
        }

        private void UpdateStatus()
        {
            _underline.sprite = Main.Randomizer.Data.GetUI(_selected ? DataStorage.UIType.TextActive : DataStorage.UIType.TextInactive);
            _text.SetText(_currentValue.Length > 0 ? _currentValue : "---");
        }

        private void ProcessCharacter(char c)
        {
            if (c == '\r' || c == '\n')
                return;

            if (c == '\b')
            {
                HandleBackspace();
                return;
            }

            if (_currentValue.Length >= _maxLength)
                return;

            if (char.IsWhiteSpace(c))
            {
                HandleWhitespace(c);
            }
            else if (!char.IsNumber(c))
            {
                HandleNonNumeric(c);
            }
            else if (c == '0')
            {
                HandleZero();
            }
            else
            {
                HandleNumber(c);
            }
        }

        void HandleBackspace()
        {
            if (_currentValue.Length > 0)
                CurrentValue = _currentValue[..^1];
        }

        void HandleWhitespace(char c)
        {
            if (_currentValue.Length > 0 && !_numeric)
                CurrentValue += c;
        }

        void HandleNonNumeric(char c)
        {
            if (!_numeric)
                CurrentValue += c;
        }

        void HandleZero()
        {
            if (_allowZero || _currentValue.Length > 0)
                CurrentValue += '0';
        }

        void HandleNumber(char c)
        {
            CurrentValue += c;
        }
    }
}
