using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseFilter
{
    NoiseSettings Settings { get; set; }

    public float Evaluate(Vector3 point);
}
