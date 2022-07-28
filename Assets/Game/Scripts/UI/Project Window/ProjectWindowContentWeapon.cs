using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using SkinDesigner.SkinSystem;
using SkinDesigner.Weapon;

public class ProjectWindowContentWeapon : ProjectWindowContentItem
{
    [Header("Weapon Settings")]
    
    [SerializeField]
    private Weapon weapon;

    public Weapon Weapon
    {
        get { return weapon; }
        set { weapon = value; }
    }
}
