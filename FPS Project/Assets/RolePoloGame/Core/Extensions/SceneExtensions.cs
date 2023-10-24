using UnityEngine.SceneManagement;

namespace RolePoloGame.Core.Extensions
{
    public static class SceneExtensions
    {
        public static bool IsGameplayScene(this Scene scene)
        {
            string sceneName = scene.name;
            string[] scenes = sceneName.Split("_");
            return scenes[0] == "Gameplay" && scenes[^1] == "Game";
        }
    }
}