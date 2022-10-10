using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;

namespace SkinDesigner.Painter.UI
{
    public class MaskDropdownItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private WeaponMaskPreviewManager weaponMaskPreviewManager;
        [SerializeField] private string maskName = "";
        [SerializeField] private bool autoAssignMaskName = true;

        private bool hover = false;

        private void Start()
        {
            if (autoAssignMaskName)
            {
                maskName = transform.name.Split(':')[1].Replace(" M","M");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hover = true;

            weaponMaskPreviewManager.ActivateMask(maskName);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hover = false;

            weaponMaskPreviewManager.DisableAllMasks();
        }
    }
}