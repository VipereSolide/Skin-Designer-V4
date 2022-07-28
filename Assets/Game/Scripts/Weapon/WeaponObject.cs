using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkinDesigner.SkinSystem;
using SkinDesigner.Textures;

namespace SkinDesigner.Weapon
{
    public class WeaponObject : MonoBehaviour
    {
        [SerializeField]
        private WeaponTextureData m_weaponTextures;

        [SerializeField]
        private SkinSystem.MaterialMetallicMode m_materialMetallicMode;

        [SerializeField]
        private SkinDesigner.SkinSystem.Weapon m_weapon;

        [SerializeField]
        private bool m_hasMultipleParts = false;

        [SerializeField]
        private WeaponSubRenderer[] m_weaponSubRenderers;

        [SerializeField]
        private bool m_disableOnStart = true;

        [SerializeField]
        private Texture2D[] m_startTextures = new Texture2D[7];

        private MeshRenderer m_renderer;

        private bool hasSetStartTextures = false;

        public MeshRenderer Renderer
        {
            get
            {
                return m_renderer;
            }
        }

        public MaterialMetallicMode MaterialMetallicMode
        {
            get
            {
                return m_materialMetallicMode;
            }
        }

        public WeaponTextureData WeaponTextures
        {
            get
            {
                return m_weaponTextures;
            }
        }

        public SkinDesigner.SkinSystem.Weapon Weapon
        {
            get
            {
                return m_weapon;
            }
        }

        public WeaponSubRenderer[] WeaponSubRenderers
        {
            get
            {
                return m_weaponSubRenderers;
            }
        }

        public bool HasMultipleParts
        {
            get
            {
                return m_hasMultipleParts;
            }
        }

        public Texture2D[] StartTextures
        {
            get
            {
                return m_startTextures;
            }
        }

        private void Awake()
        {
            m_renderer = GetComponent<MeshRenderer>();
        }

        public void Start()
        {
            if (!hasSetStartTextures)
            {
                m_startTextures = Environment.GetMaterialTextures(m_renderer.material);

                if (m_hasMultipleParts)
                {
                    for (int i = 0; i < m_weaponSubRenderers.Length; i++)
                    {
                        // Updates the start textures of the skin.
                        Texture2D[] _rendererMaterialTextures = SkinSystem.Environment.GetMaterialTextures(m_weaponSubRenderers[i].Renderer.material);
                        m_weaponSubRenderers[i].StartTextureData = new WeaponTextureData(new TextureObject[7]
                        {
                        new TextureObject(_rendererMaterialTextures[0]),
                        new TextureObject(_rendererMaterialTextures[1]),
                        new TextureObject(_rendererMaterialTextures[2]),
                        new TextureObject(_rendererMaterialTextures[3]),
                        new TextureObject(_rendererMaterialTextures[4]),
                        new TextureObject(_rendererMaterialTextures[5]),
                        new TextureObject(_rendererMaterialTextures[6])
                        });
                    }
                }
                
                hasSetStartTextures = true;
            }

            if (m_disableOnStart)
                gameObject.SetActive(false);
        }

        public void SetTextures(Texture2D[] textures)
        {
            Environment.SetMaterialTextures(Renderer.material, textures);
        }

        public void SetTextures(TextureObject[] textures)
        {
            List<Texture2D> textureList = new List<Texture2D>();

            for (int i = 0; i < textures.Length; i++)
            {
                this.m_weaponTextures.TextureObjects[i] = textures[i];
                TextureObject obj = textures[i];

                if (obj.TexturePath == "NULL")
                {
                    textureList.Add(m_startTextures[i]);
                }
                else
                {
                    textureList.Add((Texture2D)obj.GetTextureFromPath());
                }
            }

            Environment.SetMaterialTextures(Renderer.material, textureList.ToArray());
        }

        [System.Serializable]
        public class WeaponSubRenderer
        {
            [SerializeField]
            private MeshRenderer m_renderer;

            [SerializeField]
            private SkinSystem.MaterialMetallicMode m_materialMetallicMode;

            [SerializeField]
            private WeaponTextureData m_textureData;

            [SerializeField]
            private WeaponTextureData m_startTextureData;

            public MeshRenderer Renderer
            {
                get
                {
                    return m_renderer;
                }
            }

            public SkinSystem.MaterialMetallicMode MaterialMetallicMode
            {
                get
                {
                    return m_materialMetallicMode;
                }
            }

            public WeaponTextureData TextureData
            {
                get
                {
                    return m_textureData;
                }
            }

            public WeaponTextureData StartTextureData
            {
                get
                {
                    return m_startTextureData;
                }

                set
                {
                    this.m_startTextureData = value;
                }
            }

            public WeaponSubRenderer(MeshRenderer _Renderer, WeaponTextureData _TextureData, SkinSystem.MaterialMetallicMode _MetallicMode = SkinSystem.MaterialMetallicMode.M)
            {
                this.m_renderer = _Renderer;
                this.m_textureData = _TextureData;
                this.m_materialMetallicMode = _MetallicMode;
            }
        }

        [System.Serializable]
        public class WeaponTextureData
        {
            [SerializeField]
            private TextureObject[] m_textureObjects;

            public TextureObject[] TextureObjects
            {
                get
                {
                    return m_textureObjects;
                }

                set
                {
                    m_textureObjects = value;
                }
            }

            public WeaponTextureData()
            {
                this.m_textureObjects = new TextureObject[7];
            }

            public WeaponTextureData(TextureObject[] _TextureObjects)
            {
                this.m_textureObjects = _TextureObjects;
            }
        }
    }
}