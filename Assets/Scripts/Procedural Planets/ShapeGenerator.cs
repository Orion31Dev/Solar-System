using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;

    List<INoiseFilter> filters;

    public MinMax elevationMinMax;

    public void UpdateSettings(ShapeSettings settings)
    {
        this.settings = settings;
        filters = new List<INoiseFilter>();

        foreach (ShapeSettings.NoiseLayer layer in settings.noiseLayers)
        {
            if (layer.enabled)
            {
                INoiseFilter f = NoiseFilterFactory.CreateNoiseFilter(layer.noiseSettings);
                f.Settings.maskByLayer0 = layer.maskByLayer0;
                filters.Add(f);
            }
        }

        elevationMinMax = new MinMax();
    }

    public float CalculateUnscaledElevation(Vector3 pointOnUnitSphere)
    {
        float layer0Val = 0;
        float elevation = 0;

        if (filters.Count > 0)
        {
            layer0Val = filters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = layer0Val;
            }
        }

        foreach (INoiseFilter f in filters)
        {
            float mask = f.Settings.maskByLayer0 ? layer0Val : 1;
            elevation += f.Evaluate(pointOnUnitSphere) * mask;
        }

        elevationMinMax.AddValue(elevation);
        return elevation;
    }

    public float GetScaledElevation(float elevation)
    {
        elevation = Mathf.Max(0, elevation);
        elevation = settings.planetRadius * (1 + elevation);
        return elevation;
    }
}
