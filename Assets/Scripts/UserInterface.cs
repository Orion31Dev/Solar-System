using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public Text velocityText;
    public Text speedSun;
    public Text speedTH;

    private static UserInterface instance;

    private void Awake()
    {
        instance = this;
    }

    public static void UpdateUI(Vector3 velocity, object speedSun, object speedTH)
    {
        instance.velocityText.text = "Velocity: " + velocity;
        instance.speedSun.text = "Speed to Sun: " + speedSun;
        instance.speedTH.text = "Speed to Timber Hearth: " + speedTH;
    }
}
