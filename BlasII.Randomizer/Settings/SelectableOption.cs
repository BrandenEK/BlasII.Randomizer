using Il2CppTGK.Game.Components.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Settings
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    internal class SelectableOption : MonoBehaviour
    {
        private UIPixelTextWithShadow _optionText;
        private Image _arrowImage;

        private SelectableOption _otherArrow;
        private bool _facingRight;

        private string[] _options;
        private int _currentOption;

        public void Initialize(UIPixelTextWithShadow optionText, Image arrowImage, SelectableOption otherArrow, bool facingRight, string[] options)
        {
            _optionText = optionText;
            _arrowImage = arrowImage;

            _otherArrow = otherArrow;
            _facingRight = facingRight;

            _options = options;
        }

        public int CurrentOption => _currentOption;

        public void SetOption(int option)
        {
            if (option < 0 || option >= _options.Length)
                return;

            _currentOption = option;
            _optionText.SetText(_options[_currentOption]);
            UpdateActiveStatus();
        }

        public void ChangeOption()
        {
            //if (_facingRight)
            //{
            //    _currentOption++;
            //    if (_currentOption == _options.Length)
            //        _currentOption = 0;
            //}
            //else
            //{
            //    _currentOption--;
            //    if (_currentOption < 0)
            //        _currentOption = _options.Length - 1;
            //}

            int option = _currentOption + (_facingRight ? 1 : -1);
            if (option < 0 || option >= _options.Length)
                return;

            _currentOption = option;
            _optionText.SetText(_options[_currentOption]);
            UpdateActiveStatus();

            _otherArrow.SetOption(_currentOption);
        }

        private void UpdateActiveStatus()
        {
            if (_facingRight)
            {
                _arrowImage.sprite = Main.Randomizer.Data.GetUI(_currentOption == _options.Length - 1 ? DataStorage.UIType.RightInactive : DataStorage.UIType.RightActive);
            }
            else
            {
                _arrowImage.sprite = Main.Randomizer.Data.GetUI(_currentOption == 0 ? DataStorage.UIType.LeftInactive : DataStorage.UIType.LeftActive);
            }
        }
    }
}
