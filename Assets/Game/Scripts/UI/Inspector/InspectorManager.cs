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
        private InspectorHolder[] inspectorHolders;

        [SerializeField]
        private CanvasGroup inspectorContent;

        private int waitingHolder;

        private WeaponManager weaponManager;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            weaponManager = WeaponManager.Instance;
            CanvasGroupHelper.SetActive(inspectorContent, false);
        }

        public void SetInspectedWeapon(SkinSystem.Weapon weapon)
        {

            weaponManager.InstantiateWeapon(weapon);
            CanvasGroupHelper.SetActive(inspectorContent, true);

            ClearTextureHolders();
            UpdateAllTextureHolders();
        }
        public void ClearTextureHolders()
        {
            foreach(var holder in inspectorHolders)
            {
                holder.Remove();
            }
        }
        public void UpdateTextureHolder(TextureMap textureMap)
        {
            if (weaponManager.CurrentWeapon == null)
            {
                Console.LogError("Inspector > Couldn't Update the texture holders as there is no weapon currently inspected.");
                return;
            }

            WeaponManager.Instance.CurrentWeapon.UpdateWeaponMap(textureMap);
        }

        public void UpdateAllTextureHolders()
        {
            if (weaponManager.CurrentWeapon == null)
            {
                Console.LogError("Inspector > Couldn't Update the texture holders as there is no weapon currently inspected.");
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

            selectTexturesWindow.Call(inspectorHolders[holderIndex]);
        }
    }
}