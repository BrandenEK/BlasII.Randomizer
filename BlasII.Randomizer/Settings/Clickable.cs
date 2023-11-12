using System;
using UnityEngine;

namespace BlasII.Randomizer.Settings
{
    public class Clickable
    {
        public RectTransform Rect => _rect;

        public void OnClick()
        {
            _onClick?.Invoke();
        }

        public Clickable(RectTransform rect, Action onClick)
        {
            _rect = rect;
            _onClick = onClick;
        }

        private readonly RectTransform _rect;
        private readonly Action _onClick;
    }
}
