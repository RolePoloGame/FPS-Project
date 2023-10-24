namespace RolePoloGame.Core.Extensions
{
    public static class RandomExtensions
    {
        public static int Next(this System.Random random, int min, int max)
        {
            return random.Next(max - min) + min;
        }

        public static float Range(this System.Random random, float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }
    }
}