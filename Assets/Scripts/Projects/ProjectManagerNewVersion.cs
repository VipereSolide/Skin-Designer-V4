using System.Collections.Generic;
using System.Collections;
using System.Linq;

using UnityEngine;

using TMPro;

using SkinDesigner.Weapon;

namespace SkinDesigner.Project
{
    public class ProjectManagerNewVersion : MonoBehaviour
    {
        [SerializeField] private TMP_Text testText;

        [Space()]

        [SerializeField] private ProjectWindowManager manager;
        [SerializeField] private TMP_Text projectNameText;

        private int mediaIdCounter;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SaveProject();
            }
        }

        public void SaveProject()
        {
            string project_name = projectNameText.text;
            List<ProjectWindowContentItem> items = manager.Items.ToList();
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

            Project2 project = new Project2();
            project.ProjectMedia = projectMedias.ToArray();
            project.ProjectName = project_name;
            project.WeaponData = projectWeapons.ToArray();
            string json = JsonUtility.ToJson(project);
            Debug.Log("**JSON OUTPUT**" + "\n" + json);
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
            mediaIdCounter++;

            return media;
        }
    }
}
