using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ColorSettings : ScriptableObject
{
    public Material planetMaterial;

    public BiomeColorSettings biomeSettings;
    public Gradient oceanGradient;

    [System.Serializable]
    public class BiomeColorSettings
    {
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;

        [Range(0, 1)]
        public float blendAmt;

        public Biome[] biomes;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;

            public Color tint;

            [Range(0, 1)]
            public float startHeight;

            [Range(0, 1)]
            public float tintPercent;
        }
    }
}
