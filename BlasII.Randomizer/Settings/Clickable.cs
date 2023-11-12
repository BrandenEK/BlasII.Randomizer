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

        public void OnUnclick()
        {
            _onUnclick?.Invoke();
        }

        public Clickable(RectTransform rect, Action onClick, Action onUnclick = null)
        {
            _rect = rect;
            _onClick = onClick;
            _onUnclick = onUnclick;
        }

        private readonly RectTransform _rect;
        private readonly Action _onClick;
        private readonly Action _onUnclick;
    }
}
