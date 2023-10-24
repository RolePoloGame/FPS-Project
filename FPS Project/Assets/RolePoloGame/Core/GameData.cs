using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RolePoloGame.Core
{
    [CreateAssetMenu(menuName = "RolePoloGame/Core/GameData")]
    public sealed class GameData : ScriptableObject
    {
        public AssetReference ControllersScene;
    }
}