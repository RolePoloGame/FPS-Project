using UnityEngine;

namespace RolePoloGame.Core.Extensions
{
    public static class ColorExtensions
    {
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public static string ToHexString(this Color color)
        {
            return ((byte)(color.r * 255)).ToString("X2") + ((byte)(color.g * 255)).ToString("X2") +
                   ((byte)(color.b * 255)).ToString("X2");
        }

        public static Color ToUnityColor(this System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}