using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace RolePoloGame.Core
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeCoreControllers()
        {
            GameData gameData = Addressables.LoadAssetAsync<GameData>("GameData").WaitForCompletion();
            Addressables.LoadSceneAsync(gameData.ControllersScene, LoadSceneMode.Additive, false).WaitForCompletion();
        }
    }
}