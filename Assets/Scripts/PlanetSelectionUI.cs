using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSelectionUI
{
    // Updated per frame
    CelestialBody body;
    CelestialBody player;
    float relSpeed;
    float horizRelSpeed;
    float vertRelSpeed;

    // Updated on Init
    Texture arrowSquareTexture, arrowHeadTexture;
    RectTransform horizArrow, vertArrow;
    Camera cam;

    // Arrow Settings 
    const int fontSize = 25;
    const float arrowSquareDist = 40;
    const float arrowSquareSize = 10;
    const float arrowHeadSize = 20;
    const float arrowSquareInterval = 4f;

    GUIStyle style;

    public PlanetSelectionUI(Camera cam, Texture arrowSquareTexture, Texture arrowHeadTexture, RectTransform horizArrow, RectTransform vertArrow)
    {
        this.cam = cam;
        this.arrowSquareTexture = arrowSquareTexture;
        this.arrowHeadTexture = arrowHeadTexture;
        this.horizArrow = horizArrow;
        this.vertArrow = vertArrow;


        style = new GUIStyle
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = fontSize,
        };

        style.normal.textColor = Color.white;
    }

    public void Update(CelestialBody body, CelestialBody player, float relSpeed, float horizRelSpeed, float vertRelSpeed)
    {
        this.body = body;
        this.player = player;
        this.relSpeed = relSpeed;
        this.horizRelSpeed = horizRelSpeed;
        this.vertRelSpeed = vertRelSpeed;
    }

    public void DrawGUI()
    {
        Vector3 bodyScreenPos = cam.WorldToScreenPoint(body.transform.position);
        Vector3 rightEdgeOfBodyScreenPos = cam.WorldToScreenPoint(body.transform.position + body.radius * cam.transform.right);
        Vector3 topEdgeOfBodyScreenPos = cam.WorldToScreenPoint(body.transform.position + body.radius * cam.transform.up);

        Vector3 bodyScreenPosInverted = bodyScreenPos;
        Vector3 rightEdgeOfBodyScreenPosInverted = rightEdgeOfBodyScreenPos;
        Vector3 topEdgeOfBodyScreenPosInverted = topEdgeOfBodyScreenPos;

        // Fix inverted y
        bodyScreenPosInverted.y = Screen.height - bodyScreenPos.y;
        rightEdgeOfBodyScreenPosInverted.y = Screen.height - rightEdgeOfBodyScreenPos.y;
        topEdgeOfBodyScreenPosInverted.y = Screen.height - topEdgeOfBodyScreenPos.y;


        Rect bodyInfoRect = new Rect(rightEdgeOfBodyScreenPosInverted.x, (topEdgeOfBodyScreenPos.y + rightEdgeOfBodyScreenPosInverted.y) / 2, 300, 100);
        GUI.Label(bodyInfoRect, $"{body.name}\n{Vector3.Distance(body.transform.position, player.transform.position)} km \n{relSpeed:N1} m/s", style);

        // Draw Arrows

        // Horizontal Arrow
        float absRelH = Mathf.Abs(horizRelSpeed);
        float hMax = Mathf.Ceil(absRelH / arrowSquareInterval);

        float rectX;
        if (horizRelSpeed > 0) rectX = rightEdgeOfBodyScreenPos.x;
        else rectX = (bodyScreenPos - (rightEdgeOfBodyScreenPos - bodyScreenPos)).x;

        for (int i = 0; i < hMax; i++)
        {
            Rect horizArrowSquareRect = new Rect(rectX + (arrowSquareDist * i * Mathf.Sign(horizRelSpeed)) + arrowSquareSize / 2, bodyScreenPos.y - arrowSquareSize / 2, arrowSquareSize, arrowSquareSize);
            GUI.DrawTexture(horizArrowSquareRect, arrowSquareTexture);
        }

        bodyScreenPos = cam.WorldToScreenPoint(body.transform.position);
        rightEdgeOfBodyScreenPos = cam.WorldToScreenPoint(body.transform.position + body.radius * cam.transform.right);

        if (horizRelSpeed > 0) rectX = rightEdgeOfBodyScreenPos.x;
        else rectX = (bodyScreenPos - (rightEdgeOfBodyScreenPos - bodyScreenPos)).x;

        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) horizArrow.parent, bodyScreenPos, cam, out Vector2 canvasPos);

        horizArrow.anchoredPosition = new Vector2(rectX + (arrowSquareDist * (absRelH / arrowSquareInterval) * Mathf.Sign(horizRelSpeed)), canvasPos.y);
        horizArrow.rotation = Quaternion.Euler(0, 0, horizRelSpeed < 0 ? 180 : 0);
        //Rect horizArrowHeadRect = new Rect(rectX + (arrowSquareDist * (absRelH / arrowSquareInterval) * Mathf.Sign(horizRelSpeed)) + arrowHeadSize / 2, bodyScreenPos.y - arrowSquareSize / 2, arrowHeadSize, arrowHeadSize);
        //if (horizRelSpeed < 0) GUIUtility.RotateAroundPivot(180, new Vector2(horizArrowHeadRect.x, horizArrowHeadRect.y));
        //GUI.DrawTexture(horizArrowHeadRect, arrowHeadTexture);
        //GUI.EndGroup();
    }
}
