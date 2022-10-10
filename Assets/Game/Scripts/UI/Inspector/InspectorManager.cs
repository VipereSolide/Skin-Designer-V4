using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using SkinDesigner.SelectTexturesWindow;
using SkinDesigner.SkinSystem;
using SkinDesigner.Textures;
using SkinDesigner.Weapon;

using FeatherLight.Pro.Console;
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
        private CanvasGroup inspectorContent;

        private WeaponObject currentObject;
        private int waitingHolder;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            CanvasGroupHelper.SetActive(inspectorContent, false);
        }

        public void SetInspectedWeapon(SkinSystem.Weapon weapon)
        {
            WeaponManager manager = WeaponManager.Instance;

            manager.InstantiateWeapon(weapon);
            currentObject = manager.GetWeaponByWeaponType(weapon);
            CanvasGroupHelper.SetActive(inspectorContent, true);

            ClearTextureHolders();
            UpdateAllTextureHolders();
        }
        public void ClearTextureHolders()
        {
            foreach(var holder in holders)
            {
                holder.TrashTexture();
            }
        }
        public void UpdateTextureHolder(TextureMap textureMap)
        {
            if (currentObject == null)
            {
                Console.LogError("Inspector > Couldn't Update the texture holders as there is no currentObject assigned.");
                return;
            }

            int index = Environment.TextureMapToInt(textureMap);
            TextureObject correspondingMapTexture = WeaponManager.Instance.CurrentWeapon.WeaponTextures.TextureObjects[index];

            if (correspondingMapTexture == null)
            {
                Console.LogWarning("Inspector > The corresponding texture object was null.");
                return;
            }

            holders[index].SetHeldTexture(correspondingMapTexture.TexturePath);
        }
        public void UpdateAllTextureHolders()
        {
            if (currentObject == null)
            {
                Console.LogError("Inspector > Couldn't Update the texture holders as there is no currentObject assigned.");
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
        public void SetTextureByMediaItem()
        {
            TextureMap map = Environment.IntToTextureMap(waitingHolder);
            WeaponManager manager = WeaponManager.Instance;

            Texture texture = selectTexturesWindow.Texture;
            string textureLink = selectTexturesWindow.CurrentTextureLink;

            if (texture == null || selectTexturesWindow.CurrentTextureLink == "NULL")
            {
                Console.Log($"Inspector > Removing texture map {map}...");

                try
                {
                    Console.Log($"Inspector > Successfuly removed texture map {map}!");
                    manager.RemoveTexture(map);
                }
                catch (System.Exception e)
                {
                    Console.LogError($"Inspector > Couldn't remove texture map {map}!");
                    Console.LogError($"Inspector > {e}");
                }
            }
            else
            {
                Console.Log($"Inspector > Setting used texture by sending it to the WeaponManager...\nFile Path: {textureLink}.\nTexture Size: {((Texture2D)texture).EncodeToPNG().Length}");
                
                try
                {
                    Console.Log($"Inspector > Texture was successfuly sent to the WeaponManager!\nWaiting for WeaponManager receive message...");
                    manager.SetTexture(map, new TextureObject(selectTexturesWindow.Texture, textureLink));
                }
                catch(System.Exception e)
                {
                    Console.LogError($"Inspector > Couldn't send texture to Weapon Manager!");
                    Console.LogError($"Inspector > {e}");
                }
            }

            UpdateAllTextureHolders();

            waitingHolder = 0;
        }
    }
}