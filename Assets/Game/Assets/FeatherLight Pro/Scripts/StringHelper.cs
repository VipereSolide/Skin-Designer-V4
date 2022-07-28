using System.Linq;
using System.IO;
using System;

using UnityEngine;

namespace FeatherLight.Pro
{
    public static class StringHelper
    {
        public static bool ContainsArray(string text, string[] _values)
        {
            bool _output = false;

            foreach (string _v in _values)
            {
                if (text.Contains(_v))
                    _output = true;
            }

            return _output;
        }

        public static string ReplaceAll(this string value, string[] values, string newValue)
        {
            string output = value;

            foreach (string s in values)
            {
                output = output.Replace(s, newValue);
            }

            return output;
        }

        public static string RemoveStartSpace(this string value)
        {
            string output = value;

            int instances = 0;
            if (instances < 500)
            {
                while (output[0] == ' ')
                {
                    instances++;
                    output = output.Remove(0, 1);
                }
            }

            return output;
        }

        public static string RemoveStartWord(this string value, string removedWord)
        {
            if (!value.Contains(removedWord))
            {
                Debug.LogError("The string does not contain any such word: " + removedWord);
                return value;
            }

            string output = value;

            int wordPosition = value.IndexOf(removedWord);
            output = output.Remove(wordPosition, removedWord.Length);

            return output;
        }

        public static string RemoveControlCharacters(this string value)
        {
            string output = value;

            for(int i = 0; i < output.Length; i++)
            {
                if (char.IsControl(output[i]))
                {
                    output = output.Remove(i, 1);
                }
            }

            return output;
        }
    }
}