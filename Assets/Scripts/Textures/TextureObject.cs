using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkinDesigner.Textures
{
    [System.Serializable]
    public class TextureObject
    {
        [SerializeField]
        private string m_texturePath = "NULL";

        [SerializeField]
        private Texture m_texture = null;

        public string TexturePath
        {
            get
            {
                return m_texturePath;
            }

            set
            {
                m_texturePath = value.Replace("\\", "/");
            }
        }

        public Texture Texture
        {
            get
            {
                return m_texture;
            }

            set
            {
                m_texture = value;
            }
        }

        public Texture GetTextureFromPath()
        {
            if (string.IsNullOrEmpty(m_texturePath) || string.IsNullOrWhiteSpace(m_texturePath))
            {
                Debug.LogError("TextureObject has no texture path selected (null)");
                return null;
            }
            else
            {

                Texture2D _generated = new Texture2D(2, 2);
                byte[] _bytes = System.IO.File.ReadAllBytes(m_texturePath);
                _generated.LoadImage(_bytes);
                _generated.Apply();

                this.m_texture = _generated;
                return _generated;
            }
        }

        public TextureObject()
        {
            this.m_texturePath = string.Empty;
            this.m_texture = null;
        }

        public TextureObject(string _TexturePath)
        {
            this.m_texturePath = _TexturePath.Replace("\\", "/");
        }

        public TextureObject(Texture _Texture)
        {
            this.m_texture = _Texture;
        }

        public TextureObject(Texture _Texture, string _TexturePath)
        {
            this.m_texturePath = _TexturePath.Replace("\\", "/");
            this.m_texture = _Texture;
        }
    }
}