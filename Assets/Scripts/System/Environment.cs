using System;
using UnityEngine;

namespace SkinDesigner.SkinSystem
{
    public static class Environment
    {
        public static Weapon StringToWeapon(string _Value)
        {
            return (Weapon)Enum.Parse(typeof(Weapon), _Value);
        }

        public static string WeaponToString(Weapon _Value)
        {
            return _Value.ToString();
        }

        public static WeaponSight StringToWeaponSight(string _Value)
        {
            return (WeaponSight)Enum.Parse(typeof(WeaponSight), _Value);
        }

        public static string WeaponSightToString(WeaponSight _Value)
        {
            return _Value.ToString();
        }

        public static WeaponSilencer StringToWeaponSilencer(string _Value)
        {
            return (WeaponSilencer)Enum.Parse(typeof(WeaponSilencer), _Value);
        }

        public static string WeaponSilencerToString(WeaponSilencer _Value)
        {
            return _Value.ToString();
        }

        public static TextureMap StringToTextureMap(string _Value)
        {
            return (TextureMap)Enum.Parse(typeof(TextureMap), _Value);
        }

        public static string TextureMapToString(TextureMap _Value)
        {
            return _Value.ToString();
        }

        public static TextureMap IntToTextureMap(int _Value)
        {
            string[] _names = Enum.GetNames(typeof(TextureMap));
            return (TextureMap)Enum.Parse(typeof(TextureMap), _names[_Value]);
        }

        public static int TextureMapToInt(TextureMap _Value)
        {
            string[] _names = Enum.GetNames(typeof(TextureMap));
            int _output = 0;

            for (int i = 0; i < _names.Length; i++)
            {
                if (_names[i] == TextureMapToString(_Value))
                {
                    _output = i;
                    break;
                }
            }

            return _output;
        }

        public static Weapon IntToWeapon(int _Value)
        {
            string[] _names = Enum.GetNames(typeof(Weapon));
            return (Weapon)Enum.Parse(typeof(Weapon), _names[_Value]);
        }

        public static int WeaponToInt(Weapon _Value)
        {
            string[] _names = Enum.GetNames(typeof(Weapon));
            int _output = 0;

            for (int i = 0; i < _names.Length; i++)
            {
                if (_names[i] == WeaponToString(_Value))
                {
                    _output = i;
                    break;
                }
            }

            return _output;
        }

        public static string GetTextureMapRealName(TextureMap _Value)
        {
            switch (_Value)
            {
                case TextureMap.Albedo:
                    return "_MainTex";
                case TextureMap.Detail:
                    return "_DetailAlbedoMap";
                case TextureMap.Emission:
                    return "_EmissionMap";
                case TextureMap.Height:
                    return "_ParallaxMap";
                case TextureMap.Metallic:
                    return "_MetallicGlossMap";
                case TextureMap.Normal:
                    return "_BumpMap";
                case TextureMap.Occlusion:
                    return "_OcclusionMap";
                default:
                    Debug.LogError("The requested texture map does not exist.");
                    return "_MainTex";
            }
        }

        public static MaterialMetallicMode GetWeaponMMM(Weapon _Value)
        {
            switch (_Value)
            {
                case Weapon.AK12:
                    return MaterialMetallicMode.M;
                case Weapon.AK47:
                    return MaterialMetallicMode.M;
                case Weapon.AUG:
                    return MaterialMetallicMode.M;
                case Weapon.DAO:
                    return MaterialMetallicMode.M;
                case Weapon.Deagle:
                    return MaterialMetallicMode.M;
                case Weapon.G22:
                    return MaterialMetallicMode.M;
                case Weapon.K10:
                    return MaterialMetallicMode.M;
                case Weapon.KSG:
                    return MaterialMetallicMode.S;
                case Weapon.L96A1:
                    return MaterialMetallicMode.M;
                case Weapon.M107:
                    return MaterialMetallicMode.M;
                case Weapon.M110:
                    return MaterialMetallicMode.M;
                case Weapon.M4A1:
                    return MaterialMetallicMode.M;
                case Weapon.MG4:
                    return MaterialMetallicMode.S;
                case Weapon.MLG140:
                    return MaterialMetallicMode.S;
                case Weapon.P99:
                    return MaterialMetallicMode.M;
                case Weapon.USP45:
                    return MaterialMetallicMode.M;
                case Weapon.UZI:
                    return MaterialMetallicMode.S;
                default:
                    Debug.LogError("The requested weapon does not exist.");
                    return MaterialMetallicMode.M;
            }
        }

        public static MaterialMetallicMode GetWeaponSightMMM(WeaponSight _Value)
        {
            switch (_Value)
            {
                case WeaponSight.L96A1:
                    return MaterialMetallicMode.M;
                case WeaponSight.M107:
                    return MaterialMetallicMode.M;
                case WeaponSight.ACOG:
                    return MaterialMetallicMode.M;
                case WeaponSight.ACOGM:
                    return MaterialMetallicMode.S;
                case WeaponSight.REFLEX:
                    return MaterialMetallicMode.M;
                case WeaponSight.HOLO:
                    return MaterialMetallicMode.S;
                case WeaponSight.FLIR:
                    return MaterialMetallicMode.M;
                case WeaponSight.KOBRA:
                    return MaterialMetallicMode.M;
                case WeaponSight.MRDS:
                    return MaterialMetallicMode.M;
                case WeaponSight.FFMRDS:
                    return MaterialMetallicMode.M;
                case WeaponSight.PKAS:
                    return MaterialMetallicMode.M;
                case WeaponSight.RWRDS:
                    return MaterialMetallicMode.M;
                case WeaponSight.KSGIRON:
                    return MaterialMetallicMode.S;
                case WeaponSight.AK12IRON:
                    return MaterialMetallicMode.M;
                case WeaponSight.M4A1IRON:
                    return MaterialMetallicMode.M;
                case WeaponSight.K10IRON:
                    return MaterialMetallicMode.M;
                default:
                    Debug.LogError("The requested weapon sight does not exist.");
                    return MaterialMetallicMode.M;

            }
        }

        public static MaterialMetallicMode GetWeaponSilencerMMM(WeaponSilencer _Value)
        {
            switch (_Value)
            {
                case WeaponSilencer.SILENCER1:
                    return MaterialMetallicMode.M;
                case WeaponSilencer.SILENCER2:
                    return MaterialMetallicMode.S;
                case WeaponSilencer.SILENCER3:
                    return MaterialMetallicMode.S;
                default:
                    Debug.LogError("The requested silencer does not exist.");
                    return MaterialMetallicMode.M;
            }
        }

        public static Texture2D[] GetMaterialTextures(Material _Value)
        {
            Texture2D[] _output = new Texture2D[7];

            _output[0] = (Texture2D)_Value.GetTexture(GetTextureMapRealName(TextureMap.Albedo));
            _output[1] = (Texture2D)_Value.GetTexture(GetTextureMapRealName(TextureMap.Detail));
            _output[2] = (Texture2D)_Value.GetTexture(GetTextureMapRealName(TextureMap.Emission));
            _output[3] = (Texture2D)_Value.GetTexture(GetTextureMapRealName(TextureMap.Height));
            _output[4] = (Texture2D)_Value.GetTexture(GetTextureMapRealName(TextureMap.Metallic));
            _output[5] = (Texture2D)_Value.GetTexture(GetTextureMapRealName(TextureMap.Normal));
            _output[6] = (Texture2D)_Value.GetTexture(GetTextureMapRealName(TextureMap.Occlusion));
        
            return _output;
        }

        public static Material SetMaterialTextures(Material value, Texture[] textures)
        {
            Material output = value;

            if (textures.Length > 0) output.SetTexture(GetTextureMapRealName(TextureMap.Albedo), textures[0]);
            if (textures.Length > 1) output.SetTexture(GetTextureMapRealName(TextureMap.Detail), textures[1]);
            if (textures.Length > 2) output.SetTexture(GetTextureMapRealName(TextureMap.Emission), textures[2]);
            if (textures.Length > 3) output.SetTexture(GetTextureMapRealName(TextureMap.Height), textures[3]);
            if (textures.Length > 4) output.SetTexture(GetTextureMapRealName(TextureMap.Metallic), textures[4]);
            if (textures.Length > 5) output.SetTexture(GetTextureMapRealName(TextureMap.Normal), textures[5]);
            if (textures.Length > 6) output.SetTexture(GetTextureMapRealName(TextureMap.Occlusion), textures[6]);

            return value;
        }

    }
}