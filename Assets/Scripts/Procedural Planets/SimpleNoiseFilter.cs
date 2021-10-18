using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    Noise noise = new Noise();

    public NoiseSettings Settings { get; set; }

    public SimpleNoiseFilter(NoiseSettings settings)
    {
        Settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float val = 0;

        float freq = Settings.baseRoughness;
        float amp = 1;

        for (int i = 0; i < Settings.octaves; i++)
        {
            float v = noise.Evaluate(point * freq + Settings.center);
            val += (v + 1) * .5f * amp; // Map val from (-1 to 1) to (0, amp)

            freq *= Settings.roughness;
            amp *= Settings.persistance;
        }

        val -= Settings.minVal;

        return val * Settings.strength;
    }
}
