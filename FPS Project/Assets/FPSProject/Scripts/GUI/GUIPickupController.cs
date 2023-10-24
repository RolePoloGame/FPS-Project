using Assets.FPSProject.Scripts.Core.Items.Weapons;
using Assets.FPSProject.Scripts.Core.PlayerController;
using RolePoloGame.Core;
using System.Collections;
using UnityEngine;

public class GUIPickupController : MonoBehaviour
{
    [SerializeField]
    private PlayerInventoryController playerInventoryController;
    [SerializeField]
    private float displayTime = 3.0f;
    private void Start()
    {
        playerInventoryController.OnWeaponAdded += ShowItemAdded;
    }

    [SerializeField]
    private ObjectPool<GUIItem> ItemPool = new ObjectPool<GUIItem>();

    private void ShowItemAdded(Weapon weapon)
    {
        var item = ItemPool.Get();
        item.Set(weapon);
        item.FadeIn();
        StartCoroutine(FadeOut(item));
    }

    private IEnumerator FadeOut(GUIItem item)
    {
        yield return new WaitForSeconds(displayTime);
        item.FadeOut();
        ItemPool?.Release(item);
    }
}
