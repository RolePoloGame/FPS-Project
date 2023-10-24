using System;
using System.Linq;

namespace RolePoloGame.Core.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns next enum in order
        /// </summary>
        public static T Next<T>(this T src) where T : Enum
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf(Arr, src) + 1;
            return Arr.Length == j ? Arr[0] : Arr[j];
        }

        /// <summary>
        /// Returns previous enum in order
        /// </summary>
        public static T Previous<T>(this T src) where T : Enum
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            var arr = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf(arr, src) - 1;
            return j < 0 ? arr[^1] : arr[j];
        }
        /// <summary>
        /// Returns enum with a value closest to a given value
        /// </summary>
        public static T GetNearestTo<T>(int value) where T : Enum
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
            var arr = (T[])Enum.GetValues(typeof(T));
            var closest = new Tuple<T, int>[arr.Length];
            for (var i = 0; i < arr.Length; i++)
            {
                closest[i] = new Tuple<T, int>(arr[i], (int)(object)arr[i]);
            }
            closest = closest.OrderBy(t => t.Item2 - value).ThenBy(t => t.Item1).ToArray();
            return closest[0].Item1;
        }
    }
}