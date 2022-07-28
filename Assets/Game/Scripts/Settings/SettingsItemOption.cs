using UnityEngine.Events;
using UnityEngine;

namespace SkinDesigner.Settings
{
    public class SettingsItemOption : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent onChangeOption;

        [HideInInspector]
        public object Value
        {
            get { return this.value; }
            set { this.value = value; SetValue(value); }
        }

        protected string valueType;
        protected object value;

        public string ValueType
        {
            get { return valueType; }
        }

        protected virtual void SetValue(object newValue) {}
    }
}