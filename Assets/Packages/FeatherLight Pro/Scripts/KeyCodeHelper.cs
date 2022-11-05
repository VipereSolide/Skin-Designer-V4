using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherLight.Pro
{
    public static class KeyCodeHelper
    {
        public static bool[] GetPressedKeys(KeyCode[] keys)
        {
            bool[] pressedKeys = new bool[keys.Length];

            for(int i = 0; i < keys.Length; i++)
            {
                pressedKeys[i] = Input.GetKey(keys[i]);
            }

            return pressedKeys;
        }

        public static bool AreKeyPressed(KeyCode[] keys)
        {
            bool areKeyPressed = true;

            for(int i = 0; i < keys.Length; i++)
            {
                if (i == keys.Length - 1 && areKeyPressed)
                {
                    areKeyPressed = Input.GetKeyDown(keys[i]);
                    break;
                }

                if (!Input.GetKey(keys[i]))
                {
                    areKeyPressed = false;
                }
            }

            return areKeyPressed;
        }
    }
}