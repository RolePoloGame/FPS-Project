using UnityEngine;

namespace RolePoloGame.Core.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector2 WithX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        public static Vector2 WithY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }

        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        /// <summary>
        /// Rotates given vector along Y axis by given degree
        /// </summary>
        /// <param name="vector">vector</param>
        /// <param name="angle">in degrees</param>
        /// <returns></returns>
        public static Vector3 RotateY(this Vector3 vector, float angle)
        {
            return Quaternion.AngleAxis(angle % 360.0f, Vector3.up) * vector;
        }

        public static Vector2 ConvertedToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }
    }
}