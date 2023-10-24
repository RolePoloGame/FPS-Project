using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.FPSProject.Scripts.Core.Items
{
    [CreateAssetMenu(menuName = "FPS/Item/Item")]
    public class Item : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; private set; } = "Example name";
        [field: SerializeField]
        public string Description { get; private set; } = "Example description";
        [field: SerializeField]
        public Texture2D Icon { get; private set; }
        [field: SerializeField]
        public Color IconColor { get; private set; } = Color.white;
        [field: SerializeField]
        public List<AssetReference> PickupModels { get; private set; }
    }
}