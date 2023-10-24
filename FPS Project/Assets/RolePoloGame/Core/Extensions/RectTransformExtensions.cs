using UnityEngine;

namespace RolePoloGame.Core.Extensions
{
    public static class RectTransformExtensions
    {
        public static float Width(this RectTransform rect)
        {
            return rect.rect.width;
        }
        public static float Height(this RectTransform rect)
        {
            return rect.rect.height;
        }
    }
}