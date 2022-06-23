using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkinDesigner;

namespace SkinDesigner.Project
{
    [System.Serializable]
    public class Project
    {
        [SerializeField]
        private string m_projectName;

        [SerializeField]
        private SkinSystem.Weapon[] m_projectHandledWeapons;

        [SerializeField]
        private Weapon.WeaponObject[] m_projectWeaponObjects;

        public string ProjectName
        {
            get { return m_projectName; }
        }

        public SkinSystem.Weapon[] ProjectHandledWeapons
        {
            get { return m_projectHandledWeapons; }
        }

        public Weapon.WeaponObject[] ProjectWeaponObjects
        {
            get { return m_projectWeaponObjects; }
        }

        public Project(string _Name, SkinSystem.Weapon[] _HandledWeapons, Weapon.WeaponObject[] _WeaponObjects)
        {
            this.m_projectName = _Name;
            this.m_projectHandledWeapons = _HandledWeapons;
            this.m_projectWeaponObjects = _WeaponObjects;
        }
    }
}