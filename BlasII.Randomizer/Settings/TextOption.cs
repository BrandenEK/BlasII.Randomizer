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

        public string CurrentValue => _currentValue.Length > 0 ? _currentValue : string.Empty;
        public int CurrentNumericValue => int.TryParse(CurrentValue, out int value) ? value : 0;

        public void ToggleSelected()
        {
            _selected = !_selected;
            UpdateStatus();
        }

        public void SetValue(string value)
        {
            _currentValue = value;
            UpdateStatus();
        }

        private void Update()
        {
            if (!_selected)
                return;

            string input = Input.inputString;
            if (input.Length == 0)
                return;

            foreach (char c in input)
            {
                ProcessCharacter(c);
            }

            UpdateStatus();
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
            if (c == '\b')
                Backspace = c;
            else if (c == '\n' || c == '\r')
                Confirm = c;
            else if (_currentValue.Length < _maxLength)
            {
                if (char.IsWhiteSpace(c))
                    Whitespace = c;
                else if (!char.IsNumber(c))
                    Nonnumeric = c;
                else if (c == '0')
                    Zero = c;
                else
                    Numeric = c;
            }
        }

        char Backspace
        {
            set
            {
                if (_currentValue.Length > 0)
                    _currentValue = _currentValue[..^1];
            }
        }

        char Confirm
        {
            set
            {
                _selected = false;
            }
        }

        char Whitespace
        {
            set
            {
                if (_currentValue.Length > 0 && !_numeric)
                    _currentValue += value;
            }
        }

        char Nonnumeric
        {
            set
            {
                if (!_numeric)
                    _currentValue += value;
            }
        }

        char Zero
        {
            set
            {
                if (_allowZero || _currentValue.Length > 0)
                    _currentValue += value;
            }
        }

        char Numeric
        {
            set
            {
                _currentValue += value;
            }
        }
    }
}
