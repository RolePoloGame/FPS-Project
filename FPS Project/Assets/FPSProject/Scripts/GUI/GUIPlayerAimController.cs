using Assets.FPSProject.Scripts.Core.PlayerController;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerAimController : MonoBehaviour
{
    [SerializeField]
    private Image reload;
    [SerializeField]
    private PlayerWeaponController playerWeaponController;
    // Update is called once per frame
    void Update()
    {
        if (!playerWeaponController.IsReloading)
        {
            reload.enabled = false;
            return;
        }
        reload.enabled = true;
        reload.fillAmount = playerWeaponController.GetReloadActionRatio();
    }
}
