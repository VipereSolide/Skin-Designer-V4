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
            m_albedo.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Albedo, new TextureObject(m_albedo.Texture, m_albedo.TexturePath), true));
            m_detail.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Detail, new TextureObject(m_detail.Texture, m_detail.TexturePath), true));
            m_emission.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Emission, new TextureObject(m_emission.Texture, m_emission.TexturePath), true));
            m_height.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Height, new TextureObject(m_height.Texture, m_height.TexturePath), true));
            m_metallic.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Metallic, new TextureObject(m_metallic.Texture, m_metallic.TexturePath), true));
            m_normal.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Normal, new TextureObject(m_normal.Texture, m_normal.TexturePath), true));
            m_occlusion.onSelectTexture.AddListener(() => WeaponManager.Instance.SetTexture(TextureMap.Occlusion, new TextureObject(m_occlusion .Texture, m_occlusion.TexturePath), true));
        }

    }
}