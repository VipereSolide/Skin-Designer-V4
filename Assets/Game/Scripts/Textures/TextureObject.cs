using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SkinDesigner.Textures
{
    [System.Serializable]
    public class TextureObject
    {
        [SerializeField] private string m_texturePath = "NULL";
        [SerializeField] private Texture m_texture = null;
        [SerializeField] private FileInfo m_textureFileInfo = null;

        public FileInfo TextureFileInfo { get => m_textureFileInfo; set => m_textureFileInfo = value; }
        public string TexturePath { get => m_texturePath; set => m_texturePath = value.Replace("\\", "/"); }
        public Texture Texture { get => m_texture; set => m_texture = value; }

        public Texture GetTextureFromPath()
        {
            bool isTextureNull = string.IsNullOrEmpty(m_texturePath) || string.IsNullOrWhiteSpace(m_texturePath) || m_texturePath == "NULL";
            if (isTextureNull)
            {
                if (m_texturePath != "NULL")
                {
                    Debug.LogError("TextureObject has no texture path selected (null)");
                }

                return null;
            }
            else
            {
                Texture2D textureObject = new Texture2D(2, 2);
                byte[] readBytes = File.ReadAllBytes(m_texturePath);
                textureObject.LoadImage(readBytes);
                textureObject.wrapMode = TextureWrapMode.Repeat;
                textureObject.filterMode = FilterMode.Point;
                textureObject.Apply();
                this.m_texture = textureObject;

                return textureObject;
            }
        }
        public FileInfo GetFileInfo()
        {
            this.m_textureFileInfo = new FileInfo(m_texturePath);
            return this.m_textureFileInfo;
        }
        public TextureObject()
        {
            this.m_texturePath = string.Empty;
            this.m_texture = null;
        }
        public TextureObject(string _TexturePath)
        {
            this.m_texturePath = _TexturePath.Replace("\\", "/");
    
            if (!string.IsNullOrEmpty(_TexturePath))
            {
                this.m_textureFileInfo = new FileInfo(m_texturePath);
            }
        }
        public TextureObject(Texture _Texture)
        {
            this.m_texture = _Texture;
        }
        public TextureObject(Texture _Texture, string _TexturePath)
        {
            this.m_texturePath = _TexturePath.Replace("\\", "/");
            this.m_texture = _Texture;
            this.m_textureFileInfo = new FileInfo(m_texturePath);
        }
        public bool HasTextureFileChanged(bool autoUpdate = true)
        {
            if (m_textureFileInfo == null)
            {
                return false;
            }

            FileInfo fileRecentInfo = new FileInfo(m_texturePath);
            bool hasChanged = fileRecentInfo.LastWriteTime != m_textureFileInfo.LastWriteTime;

            if (autoUpdate)
            {
                GetFileInfo();
            }

            return hasChanged;
        }

        public bool IsNull()
        {
            return string.IsNullOrEmpty(m_texturePath) || !File.Exists(m_texturePath);
        }
    }
}