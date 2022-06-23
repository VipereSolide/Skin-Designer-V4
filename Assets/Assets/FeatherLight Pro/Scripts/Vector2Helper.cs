using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherLight.Pro
{
    public static class Vector2Helper
    {
        public static Vector2 Compare(this Vector2 value, Vector2 compared)
        {
            float _comparedX = value.x - compared.x;
            float _comparedY = value.y - compared.y;

            return new Vector2(_comparedX, _comparedY);
        }

        public static Vector3 ToVector3(this Vector2 value)
        {
            return new Vector3(value.x, value.y, 0);
        }
    }
}