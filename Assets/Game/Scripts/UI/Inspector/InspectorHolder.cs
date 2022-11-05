using System.Collections.Generic;
using System.Collections;

using UnityEngine.EventSystems;
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
    public class InspectorHolder : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [Header("References")]

        [SerializeField]
        private RawImage holderImage;

        [SerializeField]
        private GameObject draggableFieldPreview;

        [SerializeField]
        private GameObject removeTextureIcon;

        [Header("Holder")]

        [SerializeField] [Tooltip("To which texture is this holder assigned to.")]
        private TextureMap holderMap;

        [SerializeField] [Tooltip("Describes whether the texture object will be automatically updated or not when checking if it has changed.")]
        private bool autoUpdateTexture = true;

        [Space] // Putting a space there because the UnityEvents are **HUGE**
                // and it doesn't look very good otherwise...
        [SerializeField]
        public UnityEvent<TextureObject> onTextureChanged = null;

        [Header("Debugging")]

        [SerializeField]
        private ProjectWindowContentItem containedItem;

        [SerializeField] [Tooltip("Describes whether the currently dragged item(s) can be dropped inside this field or not.")]
        private bool canBeDropped;

        [SerializeField] [Tooltip("Whether the mouse is hovering the holder or not.")]
        private bool isHighlighted;

        private ProjectWindowContentItem lastContainedItem;
        private Texture current;

        private void Start()
        {
            ProjectWindowManager.Instance.onStartDragging.AddListener(OnStartDragging);
            ProjectWindowManager.Instance.onStopDragging.AddListener(OnStopDragging);

            draggableFieldPreview.SetActive(false);
        }

        private void OnApplicationQuit()
        {
            ProjectWindowManager.Instance.onStartDragging.RemoveListener(OnStartDragging);
            ProjectWindowManager.Instance.onStopDragging.RemoveListener(OnStopDragging);
        }

        private void Update()
        {
            ManageTextureUpdate();

            if (containedItem == null && lastContainedItem != null)
            {
                Remove();
            }
            lastContainedItem = containedItem;
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

        private void OnStartDragging(ProjectWindowContentItem[] dragging)
        {
            // The dragged items can be dropped inside an InspectorHolder only
            // if the last item is a texture.
            ProjectWindowContentItem lastDragging = dragging[dragging.Length - 1];
            if (lastDragging.GetType() == typeof(ProjectWindowContentFolder) || lastDragging.GetType() == typeof(ProjectWindowContentWeapon))
            {
                // Make sure the canBeDropped bool is disabled so OnStopDragging
                // won't accept it as a droppable item.
                canBeDropped = false;
                
                return;
            }

            // Otherwise, activate the canBeDropped bool, so OnStopDragging knows it
            // can accept this item.
            canBeDropped = true;

            // When the user starts dragging an item, activate the draggable
            // field preview object to show to the user that he can drop his texture
            // here.
            draggableFieldPreview.SetActive(true);
        }
        private void OnStopDragging(ProjectWindowContentItem[] dragging)
        {
            // When user drops item, if the draggable field preview object
            // was enabled, disable it.
            if (draggableFieldPreview.activeSelf)
            {
                draggableFieldPreview.SetActive(false);
            }

            // If the currently dragging item cannot be dropped
            // inside an InspectorHolder, we don't want to execute
            // any more logic.
            if (!canBeDropped)
            {
                return;
            }
            
            // If the user is not hovering the inspector holder, we
            // don't want to assign the texture.
            if (!isHighlighted)
            {
                return;
            }

            // If it's the case, set the last dragging item as the contained item,
            // and update the InpectorHolder.
            ProjectWindowContentItem lastDragging = dragging[dragging.Length - 1];

            // Make sure the last dragging item is not null.
            if (lastDragging == null)
            {
                Console.LogError($"Inspector Holder > The last dragging item was null!");
                return;
            }
            else
            {
                Set(lastDragging);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Updates the highlighted flag of the holder.
            isHighlighted = false;

            // Disable the trash icon one the user stops hovering over the holder,
            // and if it's not null.
            if (removeTextureIcon != null)
            {
                removeTextureIcon.gameObject.SetActive(false);
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Updates the highlighted flag of the holder.
            isHighlighted = true;
           

            // Activate the trash icon if it's not null, when the user
            // hovers over the holder and if the current texture isn't null.
            if (containedItem != null && removeTextureIcon != null)
            {
                removeTextureIcon.gameObject.SetActive(true);
            }
        }

        #region Public
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

                // If the texture is null, simply remove the current texture for this map.
                WeaponManager.Instance.CurrentWeapon.Remove(holderMap);

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

            // Set the corresponding texture to the WeaponManager.
            WeaponManager.Instance.CurrentWeapon.Set(holderMap, containedItem);
        }
        #endregion
    }
}