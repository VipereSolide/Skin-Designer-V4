using System.Collections.Generic;
using System.Text;
using System.IO;

using UnityEngine.UI;
using UnityEngine;

using SkinDesigner.Textures;
using FeatherLight.Pro;
using TMPro;

namespace SkinDesigner.Project
{
    public class ProjectManagerUI : MonoBehaviour
    {
        public static ProjectManagerUI Instance;

        [Header("References")]
        [SerializeField] private Transform weaponParent;
        [SerializeField] private GridLayoutGroup m_verticalProjectsList;
        [SerializeField] private ProjectManagerItem m_itemPrefab;
        [SerializeField] private CanvasGroup m_windowCanvasGroup;
        [SerializeField] private TMP_Text m_projectNameText;

        [Header("Settings")]
        [SerializeField] private bool m_showOnStart = true;

        private List<ProjectManagerItem> spawnedItems = new List<ProjectManagerItem>();

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            //ProjectManager.Instance.SaveProjectOnDisk(new Project("zad", new SkinSystem.Weapon[]{SkinSystem.Weapon.AK47}, new Weapon.WeaponObject[]{weaponParent.Find("AK47").GetComponent<Weapon.WeaponObject>()}));

            Init();

            if (m_showOnStart)
                SetActive(true);
        }

        public void SetActive(bool value)
        {
            CanvasGroupHelper.SetActive(m_windowCanvasGroup, value);
        }

        public void SelectProject(Project info)
        {
            ProjectManager manager = ProjectManager.Instance;

            manager.CurrentProject = info;
            manager.UpdateProject();

            SetActive(false);
        }

        public ProjectManagerItem[] Init()
        {
            foreach(ProjectManagerItem item in spawnedItems)
            {
                Destroy(item.gameObject);
            }

            spawnedItems.Clear();
            
            string _directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Skillwarz/Skin Designer/Projects/";
            string[] _filesInDir = Directory.GetFiles(_directoryPath);

            foreach (string file in _filesInDir)
            {
                if (Path.GetExtension(file) != ".json")
                    continue;

                Debug.Log("file: " + file);

                string fileContent = File.ReadAllText(file);
                Project info = ProjectManager.Instance.GetProject(fileContent);

                ProjectManagerItem instantiated = Instantiate(m_itemPrefab, m_verticalProjectsList.transform);
                instantiated.AssociateWithInfo(info);

                spawnedItems.Add(instantiated);
            }

            return spawnedItems.ToArray();
        }
    }
}