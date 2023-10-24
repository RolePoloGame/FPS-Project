using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Random = System.Random;

namespace RolePoloGame.Core.Extensions
{
    public static class ListExtensions
    {
        public static T GetRandom<T>(this IReadOnlyList<T> list)
        {
            if (list.Count == 0) return default;
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static List<T> GetRandomUnique<T>(this IReadOnlyList<T> list, int count)
        {
            if (list.Count < count)
                throw new System.ArgumentException(
                    $"Can't get {count} unique values, because list count is less than count.");

            var result = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                var random = list.GetRandom();
                while (result.Contains(random))
                {
                    random = list.GetRandom();
                }

                result.Add(random);
            }

            return result;
        }

        public static List<T> Shuffle<T>(this List<T> list)
        {
            var rand = new Random();
            var size = list.Count;
            return new List<T>(list.OrderBy((o) => (rand.Next() % size)));
        }

        public static LinkedList<T> Shuffle<T>(this LinkedList<T> list)
        {
            var rand = new Random();
            var size = list.Count;
            return new LinkedList<T>(list.OrderBy((o) => (rand.Next() % size)));
        }

        public static void AddUnique<T>(this List<T> list, T value)
        {
            if (list.Contains(value)) return;
            list.Add(value);
        }

        public static List<T> Add<T>(this List<T> list, List<T> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                list.Add(values[i]);
            }

            return list;
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
        public static T DeepCopy<T>(T item)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            var result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }

        public static int FindIndexOf<T>(this List<T> list, T item)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(item))
                    return i;
            }
            return -1;
        }
    }
}