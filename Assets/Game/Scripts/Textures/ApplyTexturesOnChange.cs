using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using SkinDesigner.SkinSystem;
using SkinDesigner.Inspector;
using SkinDesigner.Weapon;

using FeatherLight.Pro.Console;
using FeatherLight.Pro;

namespace SkinDesigner.Textures
{
    public class ApplyTexturesOnChange : MonoBehaviour
    {
        [SerializeField] private InspectorHolder[] inspectorHolders;

        private void Start()
        {
            // Attach a texture change event to every inspector holders.
            // This way, the weapon texture will update as the inspector holders
            // get a new texture.
            for (int i = 0; i < inspectorHolders.Length; i++)
            {
                inspectorHolders[i].onTextureChanged.AddListener((TextureObject t) => SetTextureByHolderIndex(t, Environment.IntToTextureMap(i), i));
            }
        }

        private void OnApplicationQuit()
        {
            // Detach the start event.
            for (int i = 0; i < inspectorHolders.Length; i++)
            {
                inspectorHolders[i].onTextureChanged.RemoveListener((TextureObject t) => SetTextureByHolderIndex(t, Environment.IntToTextureMap(i), i));
            }
        }

        /// <summary>
        /// Sets the correct map of the WeaponManager to the a new texture, based
        /// on holder system.
        /// </summary>
        /// <param name="texture">The new texture for this map.</param>
        /// <param name="holderIndex">Used to get the correct map.</param>
        private void SetTextureByHolderIndex(TextureObject texture, TextureMap map, int holderIndex)
        {
            // If the texture is null, remove the current map as well.
            if (texture == null)
            {
                WeaponManager.Instance.CurrentWeapon.Remove(map);
                return;
            }

            // Updates the weapon manager's texture for the current weapon.
            WeaponManager.Instance.CurrentWeapon.Set(map, inspectorHolders[holderIndex].ContainedItem);
        }
    }
}