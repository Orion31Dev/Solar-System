using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory {
    public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
    {
        switch (settings.type)
        {
            case FilterType.Simple:
                return new SimpleNoiseFilter(settings);
            case FilterType.Rigid:
                return new RigidNoiseFilter(settings);
        }

        return null;
    } 
}
