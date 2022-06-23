using UnityEngine;

namespace FeatherLight.Pro
{
    public static class ObjectHelper
    {
        public static GameObject FindObjectFromInstanceID(int id, Transform[] searchIn = null)
        {
            Transform[] objects = null;
            Transform output = null;

            if (searchIn == null)
            {
                objects = GameObject.FindObjectsOfType<Transform>();
            }
            else
            {
                objects = searchIn;
            }

            for (int i = 0; i < objects.Length; i++)
            {
                Debug.Log(objects[i].gameObject.GetInstanceID() + " == " + id + " => " + (objects[i].gameObject.GetInstanceID() == id));
                if (objects[i].gameObject.GetInstanceID() == id)
                {
                    output = objects[i];
                    break;
                }
            }

            return output.gameObject;
        }
    }
}