using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSettings settings;
    Texture2D texture;
    const int textureRes = 50;

    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        if (texture == null || texture.height != settings.biomeSettings.biomes.Length)
        {
            texture = new Texture2D(textureRes * 2, settings.biomeSettings.biomes.Length, TextureFormat.RGBA32, false);
        }

        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;

        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeSettings.noiseOffset) * settings.biomeSettings.noiseStrength;

        float biomeIndex = 0;
        float blendRange = settings.biomeSettings.blendAmt / 2f + 0.001f;

        for (int i = 0; i < settings.biomeSettings.biomes.Length; i++)
        {
            float dst = heightPercent - settings.biomeSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);

            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(settings.biomeSettings.biomes.Length - 1, 1);
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];
        int colorIndex = 0;
        foreach (var biome in settings.biomeSettings.biomes)
        {
            for (int i = 0; i < textureRes * 2; i++)
            {
                Color gradientColor;
                if (i < textureRes)
                {
                    gradientColor = settings.oceanGradient.Evaluate(i / (textureRes - 1f));
                }
                else
                {
                    gradientColor = biome.gradient.Evaluate((i - textureRes)  / (textureRes - 1f));
                    
                }

                Color tintColor = biome.tint;
                colors[colorIndex] = gradientColor * (1 - biome.tintPercent) + tintColor * biome.tintPercent;
                colorIndex++;
            }
        }


        texture.SetPixels(colors);
        texture.Apply();

        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
