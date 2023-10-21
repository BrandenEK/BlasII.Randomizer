using BlasII.ModdingAPI.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Settings
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class ToggleOption : MonoBehaviour
    {
        private Image _toggleBox;

        private bool _toggled;

        public bool Toggled
        {
            get => _toggled;
            set
            {
                _toggled = value;
                UpdateStatus();
            }
        }

        public void Toggle()
        {
            Toggled = !Toggled;
            Main.Randomizer.AudioHandler.PlayEffectUI(UISFX.ChangeSelection);
        }

        public void Initialize(Image toggleBox)
        {
            _toggleBox = toggleBox;
        }

        private void UpdateStatus()
        {
            _toggleBox.sprite = Main.Randomizer.Data.GetUI(_toggled ? DataStorage.UIType.ToggleOn : DataStorage.UIType.ToggleOff);
        }
    }
}
