using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkinDesigner
{
    [System.Serializable]
    public class KeyShortcut
    {
        public enum ControlKeys
        {
            [InspectorName("Disabled")]None,
            [InspectorName("Left Control")]LeftCtrl,
            [InspectorName("Left Alt")]LeftAlt,
            [InspectorName("Left Shift")] LeftShift
        }

        [SerializeField] private ControlKeys useControlKey = ControlKeys.None;
        [SerializeField] private ControlKeys useSecondControlKey = ControlKeys.None;
        [SerializeField] private KeyCode useFirstKey = KeyCode.None;
        [SerializeField] private KeyCode useSecondKey = KeyCode.None;

        public ControlKeys FirstControlKey => useControlKey;
        public ControlKeys SecondControlKey => useSecondControlKey;
        public KeyCode FirstKey => useFirstKey;
        public KeyCode SecondKey => useSecondKey;

        public static KeyCode ControlKeyToKeyCode(ControlKeys key)
        {
            switch (key)
            {
                case ControlKeys.LeftCtrl:
                    return KeyCode.LeftControl;
                case ControlKeys.LeftAlt:
                    return KeyCode.LeftAlt;
                case ControlKeys.LeftShift:
                    return KeyCode.LeftShift;
                case ControlKeys.None:
                    return KeyCode.None;
                default:
                    return KeyCode.None;
            }
        }

        public bool IsPressed()
        {
            bool output = false;
            bool ctrlk1 = (useControlKey != ControlKeys.None);
            bool ctrlk2 = (useSecondControlKey != ControlKeys.None);
            bool key2 = (useSecondKey != KeyCode.None);
            
            bool pressingCtrlK1 = Input.GetKey(ControlKeyToKeyCode(useControlKey));
            bool pressingCtrlK2 = Input.GetKey(ControlKeyToKeyCode(useSecondControlKey));
            bool pressingK1 = Input.GetKeyDown(useFirstKey);
            bool pressingK2 = Input.GetKeyDown(useSecondKey);

            if (ctrlk1)
            {
                output = pressingCtrlK1;
                if (!output) return false;
            }

            if (ctrlk2)
            {
                output = pressingCtrlK2;
                if (!output) return false;
            }

            output = pressingK1;
            if (!output) return false;

            if (key2)
            {
                output = pressingK2;
                if (!output) return false;
            }

            return output;
        }
    }
}