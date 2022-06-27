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
    public class ProjectManagerHUD : MonoBehaviour
    {
        public static ProjectManagerHUD Instance;

        [Header("References")]
        [SerializeField] private Transform weaponParent;
        [SerializeField] private GridLayoutGroup m_verticalProjectsList;
        [SerializeField] private ProjectManagerItem m_itemPrefab;
        [SerializeField] private CanvasGroup m_windowCanvasGroup;
        [SerializeField] private TMP_Text m_projectNameText;

        [Header("Settings")]
        [SerializeField] private bool m_showOnStart = true;


        public ProjectReaderInfo m_info;
        private List<ProjectManagerItem> m_spawnedItems = new List<ProjectManagerItem>();

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

        public void SelectProject(ProjectReaderInfo info)
        {
            ProjectManager.GetProjectMediasFromString(info.ProjectStringData);
            ProjectManagerHUD.Instance.SetActive(false);
            m_projectNameText.text = info.Name;

            foreach (Transform child in weaponParent)
                child.gameObject.SetActive(false);

            foreach (ProjectReaderInfo.HandledWeapon weapon in info.Weapons)
            {
                weapon.CorrespondingObject.gameObject.SetActive(true);
                weapon.CorrespondingObject.SetTextures(weapon.MainTextures);

                ProjectWindowManager.Instance.CreateWeapon(weapon.Weapon);
                Inspector.InspectorManager.Instance.SetInspectedWeapon(weapon.Weapon);
            }

            string[] medias = ProjectManager.GetProjectMediasFromString(info.ProjectStringData);

            for (int i = 0; i < medias.Length; i++)
            {
                string media = medias[i].RemoveControlCharacters();

                string mediaName = Path.GetFileNameWithoutExtension(media);
                ProjectWindowManager.Instance.CreateMedia(mediaName, media);
            }
        }

        public void SelectProject(Project info)
        {
            ProjectReaderInfo rInfo = ProjectManager.GetProjectFromString(ProjectManager.GetProjectFile(info));

            ProjectManager.GetProjectMediasFromString(rInfo.ProjectStringData);
            ProjectManagerHUD.Instance.SetActive(false);
            m_projectNameText.text = rInfo.Name;

            foreach (Transform child in weaponParent)
                child.gameObject.SetActive(false);

            foreach (ProjectReaderInfo.HandledWeapon weapon in rInfo.Weapons)
            {
                weapon.CorrespondingObject.gameObject.SetActive(true);
                weapon.CorrespondingObject.SetTextures(weapon.MainTextures);
                Inspector.InspectorManager.Instance.SetInspectedWeapon(weapon.Weapon);
            }

            string[] medias = ProjectManager.GetProjectMediasFromString(rInfo.ProjectStringData);

            for (int i = 0; i < medias.Length; i++)
            {
                string media = medias[i].RemoveControlCharacters();

                string mediaName = Path.GetFileNameWithoutExtension(media);
                ProjectWindowContentItem item = ProjectWindowManager.Instance.CreateMedia(mediaName, media);
            }
        }

        public ProjectManagerItem[] Init()
        {
            foreach (ProjectManagerItem item in m_spawnedItems)
            {
                m_spawnedItems.Remove(item);
                Destroy(item.gameObject);
            }

            string _directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Skillwarz/Skin Designer/Projects/";
            string[] _filesInDir = Directory.GetFiles(_directoryPath);

            foreach (string file in _filesInDir)
            {
                if (Path.GetExtension(file) != "json" && file == "C:\\Users\\comma\\Documents\\Skillwarz\\Skin Designer\\Projects\\AK47 Cherry Bloodsome.json")
                    continue;

                ProjectReaderInfo info = ProjectManager.GetProjectFromString(File.ReadAllText(file));

                ProjectManagerItem instantiated = Instantiate(m_itemPrefab, m_verticalProjectsList.transform);
                instantiated.AssociateWithInfo(info);

                m_spawnedItems.Add(instantiated);
            }

            return m_spawnedItems.ToArray();
        }

        private ProjectManagerItem[] FetchProjects()
        {
            List<ProjectManagerItem> _output = new List<ProjectManagerItem>();

            string _directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Skillwarz/Skin Designer/Projects/";
            string[] _filesInDir = Directory.GetFiles(_directoryPath);

            foreach (string _File in _filesInDir)
            {

            }

            ProjectManager.GetProjectFromString(_filesInDir[0]);

            return null;
        }
    }
}