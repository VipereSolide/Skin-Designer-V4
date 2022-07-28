using UnityEngine.UI;
using UnityEngine;

namespace SkinDesigner.Settings
{
    public class SettingsItemOptionCheckbox : SettingsItemOption
    {
        [SerializeField] private Toggle optionToggle;

        public Toggle OptionToggle
        {
            get { return optionToggle; }
            set { optionToggle = value; }
        }

        private void Start()
        {
            this.valueType = "bool";
        }

        private void Update()
        {
            this.value = optionToggle.isOn;
        }
        
        protected override void SetValue(object newValue)
        {
            optionToggle.isOn = (bool)newValue;
            optionToggle.onValueChanged.Invoke(optionToggle.isOn);
        }
    }
}