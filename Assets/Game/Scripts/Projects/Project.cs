using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

using UnityEngine;

namespace SkinDesigner.Project
{
    [Serializable]
    public class Project
    {
        [SerializeField]
        private string project_name = "";

        [SerializeField]
        private List<ProjectWeapon> weapons_data = new List<ProjectWeapon>();

        [SerializeField]
        private List<ProjectMedia> project_medias = new List<ProjectMedia>();

        public string ProjectName
        {
            get { return project_name; }
            set { project_name = value; }
        }

        public ProjectWeapon[] WeaponData
        {
            get { return weapons_data.ToArray(); }
            set { weapons_data = value.ToList(); }
        }

        public ProjectMedia[] ProjectMedia
        {
            get { return project_medias.ToArray(); }
            set { project_medias = value.ToList(); }
        }

        public Project(string name)
        {
            project_name = name;
        }
    }

    [Serializable]
    public class ProjectWeapon
    {

        [SerializeField]
        private int weapon_index;

        [SerializeField]
        private WeaponTextures weapon_textures = new WeaponTextures();

        [SerializeField]
        private List<WeaponSubRenderer> weapon_sub_renderers_textures = new List<WeaponSubRenderer>();

        public int WeaponIndex
        {
            get { return weapon_index; }
            set { weapon_index = value; }
        }

        public WeaponTextures WeaponTextures
        {
            get { return weapon_textures; }
            set { weapon_textures = value; }
        }

        public WeaponSubRenderer[] WeaponSubRenderers
        {
            get { return weapon_sub_renderers_textures.ToArray(); }
            set { weapon_sub_renderers_textures = value.ToList(); }
        }

        public ProjectWeapon()
        {

        }

        public ProjectWeapon(int weaponIndex, WeaponTextures weaponTextures)
        {
            this.weapon_index = weaponIndex;
            this.weapon_textures = weaponTextures;
        }

        public ProjectWeapon(int weaponIndex, WeaponTextures weaponTextures, WeaponSubRenderer[] weaponSubRenderers)
        {
            this.weapon_index = weaponIndex;
            this.weapon_textures = weaponTextures;
            this.weapon_sub_renderers_textures = weaponSubRenderers.ToList();
        }

        public ProjectWeapon(int weaponIndex, WeaponTextures weaponTextures, List<WeaponSubRenderer> weaponSubRenderers)
        {
            this.weapon_index = weaponIndex;
            this.weapon_textures = weaponTextures;
            this.weapon_sub_renderers_textures = weaponSubRenderers;
        }
    }

    [Serializable]
    public class WeaponSubRenderer
    {
        [SerializeField]
        private WeaponTextures textures = new WeaponTextures();

        public WeaponTextures Textures
        {
            get { return textures; }
            set { textures = value; }
        }
    }

    [Serializable]
    public class ProjectMedia
    {
        [SerializeField]
        private int media_id = 0;

        [SerializeField]
        private string media_name = "";

        [SerializeField]
        private string media_type = "";

        [SerializeField]
        private string media_system_path = "";

        [SerializeField]
        private string media_children_path = "Root";

        [SerializeField]
        private string media_path = "";

        [SerializeField]
        private int media_weapon = 0;

        public int MediaID
        {
            get { return media_id; }
            set { media_id = value; }
        }

        public string MediaName
        {
            get { return media_name; }
            set { media_name = value; }
        }

        public string MediaType
        {
            get { return media_type; }
            set { media_type = value; }
        }

        public string MediaSystemPath
        {
            get { return media_system_path; }
            set { media_system_path = value; }
        }

        public string MediaChildrenPath
        {
            get { return media_children_path; }
            set { media_children_path = value; }
        }

        public string MediaPath
        {
            get { return media_path; }
            set { media_path = value; }
        }

        public int MediaWeapon
        {
            get { return media_weapon; }
            set { media_weapon = value; }
        }

        public ProjectMedia(int mediaID, string mediaName, string mediaType, string mediaSystemPath, string mediaChildrenPath, string mediaPath)
        {
            this.media_id = mediaID;
            this.media_name = mediaName;
            this.media_type = mediaType;
            this.media_system_path = mediaSystemPath;
            this.media_children_path = mediaChildrenPath;
            this.media_path = mediaPath;
        }

        public ProjectMedia(int mediaID, string mediaName, string mediaType, string mediaSystemPath, string mediaChildrenPath, string mediaPath, int mediaWeapon)
        {
            this.media_id = mediaID;
            this.media_name = mediaName;
            this.media_type = mediaType;
            this.media_system_path = mediaSystemPath;
            this.media_children_path = mediaChildrenPath;
            this.media_path = mediaPath;
            this.media_weapon = mediaWeapon;
        }
    }

    [Serializable]
    public class WeaponTextures
    {

        [SerializeField]
        private string albedo = "NULL";

        [SerializeField]
        private string detail = "NULL";

        [SerializeField]
        private string emission = "NULL";

        [SerializeField]
        private string height = "NULL";

        [SerializeField]
        private string metallic = "NULL";

        [SerializeField]
        private string normal = "NULL";

        [SerializeField]
        private string occlusion = "NULL";

        public string Albedo
        {
            get { return albedo; }
            set { albedo = value; }
        }
        
        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }
        
        public string Emission
        {
            get { return emission; }
            set { emission = value; }
        }
        
        public string Height
        {
            get { return height; }
            set { height = value; }
        }
        
        public string Metallic
        {
            get { return metallic; }
            set { metallic = value; }
        }
        
        public string Normal
        {
            get { return normal; }
            set { normal = value; }
        }
        
        public string Occlusion
        {
            get { return occlusion; }
            set { occlusion = value; }
        }

        public WeaponTextures(string _albedo, string _detail, string _emission, string _height, string _metallic, string _normal, string _occlusion)
        {
            this.albedo = _albedo;
            this.detail = _detail;
            this.emission = _emission;
            this.height = _height;
            this.metallic = _metallic;
            this.normal = _normal;
            this.occlusion = _occlusion;
        }

        public WeaponTextures()
        {
            
        }
    }
}