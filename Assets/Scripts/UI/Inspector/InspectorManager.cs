using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using SkinDesigner.SkinSystem;
using SkinDesigner.Weapon;
using FeatherLight.Pro;
using SkinDesigner;

namespace SkinDesigner.Inspector
{
    public class InspectorManager : MonoBehaviour
    {
        public static InspectorManager Instance;

        [SerializeField]
        private textureHolder[] holders;

        [SerializeField]
        private colorPickerObject[] colorPickers;

        private WeaponObject currentObject;

        private void Awake()
        {
            Instance = this;
        }

        public void SetInspectedWeapon(SkinSystem.Weapon weapon)
        {
            Weapon.WeaponManager manager = Weapon.WeaponManager.Instance;

            manager.InstantiateWeapon(weapon);
            currentObject = manager.GetWeaponByWeaponType(weapon);

            UpdateAllTextureHolders();
        }

        public void ResetTextureHolder(TextureMap textureMap)
        {

        }

        public void ResetAllTextureHolders()
        {
            
        }

        public void SetTextureHolder(TextureMap textureMap, Texture texture)
        {

        }

        public void SetAllTextureHolders(Texture texture) {}

        public void UpdateTextureHolder(TextureMap textureMap)
        {
            if (currentObject == null)
            {
                Debug.LogError("Couldn't Update the texture holders as there is no currentObject assigned.");
                return;
            }

            MeshRenderer renderer = currentObject.Renderer;
            Material material = renderer.material;

            int index = Environment.TextureMapToInt(textureMap);

            Texture2D texture = new Texture2D(2,2);
            texture = (Texture2D)material.GetTexture(Environment.GetTextureMapRealName(textureMap));
            texture = TextureHelper.Scaled(texture, 64, 64);
            texture.Apply();

            holders[index].SetHeldTexture(texture);
        }

        public void UpdateAllTextureHolders()
        {
            if (currentObject == null)
            {
                Debug.LogError("Couldn't Update the texture holders as there is no currentObject assigned.");
                return;
            }

            for(int i = 0; i < 7; i++)
            {
                TextureMap map = Environment.IntToTextureMap(i);
                UpdateTextureHolder(map);
            }
        }
        
    }
}