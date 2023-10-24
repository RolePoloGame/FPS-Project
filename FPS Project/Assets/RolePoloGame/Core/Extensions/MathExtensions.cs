using UnityEngine;

namespace RolePoloGame.Core.Extensions
{
    public static class MathExtensions
    {
        /// <summary>
        /// Implementation of Marsaglia polar method 
        /// https://en.wikipedia.org/wiki/Marsaglia_polar_method
        /// </summary>
        public class GaussianDistribution
        {
            private float m_SpareResult;
            private bool m_HasSpareResult = false;
            private readonly System.Random m_Random;

            public GaussianDistribution()
            {
                m_Random = new System.Random();
            }

            public float Next()
            {
                if (m_HasSpareResult)
                {
                    m_HasSpareResult = false;
                    return m_SpareResult;
                }

                float x, y, s;
                do
                {
                    x = 2f * (float)m_Random.NextDouble() - 1f;
                    y = 2f * (float)m_Random.NextDouble() - 1f;
                    s = x * x + y * y;
                } while (s >= 1f || s == 0);

                s = Mathf.Sqrt((-2f * Mathf.Log(s)) / s);

                m_SpareResult = y * s;
                m_HasSpareResult = true;
                return x * s;
            }

            public float Next(float mean, float sigma = 1f) => mean + sigma * Next();

            public float Next(float mean, float sigma, float min, float max)
            {
                float x = min - 1f;
                while (x < min || x > max)
                    x = Next(mean, sigma);
                return x;
            }

        }
    }
}