using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using SkinDesigner.SkinSystem;
using SkinDesigner.Textures;
using SkinDesigner;

using NaughtyAttributes;
using FeatherLight.Pro.Console;

namespace SkinDesigner.Weapon
{
    public class WeaponObject : MonoBehaviour
    {
        [Header("Behavior")]

        [SerializeField]
        private WeaponProfile mainProfile;

        [SerializeField]
        private SkinSystem.Weapon weaponType;

        [SerializeField]
        private bool disableOnStart = true;

        [Space]

        [SerializeField]
        private bool hasMultipleParts = false;

        [SerializeField]
        [ShowIf("hasMultipleParts")]
        private WeaponProfile[] weaponSubRenderers;


        private MeshRenderer m_renderer;
        private bool hasSetStartTextures = false;

        public WeaponProfile MainProfile
        {
            get
            {
                return mainProfile;
            }
        }
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
                return mainProfile.MaterialMetallicMode;
            }
        }
        public WeaponTextureData WeaponTextures
        {
            get
            {
                return mainProfile.Textures;
            }
        }
        public SkinSystem.Weapon Weapon
        {
            get
            {
                return weaponType;
            }
        }
        public WeaponProfile[] WeaponSubRenderers
        {
            get
            {
                return weaponSubRenderers;
            }
        }
        public bool HasMultipleParts
        {
            get
            {
                return hasMultipleParts;
            }
        }

        private void Awake()
        {
            m_renderer = GetComponent<MeshRenderer>();
        }
        public void Start()
        {
            mainProfile.Textures.UpdateStartTextures(mainProfile.Renderer);

            for (int i = 0; i < WeaponSubRenderers.Length; i++)
            {
                weaponSubRenderers[i].Textures.UpdateStartTextures(weaponSubRenderers[i].Renderer);
            }

            UpdateWeaponObject();

            if (disableOnStart)
            {
                gameObject.SetActive(false);
            }
        }

        private void SetWeaponProfileTexture(WeaponProfile profile, TextureMap map, ProjectWindowContentItem item, bool autoUpdate = true)
        {
            int index = Environment.TextureMapToInt(map);

            profile.Textures.IsUsingDefault[index] = false;
            profile.Textures.CustomTextures[index] = item;

            if (autoUpdate)
            {
                UpdateWeaponMap(map);
            }
        }
        private void RemoveWeaponProfileTexture(WeaponProfile profile, TextureMap map, bool autoUpdate = true)
        {
            int index = Environment.TextureMapToInt(map);

            profile.Textures.IsUsingDefault[index] = true;
            profile.Textures.CustomTextures[index] = null;

            if (autoUpdate)
            {
                UpdateWeaponMap(map);
            }
        }
        private void UpdateWeaponProfileRenderer(TextureMap map, WeaponProfile profile)
        {
            string mapName = Environment.GetTextureMapRealName(map);
            int mapIndex = Environment.TextureMapToInt(map);

            TextureObject textureObject = profile.Textures.DefaultTextureObjects[mapIndex];
            Texture usedTexture = textureObject.Texture;

            if (profile.Textures.IsUsingDefault[mapIndex])
            {
                if (usedTexture == null && textureObject.Texture != null)
                {
                    usedTexture = textureObject.GetTextureFromPath();
                }
            }
            else
            {
                textureObject = profile.Textures.CustomTextures[mapIndex].HeldTexture;

                if (textureObject.Texture == null)
                {
                    textureObject.GetTextureFromPath();
                }

                usedTexture = textureObject.Texture;
            }

            profile.Renderer.material.SetTexture(mapName, usedTexture);
        }

        public void Set(TextureMap map, ProjectWindowContentItem item, bool autoUpdate = true)
        {
            SetWeaponProfileTexture(mainProfile, map, item, autoUpdate);
        }
        public void Remove(TextureMap map, bool autoUpdate = true)
        {
            RemoveWeaponProfileTexture(mainProfile, map, autoUpdate);
        }
        public void SetPart(int partIndex, TextureMap map, ProjectWindowContentItem item, bool autoUpdate = true)
        {
            SetWeaponProfileTexture(weaponSubRenderers[partIndex], map, item, autoUpdate);
        }
        public void RemovePart(int partIndex, TextureMap map, bool autoUpdate = true)
        {
            RemoveWeaponProfileTexture(weaponSubRenderers[partIndex], map, autoUpdate);
        }

        public void UpdateWeaponMap(TextureMap map)
        {
            UpdateWeaponProfileRenderer(map, mainProfile);
        }
        public void UpdatePartsMap(TextureMap map, int partIndex)
        {
            UpdateWeaponProfileRenderer(map, weaponSubRenderers[partIndex]);
        }
        public void UpdateWeaponObject()
        {
            for(int i = 0; i < 7; i++)
            {
                UpdateWeaponMap(Environment.IntToTextureMap(i));
            }
        }


        [System.Serializable]
        public class WeaponProfile
        {
            [Header("Profile")]

            [SerializeField]
            private string name;

            [SerializeField]
            private MaterialMetallicMode materialMetallicMode;

            [SerializeField]
            private WeaponTextureData weaponTextures;

            [Header("References")]

            [SerializeField]
            private MeshRenderer renderer;

            public string Name
            {
                get
                {
                    return name;
                }
            }
            public MeshRenderer Renderer
            {
                get
                {
                    return renderer;
                }
            }
            public MaterialMetallicMode MaterialMetallicMode
            {
                get
                {
                    return materialMetallicMode;
                }
            }
            public WeaponTextureData Textures
            {
                get
                {
                    return weaponTextures;
                }
            }

            public WeaponProfile(MeshRenderer renderer, WeaponTextureData weaponTextures, MaterialMetallicMode metallicMode = MaterialMetallicMode.M)
            {
                this.renderer = renderer;
                this.weaponTextures = weaponTextures;
                materialMetallicMode = metallicMode;
            }
        }

        [System.Serializable]
        public class WeaponTextureData
        {
            [SerializeField]
            private bool[] isUsingDefault = new bool[7] { true , true , true , true , true , true , true };

            [SerializeField]
            private ProjectWindowContentItem[] customTextures;

            [SerializeField]
            private TextureObject[] defaultTextureObjects;

            public bool[] IsUsingDefault
            {
                get
                {
                    return isUsingDefault;
                }
                set
                {
                    isUsingDefault = value;
                }
            }
            public TextureObject[] DefaultTextureObjects
            {
                get
                {
                    return defaultTextureObjects;
                }

                set
                {
                    defaultTextureObjects = value;
                }
            }
            public ProjectWindowContentItem[] CustomTextures
            {
                get
                {
                    return customTextures;
                }

                set
                {
                    customTextures = value;
                }
            }

            public WeaponTextureData()
            {
                defaultTextureObjects = new TextureObject[7];
                customTextures = new ProjectWindowContentItem[7];
            }
            public WeaponTextureData(TextureObject[] startTextures)
            {
                defaultTextureObjects = startTextures;
                customTextures = new ProjectWindowContentItem[7];
            }
            public WeaponTextureData(TextureObject[] startTextures, ProjectWindowContentItem[] items)
            {
                defaultTextureObjects = startTextures;
                customTextures = items;
            }

            public void UpdateStartTextures(MeshRenderer renderer)
            {
                defaultTextureObjects = new TextureObject[7];

                for (int i = 0; i < 7; i++)
                {
                    defaultTextureObjects[i] = new TextureObject();
                    defaultTextureObjects[i].Texture = renderer.sharedMaterial.GetTexture(Environment.GetTextureMapRealName(Environment.IntToTextureMap(i)));
                }
            }
        }
    }
}