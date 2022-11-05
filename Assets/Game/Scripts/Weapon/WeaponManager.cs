using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using SkinDesigner.SkinSystem;
using SkinDesigner.Inspector;
using SkinDesigner.Textures;
using FeatherLight.Pro.Console;

namespace SkinDesigner.Weapon
{
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance;

        [SerializeField] private WeaponObject[] m_weapons;

        private WeaponObject m_currentWeapon;
        public WeaponObject.WeaponTextureData CurrentTextures
        {
            get { return m_currentWeapon.MainProfile.Textures; }
        }
        public WeaponObject[] Weapons
        {
            get
            {
                return m_weapons;
            }
        }
        public WeaponObject CurrentWeapon
        {
            get
            {
                return m_currentWeapon;
            }
        }
        public WeaponObject GetWeaponByWeaponType(SkinDesigner.SkinSystem.Weapon weapon)
        {
            WeaponObject output = null;

            for (int i = 0; i < m_weapons.Length; i++)
            {
                if (m_weapons[i].Weapon == weapon)
                {
                    output = m_weapons[i];
                    break;
                }
            }

            return output;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            foreach (WeaponObject _Weapon in m_weapons)
            {
                _Weapon.gameObject.SetActive(false);
            }
        }

        public void InstantiateWeapon(SkinSystem.Weapon _InstantiatedWeapon)
        {
            int _weaponIndex = Environment.WeaponToInt(_InstantiatedWeapon);
            WeaponObject _toBeInstantiated = m_weapons[_weaponIndex];

            foreach (WeaponObject _Weapon in m_weapons)
            {
                _Weapon.Start();
                _Weapon.gameObject.SetActive(_Weapon == _toBeInstantiated);
            }

            m_currentWeapon = _toBeInstantiated;
        }
        public void UpdateTextureMap(TextureMap mapName)
        {
            if (m_currentWeapon == null)
            {
                Console.LogError($"Weapon Manager > There is no weapon selected. You must have a current weapon set to use UpdateTextureMap!");
                return;
            }

            m_currentWeapon.UpdateWeaponMap(mapName);
        }
        public void UpdateAllTextureMaps()
        {
            if (m_currentWeapon == null)
            {
                Console.LogError($"Weapon Manager > There is no weapon selected. You must have a current weapon set to use UpdateTextureMap!");
                return;
            }

            m_currentWeapon.UpdateWeaponObject();
        }
        public void UpdateAllPartTextures()
        {
            if (m_currentWeapon == null)
            {
                Console.LogError($"Weapon Manager > There is no weapon selected. You must have a current weapon set to use UpdateTextureMap!");
                return;
            }

            for (int c = 0; c < m_currentWeapon.WeaponSubRenderers.Length; c++)
            {
                for (int i = 0; i < 7; i++)
                {
                    m_currentWeapon.UpdatePartsMap(Environment.IntToTextureMap(i), c);
                }
            }
        }
    }
}