using UnityEngine;

namespace SkinDesigner.Inspector
{
    public class Category : MonoBehaviour
    {
        [SerializeField] private Transform activeArrow;
        [SerializeField] private GameObject[] categoryItems;
        private bool isActive = true;

        public GameObject[] CategoryItems
        {
            get { return categoryItems; }
        }

        public bool IsActive
        {
            get { return isActive; }
        }

        public void SetActive(bool value)
        {
            foreach (GameObject obj in categoryItems)
            {
                obj.SetActive(value);
            }

            activeArrow.localEulerAngles = new Vector3(0, 0, (value) ? 0 : 90);
            isActive = value;
        }

        public void ToggleActive()
        {
            SetActive(!isActive);
        }
    }
}