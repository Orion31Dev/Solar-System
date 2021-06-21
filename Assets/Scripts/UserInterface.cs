using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public Text velocityText;
    public Text speed;
    public Text speedTH;

    private static UserInterface instance;

    private void Awake()
    {
        instance = this;
    }

    public static void UpdateUI(Vector3 velocity, string target, float speed)
    {
        instance.velocityText.text = "Velocity: " + velocity;
        instance.speed.text = $"Speed to {target}: {speed}";
    }
}
