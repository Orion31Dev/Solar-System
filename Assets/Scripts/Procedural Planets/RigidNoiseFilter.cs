using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    Noise noise = new Noise();
    public NoiseSettings Settings { get; set; }

    public RigidNoiseFilter(NoiseSettings settings)
    {
        Settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float val = 0;

        float freq = Settings.baseRoughness;
        float amp = 1;

        float weight = 1;

        for (int i = 0; i < Settings.octaves; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * freq + Settings.center));

            v *= v; // Square v to get sharper peaks.
            v *= weight * Settings.weightPersistance;

            weight = v; // Make noise in ridges more detailed than valleys below

            val += v * amp; // V is already in range (0 to 1) b/c of abs

            freq *= Settings.roughness;
            amp *= Settings.persistance;
        }

        val -= Settings.minVal;

        return val * Settings.strength;
    }
}
