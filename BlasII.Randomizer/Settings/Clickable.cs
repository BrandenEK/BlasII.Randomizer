using UnityEngine;

namespace BlasII.Randomizer.Settings
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class Clickable : MonoBehaviour
    {
        public delegate void OnClick();

        public OnClick ClickEvent { get; set; }
    }
}
