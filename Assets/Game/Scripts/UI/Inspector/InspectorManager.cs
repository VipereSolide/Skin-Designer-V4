using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using SkinDesigner.SelectTexturesWindow;
using SkinDesigner.SkinSystem;
using SkinDesigner.Textures;
using SkinDesigner.Weapon;
using FeatherLight.Pro;

namespace SkinDesigner.Inspector
{
    public class InspectorManager : MonoBehaviour
    {
        public static InspectorManager Instance;

        [SerializeField]
        private TexturesWindowManager selectTexturesWindow;

        [SerializeField]
        private textureHolder[] holders;

        [SerializeField]
        private colorPickerObject[] colorPickers;

        private WeaponObject currentObject;
        private int waitingHolder;

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

        public virtual void ResetTextureHolder(TextureMap textureMap)
        {

        }

        public virtual void ResetAllTextureHolders()
        {

        }

        public virtual void SetTextureHolder(TextureMap textureMap, Texture texture)
        {

        }

        public virtual void SetAllTextureHolders(Texture texture)
        {

        }

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

            Texture2D texture = new Texture2D(2, 2);
            texture = (Texture2D)material.GetTexture(Environment.GetTextureMapRealName(textureMap));

            if (texture != null)
            {
                texture = TextureHelper.Scaled(texture, 64, 64);
                texture.Apply();
            }

            holders[index].SetHeldTexture(texture);
        }

        public void UpdateAllTextureHolders()
        {
            if (currentObject == null)
            {
                Debug.LogError("Couldn't Update the texture holders as there is no currentObject assigned.");
                return;
            }

            for (int i = 0; i < 7; i++)
            {
                TextureMap map = Environment.IntToTextureMap(i);
                UpdateTextureHolder(map);
            }
        }

        public void SetTextureViaTextureWindow(int holderIndex)
        {
            waitingHolder = holderIndex;

            selectTexturesWindow.Call(holders[holderIndex]);
        }

        [HideInInspector]
        public void SetTextureByMediaItem()
        {
            TextureMap map = SkinDesigner.SkinSystem.Environment.IntToTextureMap(waitingHolder);
            WeaponManager manager = WeaponManager.Instance;

            Texture texture = selectTexturesWindow.Texture;
            string textureLink = selectTexturesWindow.CurrentTextureLink;

            if (texture == null || selectTexturesWindow.CurrentTextureLink == "NULL")
            {
                manager.RemoveTexture(map);
            }
            else
            {
                manager.SetTexture(map, new TextureObject(selectTexturesWindow.Texture, textureLink));
            }

            UpdateAllTextureHolders();

            waitingHolder = 0;
        }
    }
}