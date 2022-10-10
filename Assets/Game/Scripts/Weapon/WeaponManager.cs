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
        public TextureObject[] CurrentTextures
        {
            get { return m_currentWeapon.WeaponTextures.TextureObjects; }
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

        public void InstantiateWeapon(SkinDesigner.SkinSystem.Weapon _InstantiatedWeapon)
        {
            int _weaponIndex = SkinDesigner.SkinSystem.Environment.WeaponToInt(_InstantiatedWeapon);
            WeaponObject _toBeInstantiated = m_weapons[_weaponIndex];

            foreach (WeaponObject _Weapon in m_weapons)
            {
                _Weapon.Start();
                _Weapon.gameObject.SetActive(_Weapon == _toBeInstantiated);
            }

            m_currentWeapon = _toBeInstantiated;
        }

        public void UpdateTextureMap(TextureMap _MapName)
        {
            if (m_currentWeapon == null)
            {
                Console.LogError($"Weapon Manager > There is no weapon selected. You must have a current weapon set to use UpdateTextureMap!");
                return;
            }

            string _trueTextureName = Environment.GetTextureMapRealName(_MapName);
            int _textureIndex = Environment.TextureMapToInt(_MapName);
            Texture texture = m_currentWeapon.WeaponTextures.TextureObjects[_textureIndex].Texture;

            if (texture == null)
            {
                Console.LogError($"Weapon Manager > The texture of \"{_MapName}\" is null.");
                texture = m_currentWeapon.WeaponTextures.TextureObjects[_textureIndex].GetTextureFromPath();
            }

            MeshRenderer weaponRenderer = m_currentWeapon.GetComponent<MeshRenderer>();

            Console.LogWarning($"Weapon Manager > Updating current weapon material field \"{_trueTextureName}\" to new texture...");

            Texture before = weaponRenderer.material.GetTexture(_trueTextureName);
            weaponRenderer.material.SetTexture(_trueTextureName, texture);

            if (weaponRenderer.material.GetTexture(_trueTextureName) == before && texture != before)
            {
                if (before == null)
                {
                    Console.LogError($"Weapon Manager > The true texture name doesn't exist.");
                }

                Console.LogError($"Weapon Manager > Setting new material failed.");
            }
        }

        public void UpdateAllTextureMaps()
        {
            if (m_currentWeapon == null)
                return;

            for (int i = 0; i < 7; i++)
            {
                TextureMap _textureMap = SkinDesigner.SkinSystem.Environment.IntToTextureMap(i);
                string _trueTextureName = SkinDesigner.SkinSystem.Environment.GetTextureMapRealName(_textureMap);
                Texture tex = m_currentWeapon.WeaponTextures.TextureObjects[i].GetTextureFromPath();

                if (tex == null)
                {
                    RemoveTexture(i);
                }
                else
                {
                    m_currentWeapon.GetComponent<MeshRenderer>().material.SetTexture(_trueTextureName, tex);
                }
            }
        }

        public void UpdateAllPartTextures()
        {
            if (m_currentWeapon == null)
                return;

            for (int c = 0; c < m_currentWeapon.WeaponSubRenderers.Length; c++)
            {
                for (int i = 0; i < 7; i++)
                {
                    TextureMap _textureMap = SkinDesigner.SkinSystem.Environment.IntToTextureMap(i);
                    string _trueTextureName = SkinDesigner.SkinSystem.Environment.GetTextureMapRealName(_textureMap);

                    m_currentWeapon.WeaponSubRenderers[c].Renderer.material.SetTexture(_trueTextureName, m_currentWeapon.WeaponSubRenderers[c].TextureData.TextureObjects[i].GetTextureFromPath());
                }
            }
        }

        public void SetAllTextures(TextureObject[] _TextureObjects, bool _AutoUpdate = true)
        {
            if (m_currentWeapon == null)
                return;

            m_currentWeapon.WeaponTextures.TextureObjects = _TextureObjects;

            if (_AutoUpdate)
                UpdateAllTextureMaps();

        }

        public void SetTexture(TextureMap _MapName, TextureObject _TextureObject, bool _AutoUpdate = true)
        {
            if (_TextureObject == null)
            {
                Console.LogError($"Weapon Manager > The sent map is null!");
                return;
            }

            Console.LogSuccess($"Map \"{System.IO.Path.GetFileName(_TextureObject.TexturePath)}\" was successfuly received.");

            if (m_currentWeapon == null)
            {
                return;
            }

            m_currentWeapon.WeaponTextures.TextureObjects[SkinSystem.Environment.TextureMapToInt(_MapName)] = _TextureObject;

            if (_AutoUpdate)
            {
                Console.Log($"Updating the texture holder \"{_MapName}\"...");
                UpdateTextureMap(_MapName);
            }
        }

        public void SetPartsTexture(List<TextureObject[]> _TextureObjects, bool _AutoUpdate = true)
        {
            if (m_currentWeapon == null)
                return;

            for (int i = 0; i < _TextureObjects.Count; i++)
            {
                m_currentWeapon.WeaponSubRenderers[i].TextureData.TextureObjects = _TextureObjects[i];
            }

            if (_AutoUpdate)
                UpdateAllPartTextures();
        }

        public void RemoveTexture(TextureMap _TextureName)
        {
            if (m_currentWeapon == null)
                return;

            string _trueTextureName = Environment.GetTextureMapRealName(_TextureName);
            int _textureIndex = Environment.TextureMapToInt(_TextureName);
            m_currentWeapon.GetComponent<MeshRenderer>().material.SetTexture(_trueTextureName, m_currentWeapon.StartTextures[_textureIndex]);

            if (m_currentWeapon.HasMultipleParts)
            {
                for (int i = 0; i < m_currentWeapon.WeaponSubRenderers.Length; i++)
                {
                    m_currentWeapon.WeaponSubRenderers[i].TextureData.TextureObjects = m_currentWeapon.WeaponSubRenderers[i].StartTextureData.TextureObjects;
                }
            }

            InspectorManager.Instance.UpdateTextureHolder(_TextureName);
        }

        public void RemoveTexture(int index)
        {
            if (m_currentWeapon == null)
                return;

            TextureMap map = Environment.IntToTextureMap(index);
            string _trueTextureName = Environment.GetTextureMapRealName(map);
            m_currentWeapon.GetComponent<MeshRenderer>().material.SetTexture(_trueTextureName, m_currentWeapon.StartTextures[index]);

            if (m_currentWeapon.HasMultipleParts)
            {
                for (int i = 0; i < m_currentWeapon.WeaponSubRenderers.Length; i++)
                {
                    m_currentWeapon.WeaponSubRenderers[i].TextureData.TextureObjects = m_currentWeapon.WeaponSubRenderers[i].StartTextureData.TextureObjects;
                }
            }

            InspectorManager.Instance.UpdateTextureHolder(map);
        }
    }
}