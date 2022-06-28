using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;

using UnityEngine;

using TMPro;

using SkinDesigner.Inspector;
using SkinDesigner.Textures;
using SkinDesigner.Weapon;

namespace SkinDesigner.Project
{
    public class ProjectManager : MonoBehaviour
    {
        public static ProjectManager Instance;

        [SerializeField] private Project outputProject;
        [SerializeField] private TMP_Text testText;
        [SerializeField] private ProjectWindowManager projectWindowManager;
        [SerializeField] private TMP_Text projectNameText;
        [SerializeField] private string projectPath;

        private int mediaIdCounter;
        private Project currentProject = null;
        public Project CurrentProject
        {
            get { return currentProject; }
            set { currentProject = value; }
        }

        private void Awake()
        {
            Instance = this;
        }

        public void SaveProject()
        {
            mediaIdCounter = 0;

            string project_name = projectNameText.text;
            List<ProjectWindowContentItem> items = projectWindowManager.Items.ToList();
            List<ProjectWindowContentWeapon> projectWeaponsItems = new List<ProjectWindowContentWeapon>();

            List<ProjectMedia> projectMedias = new List<ProjectMedia>();
            List<ProjectWeapon> projectWeapons = new List<ProjectWeapon>();

            foreach (ProjectWindowContentItem item in items)
            {
                if (item.GetType() == typeof(ProjectWindowContentWeapon))
                {
                    projectWeaponsItems.Add((ProjectWindowContentWeapon)item);
                }

                projectMedias.Add(ContentItemToMedia(item));
            }

            foreach (ProjectWindowContentWeapon weapon in projectWeaponsItems)
            {
                WeaponObject weaponObject = WeaponManager.Instance.GetWeaponByWeaponType(weapon.Weapon);

                WeaponTextures textures = new WeaponTextures(weaponObject.WeaponTextures.TextureObjects[0].TexturePath, weaponObject.WeaponTextures.TextureObjects[1].TexturePath, weaponObject.WeaponTextures.TextureObjects[2].TexturePath, weaponObject.WeaponTextures.TextureObjects[3].TexturePath, weaponObject.WeaponTextures.TextureObjects[4].TexturePath, weaponObject.WeaponTextures.TextureObjects[5].TexturePath, weaponObject.WeaponTextures.TextureObjects[6].TexturePath);
                ProjectWeapon output = new ProjectWeapon((int)weapon.Weapon, textures);

                projectWeapons.Add(output);
            }

            Project project = new Project();
            project.ProjectMedia = projectMedias.ToArray();
            project.ProjectName = project_name;
            project.WeaponData = projectWeapons.ToArray();
            string json = JsonUtility.ToJson(project);
            Debug.Log(json);
        }

        public string GetProjectInString(Project project)
        {
            string json = JsonUtility.ToJson(project);
            return json;
        }

        public Project GetProject(string json)
        {
            return (Project)JsonUtility.FromJson<Project>(json);
        }

        public void UpdateProject()
        {
            if (currentProject == null)
                return;

            projectNameText.text = currentProject.ProjectName;

            if (currentProject.ProjectMedia.Length > 0)
            {
                foreach (ProjectMedia media in currentProject.ProjectMedia)
                {
                    if (media.MediaType == "file")
                    {
                        ProjectWindowContentItem item = projectWindowManager.CreateMedia(media.MediaName, media.MediaSystemPath);
                        item.SetDirectory(media.MediaChildrenPath);
                    }
                    else if (media.MediaType == "folder")
                    {
                        if (media.MediaName != "..")
                        {
                            ProjectWindowContentFolder item = projectWindowManager.CreateFolder(media.MediaName);
                            item.SetDirectory(media.MediaChildrenPath);
                            item.SetPath(media.MediaPath);
                        }
                    }
                    else
                    {
                        ProjectWindowContentWeapon item = projectWindowManager.CreateWeapon(media.MediaWeapon, media.MediaChildrenPath);
                        item.SetDirectory(media.MediaChildrenPath);
                    }
                }
            }

            projectWindowManager.UpdatePath("Root");

            if (currentProject.WeaponData.Length > 0)
            {
                foreach (ProjectWeapon weapon in currentProject.WeaponData)
                {
                    WeaponManager weaponManager = WeaponManager.Instance;
                    InspectorManager inspectorManager = InspectorManager.Instance;

                    SkinSystem.Weapon weaponType = SkinSystem.Environment.IntToWeapon(weapon.WeaponIndex);

                    inspectorManager.SetInspectedWeapon(weaponType);

                    TextureObject[] weaponTextures = new TextureObject[7]
                    {
                    new TextureObject(weapon.WeaponTextures.Albedo),
                    new TextureObject(weapon.WeaponTextures.Detail),
                    new TextureObject(weapon.WeaponTextures.Emission),
                    new TextureObject(weapon.WeaponTextures.Height),
                    new TextureObject(weapon.WeaponTextures.Metallic),
                    new TextureObject(weapon.WeaponTextures.Normal),
                    new TextureObject(weapon.WeaponTextures.Occlusion)
                    };

                    weaponManager.InstantiateWeapon(weaponType);
                    weaponManager.SetAllTextures(weaponTextures);
                    inspectorManager.UpdateAllTextureHolders();

                    if (weapon.WeaponSubRenderers.Length > 0)
                    {
                        List<TextureObject[]> textures = new List<TextureObject[]>();

                        foreach (WeaponSubRenderer subRenderer in weapon.WeaponSubRenderers)
                        {
                            TextureObject[] textureObjects = new TextureObject[7]
                            {
                            new TextureObject(subRenderer.Textures.Albedo),
                            new TextureObject(subRenderer.Textures.Detail),
                            new TextureObject(subRenderer.Textures.Emission),
                            new TextureObject(subRenderer.Textures.Height),
                            new TextureObject(subRenderer.Textures.Metallic),
                            new TextureObject(subRenderer.Textures.Normal),
                            new TextureObject(subRenderer.Textures.Occlusion)
                            };

                            textures.Add(textureObjects);
                        }

                        weaponManager.SetPartsTexture(textures);
                    }
                }
            }
        }

        public ProjectMedia FindItemInMedias(ProjectWindowContentItem item, ProjectMedia[] medias)
        {
            ProjectMedia output = null;

            for (int i = 0; i < medias.Length; i++)
            {
                if (medias[i].MediaName == item.Name && medias[i].MediaSystemPath == item.HeldTexture.TexturePath)
                {
                    output = medias[i];
                }
            }

            return output;
        }

        public ProjectMedia ContentItemToMedia(ProjectWindowContentItem item)
        {
            string mediaType = "file";

            if (item.GetType() == typeof(ProjectWindowContentWeapon))
            {
                mediaType = "weapon";
            }

            if (item.GetType() == typeof(ProjectWindowContentFolder))
            {
                mediaType = "folder";
            }

            string systemPath = "";

            if (mediaType == "file")
            {
                systemPath = item.HeldTexture.TexturePath;
            }

            string path = "";

            if (mediaType == "folder")
            {
                path = ((ProjectWindowContentFolder)item).Path;
            }

            ProjectMedia media = new ProjectMedia(mediaIdCounter, item.Name, mediaType, systemPath, item.ChildrenPath, path);

            if (mediaType == "weapon")
            {
                media.MediaWeapon = (int)((ProjectWindowContentWeapon)item).Weapon;
            }

            mediaIdCounter++;

            return media;
        }

        public void SaveProjectWithInputField(TMP_InputField field)
        {
            Project newCurrent = new Project();
            newCurrent.ProjectName = field.text;

            string directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Skillwarz/Skin Designer/Projects/";
            File.WriteAllText(directoryPath + field.text + ".json", GetProjectInString(newCurrent));

            currentProject = newCurrent;
            ProjectManagerUI.Instance.SelectProject(currentProject);
        }
    }
}
