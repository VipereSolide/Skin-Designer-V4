using UnityEngine;
using UnityEngine.EventSystems;
public class StandaloneInputModulePlus : StandaloneInputModule
{
    // based on solution provided by Numior: https://answers.unity.com/questions/1234594/how-do-i-find-which-object-is-eventsystemcurrentis.html
    public static StandaloneInputModulePlus instance;
    protected override void Awake()
    {
        instance = this;
        base.Awake();
    }
    protected override void OnDestroy()
    {
        instance = null;
        base.OnDestroy();
    }
    public GameObject GameObjectUnderPointer(int pointerId)
    {
        var lastPointer = GetLastPointerEventData(pointerId);
        if (lastPointer != null) return lastPointer.pointerCurrentRaycast.gameObject;
        return null;
    }
    public GameObject GameObjectUnderMouse()
    {
        return GameObjectUnderPointer(PointerInputModule.kMouseLeftId);
    }
    public GameObject GameObjectUnderTouch(Touch touch)
    {
        return GameObjectUnderPointer(touch.fingerId);
    }
}
