using UnityEngine;

namespace BlasII.Randomizer.Extensions
{
    public static class UIExtensions
    {
        public static bool OverlapsPoint(this RectTransform rect, Vector2 point)
        {
            float xScale = (float)Screen.width / 1920;
            var scaling = new Vector3(xScale, xScale, (Screen.height - 1080 * xScale) * 0.5f);

            Vector2 position = Camera.main.WorldToScreenPoint(rect.position);
            position = new Vector2(position.x * scaling.x, position.y * scaling.y + scaling.z);

            var size = new Vector2(rect.rect.width * scaling.x, rect.rect.height * scaling.y);

            float leftBound = position.x + size.x * -rect.pivot.x;
            float rightBound = position.x + size.x * (1 - rect.pivot.x);
            float lowerBound = position.y + size.y * -rect.pivot.y;
            float upperBound = position.y + size.y * (1 - rect.pivot.y);

            return point.x >= leftBound && point.x <= rightBound && point.y >= lowerBound && point.y <= upperBound;
        }
    }
}
