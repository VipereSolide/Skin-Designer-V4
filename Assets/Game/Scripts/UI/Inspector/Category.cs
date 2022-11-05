using UnityEngine.EventSystems;
using UnityEngine;

namespace SkinDesigner.Inspector
{
    public class Category : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform activeArrow;
        [SerializeField] private GameObject[] categoryItems;
        [SerializeField] private bool isActive = true;

        public GameObject[] CategoryItems
        {
            get { return categoryItems; }
        }

        public bool IsActive
        {
            get { return isActive; }
        }

        protected virtual void Start()
        {
            UpdateActiveState();
        }

        public virtual void SetActive(bool value, bool autoUpdate = true)
        {
            isActive = value;

            if (autoUpdate)
            {
                UpdateActiveState();
            }
        }
        public virtual void ToggleActive(bool autoUpdate = true)
        {
            SetActive(!isActive);

            if (autoUpdate)
            {
                UpdateActiveState();
            }
        }

        public virtual void UpdateActiveState()
        {
            foreach (GameObject item in categoryItems)
            {
                item.SetActive(isActive);
            }

            if (isActive)
            {
                activeArrow.localEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                activeArrow.localEulerAngles = new Vector3(0, 0, 90);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ToggleActive();
        }
    }
}