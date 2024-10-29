using Il2CppTGK.Game.Components.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Settings;

[MelonLoader.RegisterTypeInIl2Cpp]
public class ArrowOption : MonoBehaviour
{
    private UIPixelTextWithShadow _optionText;
    private Image _leftArrow;
    private Image _rightArrow;

    private string[] _options;
    private int _currentOption;

    public int CurrentOption
    {
        get => _currentOption;
        set
        {
            _currentOption = value;
            UpdateStatus();
        }
    }

    public void ChangeOption(int change)
    {
        int newOption = _currentOption + change;
        if (newOption < 0 || newOption >= _options.Length)
            return;

        CurrentOption = newOption;
        Main.Randomizer.AudioHandler.PlayEffectUI(UISFX.ChangeSelection);
    }

    public void Initialize(UIPixelTextWithShadow optionText, Image leftArrow, Image rightArrow, string[] options)
    {
        _optionText = optionText;
        _leftArrow = leftArrow;
        _rightArrow = rightArrow;

        _options = options;
    }

    private void UpdateStatus()
    {
        string text = Main.Randomizer.LocalizationHandler.Localize(_options[_currentOption]);
        _optionText.SetText(text);
        _leftArrow.sprite = Main.Randomizer.Data.GetUI(_currentOption == 0 ? DataStorage.UIType.ArrowLeftOff : DataStorage.UIType.ArrowLeftOn);
        _rightArrow.sprite = Main.Randomizer.Data.GetUI(_currentOption == _options.Length - 1 ? DataStorage.UIType.ArrowRightOff : DataStorage.UIType.ArrowRightOn);
    }
}
