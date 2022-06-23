using System;
using UnityEngine;

namespace SkinDesigner.SkinSystem
{
    public enum Weapon
    {
        AK12,
        AK47,
        AUG,
        DAO,
        Deagle,
        G22,
        K10,
        KSG,
        L96A1,
        M4A1,
        M107,
        M110,
        MG4,
        MLG140,
        P99,
        USP45,
        UZI
    }

    public enum WeaponSight
    {
        L96A1,
        M107,
        ACOG,
        ACOGM,
        REFLEX,
        HOLO,
        FLIR,
        KOBRA,
        MRDS,
        FFMRDS,
        PKAS,
        RWRDS,
        KSGIRON,
        AK12IRON,
        M4A1IRON,
        K10IRON
    }

    public enum WeaponSilencer
    {
        SILENCER1,
        SILENCER2,
        SILENCER3
    }

    


    public enum MaterialMetallicMode
    {
        [InspectorName("Metallic")]M,
        [InspectorName("Specular")]S
    }
}