using System;
using System.Linq;
using System.Text.RegularExpressions;
using RolePoloGame.Core.Extensions;
using UnityEngine;

namespace RolePoloGame.Core.Extensions
{
    public static class StringExtensions
    {
        // https://stackoverflow.com/questions/13599/convert-enums-to-human-readable-values
        public static string Humanize(this string input)
        {
            Regex r = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
            return r.Replace(input, " ${x}");
        }

        /// <summary>
        /// Gets currently string between
        /// </summary>
        /// <param name="word">Currently string</param>
        /// <param name="start">String left</param>
        /// <param name="end">String right</param>
        /// <returns>String between start and end</returns>
        /// <example>The string "value (4815162342)" use Between("(",")") generates in method: "4815162342"</example>
        public static string Between(this string word, char start, char end)
        {
            if (start.Equals(end))
                throw new ArgumentException("Start string can't equals a end string.");

            int startIndex = word.LastIndexOf(start) + 1;
            int endIndex = word.LastIndexOf(end) - 1 - word.LastIndexOf(start);

            return word.Substring(startIndex, endIndex);
        }

        public static string[] BetweenAll(this string word, char start, char end)
        {
            if (start.Equals(end))
                throw new ArgumentException("Start string can't equals a end string.");

            int countStart = word.Count(x => x == start);
            int countEnd = word.Count(x => x == end);

            if (countStart != countEnd)
                throw new ArgumentException("Start string count must equals a end string count.");

            string[] words = word.Split(new[] { start }, StringSplitOptions.None);
            string[] result = new string[words.Length - 1];
            for (int i = 1; i < words.Length; i++)
            {
                result[i - 1] = words[i].Between(start, end);
            }

            return result;
        }

        public static string BetweenContaining(this string word, char start, char end, string containing)
        {
            if (start.Equals(end))
                throw new ArgumentException("Start string can't equals a end string.");

            string[] all = word.BetweenAll(start, end);
            foreach (string s in all)
            {
                if (s.Contains(containing))
                    return s;
            }

            return null;
        }

        public static string Bold(this string word) => $"<b>{word}</b>";
        public static string Italics(this string word) => $"<i>{word}</i>";
        public static string Underline(this string word) => $"<u>{word}</u>";
        public static string Colored(this bool word) => $"<color=#{(word ? UnityEngine.Color.green.ToHexString() : UnityEngine.Color.red.ToHexString())}>{word}</color>";
        public static string Color(this string word, Color color) => $"<color=#{color.ToHexString()}>{word}</color>";
        public static string Size(this string word, int size) => $"<size={size}>{word}</size>";
    }
}