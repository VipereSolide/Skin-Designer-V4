using System.Collections.Generic;
using System.Collections;

using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

using SkinDesigner.SelectTexturesWindow;
using SkinDesigner.SkinSystem;
using SkinDesigner.Textures;
using SkinDesigner.Weapon;

using FeatherLight.Pro.Console;
using FeatherLight.Pro;

namespace SkinDesigner.Inspector
{
    public class InspectorHolder : MonoBehaviour
    {
        [Header("References")]

        [SerializeField]
        private RawImage holderImage;

        [Header("Holder")]

        [SerializeField] [Tooltip("Describes whether the texture object will be automatically updated or not when checking if it has changed.")]
        private bool autoUpdateTexture = true;

        [Space] // Putting a space there because the UnityEvents are **HUGE**
                // and it doesn't look very good otherwise...
        [SerializeField]
        private UnityEvent<TextureObject> onTextureChanged = null;

        [Header("Debugging")]
        
        [SerializeField]
        private ProjectWindowContentItem containedItem;

        private Texture current;

        private void Update()
        {
            ManageTextureUpdate();
        }

        /// <summary>
        /// Used to get what texture to use.
        /// </summary>
        public ProjectWindowContentItem ContainedItem
        {
            get { return containedItem; }
        }
        
        /// <summary>
        /// Sets the contained item used by the inspector holder.
        /// </summary>
        /// <param name="item">The new contained item.</param>
        public void Set(ProjectWindowContentItem item, bool autoUpdate = true)
        {
            containedItem = item;

            if (autoUpdate)
            {
                UpdateItem();
            }
        }
        /// <summary>
        /// Removes or trashes the current contained item and texture to restore the normal one.
        /// </summary>
        public void Remove(bool autoUpdate = true)
        {
            containedItem = null;

            if (autoUpdate)
            {
                UpdateItem();
            }
        }
        /// <summary>
        /// Updates the visuals of the inspector holder.
        /// </summary>
        public void UpdateItem()
        {
            // If there is no item assigned, return.
            if (containedItem == null)
            {
                // Removes the texture held by the holder image.
                holderImage.texture = null;

                return;
            }

            // If there is no texture loaded yet, load it.
            if (containedItem.HeldTexture.Texture == null)
            {
                // If texture is null, stop updating the item.
                if (string.IsNullOrEmpty(containedItem.HeldTexture.TexturePath) || containedItem.HeldTexture.TexturePath == "NULL")
                {
                    Console.LogError($"Inspector Holder > The contained item's path (\"{containedItem.HeldTexture.TexturePath}\") is not a valid file path.");
                    return;
                }

                containedItem.HeldTexture.GetTextureFromPath();
            }

            // Fetch the texture.
            current = containedItem.HeldTexture.Texture;
            // Assign the texture to the holder background.
            holderImage.texture = current;
        }

        /// <summary>
        /// Updates the held image if it's texture has changed.
        /// </summary>
        private void ManageTextureUpdate()
        {
            // If we don't hold any item, return.
            if (containedItem == null)
            {
                return;
            }

            // We don't need to execute any logic if there is no current
            // texture either.
            if (current == null)
            {
                return;
            }

            // Describes whether the bytes of the contained texture have changed or not.
            // The autoUpdateTexture bool tells the method whether it should update the
            // FileInfo to register the change.
            bool hasChanged = containedItem.HeldTexture.HasTextureFileChanged(autoUpdateTexture);

            // If the texture has changed so all the graphics match, and update the item and invoke
            // the onTextureChanged event.
            if (hasChanged)
            {
                UpdateItem();
                onTextureChanged.Invoke(containedItem.HeldTexture);
            }
        }
    }
}