using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkinDesigner.Weapon;
using SkinDesigner.SkinSystem;

namespace SkinDesigner.Textures
{
    public class ApplyTexturesOnChange : MonoBehaviour
    {
        [SerializeField] private textureHolder m_albedo;
        [SerializeField] private textureHolder m_detail;
        [SerializeField] private textureHolder m_emission;
        [SerializeField] private textureHolder m_height;
        [SerializeField] private textureHolder m_metallic;
        [SerializeField] private textureHolder m_normal;
        [SerializeField] private textureHolder m_occlusion;

        private void Start()
        {
            m_albedo.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Albedo, m_albedo.Texture, true));
            m_detail.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Detail, m_detail.Texture, true));
            m_emission.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Emission, m_emission.Texture, true));
            m_height.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Height, m_height.Texture, true));
            m_metallic.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Metallic, m_metallic.Texture, true));
            m_normal.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Normal, m_normal.Texture, true));
            m_occlusion.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Occlusion, m_occlusion.Texture, true));
        }

    }
}