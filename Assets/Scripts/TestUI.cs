using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class TestUI : MonoBehaviour
    {
        [SerializeField] RectTransform horizArrow;
        [SerializeField] Camera cam;
        [SerializeField] Transform sphere;
        // Update is called once per frame
        void Update()
        {
            if (!CameraCanSeePoint(cam, sphere.position)) return;
            Debug.Log(cam.WorldToScreenPoint(sphere.position));
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)horizArrow.parent, cam.WorldToScreenPoint(sphere.position), null, out Vector2 canvasPos);
            horizArrow.anchoredPosition = new Vector2(canvasPos.x + 200, canvasPos.y);
        }

        bool CameraCanSeePoint(Camera cam, Vector3 point)
        {
            var vp = cam.WorldToViewportPoint(point);
            return !(vp.x > 1 || vp.x < 0 || vp.y > 1 || vp.y < 0 || vp.z < 0);
        }
    }
