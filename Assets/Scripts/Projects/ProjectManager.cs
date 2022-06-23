using System.IO;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

using TMPro;
using FeatherLight.Pro;
using SkinDesigner.Textures;

namespace SkinDesigner.Project
{
    public class ProjectManager : MonoBehaviour
    {
        public static ProjectManager Instance;

        [SerializeField]
        private Project m_project;

        [Header("UI")]

        [SerializeField]
        private TMP_InputField m_projectNameInputField;

        public static string PROJECT_PATH = "";
        public static string PROJECT_RESOURCES_PATH = "";

        private void Awake()
        {
            Instance = this;

            CreateProjectHierarchy();
        }

        public void UISaveProjectOnDisk()
        {
            Project _newProject = new Project(m_projectNameInputField.text, new SkinSystem.Weapon[] { }, new Weapon.WeaponObject[] { });
            m_project = _newProject;

            SaveProjectOnDisk(_newProject);

            ProjectManagerHUD.Instance.SelectProject(_newProject);
        }

        /*public static Project ProjectReaderInfoToProject(ProjectReaderInfo info)
        {
            List<SkinSystem.Weapon> weapons = new List<SkinSystem.Weapon>();

            foreach (ProjectReaderInfo.HandledWeapon weapon in info.Weapons)
            {
                weapons.Add(weapon.Weapon);
            }

            List<Weapon.WeaponObject> objects = new List<Weapon.WeaponObject>();

            foreach (ProjectReaderInfo.HandledWeapon obj in info.Weapons)
            {
                objects.Add(obj.CorrespondingObject);
            }

            return new Project(info.Name, weapons.ToArray(), objects.ToArray());
        }*/

        private void CreateProjectHierarchy()
        {
            string _projectPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Skillwarz/Skin Designer/Projects/";
            string _projectResourcesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Skillwarz/Skin Designer/Project Resources/";

            Directory.CreateDirectory(_projectPath);
            Directory.CreateDirectory(_projectResourcesPath);

            PROJECT_PATH = _projectPath;
            PROJECT_RESOURCES_PATH = _projectResourcesPath;
        }

        public void SaveProjectOnDisk(Project _Project)
        {
            StringBuilder _projectFilePathBuilder = new StringBuilder(PROJECT_PATH);

            _projectFilePathBuilder.Append(_Project.ProjectName);
            _projectFilePathBuilder.Append(".json");

            string _projectFilePath = _projectFilePathBuilder.ToString();

            File.WriteAllText(_projectFilePath, GetProjectFile(_Project));
        }

        public static string RemoveSpaceAtStart(string _Value)
        {
            string _output = _Value;

            while (_output[0] == ' ' && !(_Value.Contains("{") && _Value.Contains("}")))
            {
                _output = _output.Remove(0, 1);
            }

            return _output;
        }

        public static string[] GetProjectMediasFromString(string _Project)
        {
            // Get all the medias from a project string file.
            string currentBracketsCategoryName = "";

            int _openBracketsCount = 0;

            foreach (string _Line in _Project.Split('\n'))
            {
                string _line = _Line;

                if (_line.Contains("["))
                {
                    _openBracketsCount++;
                }

                if (_line.Contains("]"))
                {
                    _openBracketsCount--;
                }

                if (_line.Contains("ProjectMedias"))
                {
                    currentBracketsCategoryName = "ProjectMedias";
                }
                
                if (_openBracketsCount >= 1)
                {
                    if (currentBracketsCategoryName == "ProjectMedias")
                    {
                        if (!_line.Contains("[") && !_line.Contains("]"))
                        {
                            Debug.Log("Mediaa");
                            string mediaPath = _line.RemoveStartSpace().ReplaceAll(new string[]{"\"",","}, "");
                            ProjectWindowManager.Instance.AddMedia(mediaPath);
                        }
                    }
                }
            }

            return ProjectWindowManager.Instance.Medias;
        }

        public static ProjectReaderInfo GetProjectFromString(string _Project, bool setProjectMedias = true)
        {
            ProjectWindowManager.Instance.ResetMedias();

            string[] _projectLines = _Project.Split('\n');

            int _openCurlyBracketsCount = 0;
            int _openBracketsCount = 0;

            bool[] moduleBracketsIsOpened = new bool[] { false, false };
            bool moduleIsOpened = false;

            bool isMainTextureFlagLocked = false;

            string currentBracketsCategoryName = "";

            ProjectReaderInfo info = new ProjectReaderInfo();
            List<ProjectReaderInfo.HandledWeapon> _handleds = new List<ProjectReaderInfo.HandledWeapon>();
            ProjectReaderInfo.HandledWeapon _handled = new ProjectReaderInfo.HandledWeapon();
            int _handledIndex = -1;

            info.ProjectStringData = _Project;

            foreach (string _Line in _projectLines)
            {
                string _line = _Line;

                if (_line.Contains("{"))
                {
                    _openCurlyBracketsCount++;

                    if (_openCurlyBracketsCount == 2)
                    {
                        _handleds.Add(new ProjectReaderInfo.HandledWeapon());
                        _handledIndex++;
                    }
                }

                if (_line.Contains("}"))
                {
                    _openCurlyBracketsCount--;
                }

                if (_line.Contains("["))
                {
                    _openBracketsCount++;
                }

                if (_line.Contains("]"))
                {
                    _openBracketsCount--;
                }

                if (_line.Contains("HandledWeapons"))
                {
                    currentBracketsCategoryName = "HandledWeapons";
                }

                if (_line.Contains("ProjectMedias"))
                {
                    currentBracketsCategoryName = "ProjectMedias";
                }

                if (_openCurlyBracketsCount >= 1)
                {
                    if (_line.Contains("\"ProjectName\""))
                    {
                        info.Name = _line.Split(':')[1].Replace("\"", "").Replace(",", "");
                    }
                }

                if (_openBracketsCount >= 1)
                {
                    if (currentBracketsCategoryName == "HandledWeapons")
                    {
                        if (_openCurlyBracketsCount > 1)
                        {
                            if (_line.Contains("\"HasMultipleParts\""))
                            {
                                string _value = _line.Split(':')[1].Replace("\"", "").Replace(",", "");
                                _handleds[_handledIndex].HasMultipleParts = bool.Parse(_value);
                            }

                            if (_line.Contains("\"WeaponType\":"))
                            {
                                string _value = _line.Split(':')[1].ReplaceAll(new string[] { "\"", ",", "\n" }, "").RemoveControlCharacters();

                                SkinSystem.Weapon weaponType = (SkinSystem.Weapon)System.Enum.Parse(typeof(SkinSystem.Weapon), _value);
                                Weapon.WeaponObject returned = SkinDesigner.Weapon.WeaponManager.Instance.GetWeaponByWeaponType(weaponType);

                                _handleds[_handledIndex].CorrespondingObject = returned.transform.GetComponent<Weapon.WeaponObject>();
                                _handleds[_handledIndex].Weapon = weaponType;
                            }

                            #region Main Textures
                            if (_line.Contains("\"MainTextures\"") && !isMainTextureFlagLocked)
                            {
                                string mainTextureLineText = _line.ReplaceAll(new string[] { "[", "]", "\"" }, "").RemoveStartSpace().RemoveStartWord("MainTextures: ");

                                string textures = mainTextureLineText;

                                string[] textureParts = textures.Split(',');

                                for (int i = 0; i < textureParts.Length - 1; i++)
                                {
                                    if (i < 7)
                                    {
                                        if (!string.IsNullOrEmpty(textureParts[i]))
                                        {
                                            _handleds[_handledIndex].MainTextures[i].TexturePath = textureParts[i];
                                        }
                                    }
                                    else
                                    {
                                        _handleds[_handledIndex].MainTextureMaterialMetallicMode = (SkinDesigner.SkinSystem.MaterialMetallicMode)System.Enum.Parse(
                                            typeof(SkinDesigner.SkinSystem.MaterialMetallicMode),
                                            textureParts[i]
                                        );
                                    }
                                }

                                isMainTextureFlagLocked = true;
                            }

                            #endregion

                            if (_openBracketsCount >= 2)
                            {
                                if (_openCurlyBracketsCount == 3)
                                {
                                    moduleIsOpened = true;
                                }

                                bool moduleStartLine = _line.Replace("\\n", "").Replace(" ", "") == "{";
                                bool moduleEndLine = _line.Replace("\\n", "").Replace(" ", "") == "}";

                                if (moduleIsOpened && !moduleStartLine && !moduleEndLine)
                                {
                                    #region Secondary Textures
                                    if (true)
                                    {
                                        string secondaryTextureHarsh = "\"SecondaryTextures\": [";
                                        bool moduleBracketsStartLine = _line.Replace("\\n", "").Contains(secondaryTextureHarsh);
                                        bool moduleBracketsEndLine = _line.Replace("\\n", "").Replace(" ", "") == "]";

                                        if (moduleBracketsStartLine)
                                        {
                                            moduleBracketsIsOpened[0] = true;
                                        }

                                        if (moduleBracketsEndLine)
                                        {
                                            moduleBracketsIsOpened[0] = false;
                                        }

                                        if (moduleBracketsIsOpened[0] && !moduleBracketsStartLine)
                                        {
                                            string textures = _line.ReplaceAll(new string[] { "[", "]", "\"" }, "").RemoveStartSpace();

                                            string[] textureParts = textures.Split(',');

                                            for (int i = 0; i < textureParts.Length - 1; i++)
                                            {
                                                if (i < 7)
                                                {
                                                    if (!string.IsNullOrEmpty(textureParts[i]))
                                                    {
                                                        _handleds[_handledIndex].SecondaryTextures[i].TexturePath = textureParts[i];
                                                    }
                                                }
                                                else
                                                {
                                                    _handleds[_handledIndex].SecondaryTextureMaterialMetallicMode = (SkinDesigner.SkinSystem.MaterialMetallicMode)System.Enum.Parse(
                                                        typeof(SkinDesigner.SkinSystem.MaterialMetallicMode),
                                                        textureParts[i]
                                                    );
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }

                                if (moduleIsOpened && moduleEndLine)
                                {
                                    moduleIsOpened = false;
                                }
                            }
                        }
                    }

                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                }
            }

            foreach (ProjectReaderInfo.HandledWeapon h in _handleds)
            {
                info.Weapons.Add(h);
            }

            return info;
        }

        protected static string ReturnTrueText(string value)
        {
            return (string.IsNullOrEmpty(value)) ? "NULL" : value;
        }

        public static string GetProjectFile(Project _Project)
        {
            StringBuilder _projectBuilder = new StringBuilder();

            _projectBuilder.Append("{\n");
            _projectBuilder.Append("    \"ProjectName\": \"" + _Project.ProjectName + "\",\n");
            _projectBuilder.Append("    \"HandledWeapons\":\n");
            _projectBuilder.Append("    [\n");

            foreach (Weapon.WeaponObject _Object in _Project.ProjectWeaponObjects)
            {
                bool _isLastIndex = (_Object == _Project.ProjectWeaponObjects[_Project.ProjectWeaponObjects.Length - 1]);

                _projectBuilder.Append("        {\n");
                _projectBuilder.Append("            \"WeaponType\": " + SkinSystem.Environment.WeaponToInt(_Object.Weapon) + ",\n");
                _projectBuilder.Append("            \"HasMultipleParts\": " + _Object.HasMultipleParts.ToString().ToLower() + ",\n");
                _projectBuilder.Append("            \"CorrespondingObjects\": " + _Object.gameObject.GetInstanceID() + ",\n");
                _projectBuilder.Append("            \"CorrespondingObjectsTextures\":\n");
                _projectBuilder.Append("            {\n");
                _projectBuilder.Append("                \"MainTextures\": [\"");
                _projectBuilder.Append(ReturnTrueText(_Object.WeaponTextures.TextureObjects[0].TexturePath.Replace("\\", "/")));
                _projectBuilder.Append("\",\"");
                _projectBuilder.Append(ReturnTrueText(_Object.WeaponTextures.TextureObjects[1].TexturePath.Replace("\\", "/")));
                _projectBuilder.Append("\",\"");
                _projectBuilder.Append(ReturnTrueText(_Object.WeaponTextures.TextureObjects[2].TexturePath.Replace("\\", "/")));
                _projectBuilder.Append("\",\"");
                _projectBuilder.Append(ReturnTrueText(_Object.WeaponTextures.TextureObjects[3].TexturePath.Replace("\\", "/")));
                _projectBuilder.Append("\",\"");
                _projectBuilder.Append(ReturnTrueText(_Object.WeaponTextures.TextureObjects[4].TexturePath.Replace("\\", "/")));
                _projectBuilder.Append("\",\"");
                _projectBuilder.Append(ReturnTrueText(_Object.WeaponTextures.TextureObjects[5].TexturePath.Replace("\\", "/")));
                _projectBuilder.Append("\",\"");
                _projectBuilder.Append(ReturnTrueText(_Object.WeaponTextures.TextureObjects[6].TexturePath.Replace("\\", "/")));
                _projectBuilder.Append("\",\"");
                _projectBuilder.Append(_Object.MaterialMetallicMode.ToString() + "\"");
                _projectBuilder.Append("],\n");

                if (_Object.HasMultipleParts)
                {
                    _projectBuilder.Append("                \"SecondaryTextures\": [\n");
                    foreach (Weapon.WeaponObject.WeaponSubRenderer _Part in _Object.WeaponSubRenderers)
                    {
                        bool _isLastPart = (_Part == _Object.WeaponSubRenderers[_Object.WeaponSubRenderers.Length - 1]);
                        _projectBuilder.Append("                    [\"" + _Part.TextureData.TextureObjects[0].TexturePath.Replace("\\", "/") + "\",\"" + _Part.TextureData.TextureObjects[1].TexturePath.Replace("\\", "/") + "\",\"" + _Part.TextureData.TextureObjects[2].TexturePath.Replace("\\", "/") + "\",\"" + _Part.TextureData.TextureObjects[3].TexturePath.Replace("\\", "/") + "\",\"" + _Part.TextureData.TextureObjects[4].TexturePath.Replace("\\", "/") + "\",\"" + _Part.TextureData.TextureObjects[5].TexturePath.Replace("\\", "/") + "\",\"" + _Part.TextureData.TextureObjects[6].TexturePath.Replace("\\", "/") + "\"");
                        _projectBuilder.Append(",\"" + _Part.MaterialMetallicMode.ToString() + "\"");
                        _projectBuilder.Append("]");
                        _projectBuilder.Append((_isLastPart) ? "\n" : ",\n");
                    }

                    _projectBuilder.Append("                ]\n");
                }
                else
                {
                    _projectBuilder.Append("                \"SecondaryTextures\": [\n");
                    _projectBuilder.Append("                ]\n");
                }

                _projectBuilder.Append("            }\n");
                _projectBuilder.Append("        }");
                _projectBuilder.Append((_isLastIndex) ? "\n" : ",\n");

            }

            _projectBuilder.Append("    ],\n");

            string[] medias = ProjectWindowManager.Instance.Medias;

            _projectBuilder.Append("    \"ProjectMedias\":\n");
            _projectBuilder.Append("    [\n");

            foreach (string media in medias)
            {
                _projectBuilder.Append("        \"");
                _projectBuilder.Append(media.Replace("\\", "/").RemoveStartSpace());
                _projectBuilder.Append("\"");

                if (media != medias[medias.Length - 1])
                    _projectBuilder.Append(",");

                _projectBuilder.Append("\n");
            }

            _projectBuilder.Append("    ]\n");

            _projectBuilder.Append("}\n");

            return _projectBuilder.ToString();
        }
    }

    [System.Serializable]
    public class ProjectReaderInfo
    {
        [SerializeField]
        private string m_projectStringData;

        [SerializeField]
        private string m_name;

        [SerializeField]
        private List<HandledWeapon> m_weapon = new List<HandledWeapon>();

        public string ProjectStringData
        {
            get
            {
                return m_projectStringData;
            }
            set
            {
                m_projectStringData = value;
            }
        }

        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public List<HandledWeapon> Weapons
        {
            get
            {
                return m_weapon;
            }
            set
            {
                m_weapon = value;
            }
        }

        [System.Serializable]
        public class HandledWeapon
        {
            [SerializeField]
            private SkinSystem.Weapon m_weapon;

            [SerializeField]
            private bool m_hasMulipleParts;

            [SerializeField]
            private Weapon.WeaponObject m_correspondingObject;

            [SerializeField]
            private TextureObject[] m_mainTextures = new TextureObject[7] { new TextureObject(""), new TextureObject(""), new TextureObject(""), new TextureObject(""), new TextureObject(""), new TextureObject(""), new TextureObject("") };

            [SerializeField]
            private TextureObject[] m_secondaryTextures = new TextureObject[7] { new TextureObject(""), new TextureObject(""), new TextureObject(""), new TextureObject(""), new TextureObject(""), new TextureObject(""), new TextureObject("") };

            [SerializeField]
            private SkinDesigner.SkinSystem.MaterialMetallicMode m_mainTextureMmm;

            [SerializeField]
            private SkinDesigner.SkinSystem.MaterialMetallicMode m_secondaryTextureMmm;

            public SkinSystem.Weapon Weapon
            {
                get
                {
                    return m_weapon;
                }
                set
                {
                    m_weapon = value;
                }
            }

            public bool HasMultipleParts
            {
                get
                {
                    return m_hasMulipleParts;
                }
                set
                {
                    m_hasMulipleParts = value;
                }
            }

            public Weapon.WeaponObject CorrespondingObject
            {
                get
                {
                    return m_correspondingObject;
                }
                set
                {
                    m_correspondingObject = value;
                }
            }

            public TextureObject[] SecondaryTextures
            {
                get
                {
                    return m_secondaryTextures;
                }
                set
                {
                    m_secondaryTextures = value;
                }
            }

            public TextureObject[] MainTextures
            {
                get
                {
                    return m_mainTextures;
                }
                set
                {
                    m_mainTextures = value;
                }
            }

            public SkinDesigner.SkinSystem.MaterialMetallicMode MainTextureMaterialMetallicMode
            {
                get
                {
                    return m_mainTextureMmm;
                }
                set
                {
                    m_mainTextureMmm = value;
                }
            }

            public SkinDesigner.SkinSystem.MaterialMetallicMode SecondaryTextureMaterialMetallicMode
            {
                get
                {
                    return m_secondaryTextureMmm;
                }
                set
                {
                    m_secondaryTextureMmm = value;
                }
            }
        }
    }
}





// GITEUBE