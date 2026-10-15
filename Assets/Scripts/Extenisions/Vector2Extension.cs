using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Extenisions
{
    public static class Vector2Extension
    {
        public static Vector2Serialize ToSerializable(this Vector2 vector)
        {
            var ser = new Vector2Serialize
            {
                X = vector.x,
                Y = vector.y
            };

            return ser;
        }

        public static Vector2 ToVector(this Vector2Serialize ser)
        {
            var vector = new Vector2
            {
                x = ser.X,
                y = ser.Y
            };

            return vector;
        }
    }
}
