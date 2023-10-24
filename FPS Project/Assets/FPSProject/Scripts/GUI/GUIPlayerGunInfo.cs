using Assets.FPSProject.Scripts.Core.PlayerController;
using RolePoloGame.Core.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FPSProject.Scripts.GUI
{
    public class GUIPlayerGunInfo : MonoBehaviour
    {
        private const string INFINITY_SYMBOL = "\u221E";
        [SerializeField] private TextMeshProUGUI m_NameTMP;
        [SerializeField] private TextMeshProUGUI m_CurrentAmmoTMP;
        [SerializeField] private TextMeshProUGUI m_ClipTMP;
        [SerializeField] private TextMeshProUGUI m_MaxAmmoTMP;
        [SerializeField] private RawImage m_Icon;

        [SerializeField]
        private PlayerInventoryController m_InventoryController;
        [SerializeField]
        private PlayerWeaponController m_WeaponController;

        private void Start()
        {
            m_WeaponController.OnWeaponUpdate += UpdateGUI;
        }

        private void OnDestroy()
        {
            m_WeaponController.OnWeaponUpdate -= UpdateGUI;
        }

        public void UpdateGUI()
        {
            if (!m_WeaponController.HasWeapon)
            {
                m_NameTMP.SetText("Fists");
                m_Icon.color = Color.white.WithAlpha(0.0f);
                m_CurrentAmmoTMP.SetText(string.Empty);
                m_ClipTMP.SetText(string.Empty);
                m_MaxAmmoTMP.SetText(string.Empty);
                return;
            }

            m_NameTMP.SetText(m_WeaponController.Weapon.Name);
            m_Icon.texture = m_WeaponController.Weapon.Icon;
            m_Icon.color = m_WeaponController.Weapon.IconColor.WithAlpha(1.0f);

            if (!m_WeaponController.Weapon.HasAmmo)
            {
                m_CurrentAmmoTMP.SetText(INFINITY_SYMBOL);
                m_ClipTMP.SetText($"/{INFINITY_SYMBOL}");
                m_MaxAmmoTMP.SetText(INFINITY_SYMBOL);
            }
            else
            {
                m_CurrentAmmoTMP.SetText(m_WeaponController.CurrentAmmo.ToString());
                m_ClipTMP.SetText($"/{m_WeaponController.ClipSize}");
                m_MaxAmmoTMP.SetText(m_WeaponController.MaxAmmo.ToString());
            }
        }
    }
}