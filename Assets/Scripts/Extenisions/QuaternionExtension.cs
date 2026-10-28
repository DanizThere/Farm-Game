using UnityEngine;

namespace Assets.Scripts.Extenisions
{
    public static class QuaternionExtension
    {
        public static QuaternionSerialize ToSerialize(this Quaternion quaternion)
        {
            var serialize = new QuaternionSerialize
            {
                W = quaternion.w,
                X = quaternion.x,
                Y = quaternion.y,
                Z = quaternion.z
            };

            return serialize;
        }

        public static Quaternion Deserialize(this QuaternionSerialize serialize)
        {
            var quaternion = new Quaternion
            {
                x = serialize.X,
                y = serialize.Y,
                z = serialize.Z,
                w = serialize.W
            };

            return quaternion;
        }
    }
}
