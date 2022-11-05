using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighPriorityParent : MonoBehaviour
{
    [SerializeField]
    protected Transform highPriorityParent;

    protected bool isHighPriority = false;
    protected Transform previousParent;

    public bool IsHighPriority { get { return isHighPriority; } set { SetHighPriority(value); } }

    public virtual void SetHighPriority(bool value)
    {
        if (value)
        {
            previousParent = transform.parent;
            transform.SetParent(highPriorityParent, true);
        }
        else
        {
            transform.SetParent(previousParent, true);
        }

        isHighPriority = value;
    }

    public virtual void ToggleHighPriority()
    {
        SetHighPriority(!isHighPriority);
    }
}