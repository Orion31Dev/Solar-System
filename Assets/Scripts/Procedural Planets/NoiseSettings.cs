using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NoiseSettings
{
    public FilterType type;

    public float strength = 1;

    public float baseRoughness = 1;
    public float roughness = 2;

    public float persistance = .5f;

    [Range(0, 1)]
    public float weightPersistance = 1;

    public float minVal = 0;


    [Range(1, 8)]
    public int octaves;

    [HideInInspector]
    public bool maskByLayer0;

    public Vector3 center;
}

public enum FilterType
{
    Simple, Rigid
}