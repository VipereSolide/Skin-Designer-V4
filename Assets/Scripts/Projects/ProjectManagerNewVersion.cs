using System.Collections.Generic;
using System.Collections;
using System.Linq;

using UnityEngine;

using TMPro;

namespace SkinDesigner.Project
{
    public class ProjectManagerNewVersion : MonoBehaviour
    {
        [SerializeField] private ProjectWindowManager manager;
        [SerializeField] private TMP_Text projectNameText;

        private int mediaIdCounter;

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
            }

            foreach (ProjectWindowContentWeapon weapon in projectWeaponsItems)
            {
                items.Remove(weapon);
                WeaponTextures textures = new WeaponTextures(0, 0, 0, 0, 0, 0, 0);
                ProjectWeapon output = new ProjectWeapon((int)weapon.Weapon, textures);
            }
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
