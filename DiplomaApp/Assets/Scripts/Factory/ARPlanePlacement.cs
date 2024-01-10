using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

namespace Diploma.Factory

{    
    [RequireComponent(typeof(ARPlaneManager))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class ARPlanePlacement : MonoBehaviour
    {
        private ARPlaneManager arPlaneManager;
        private ARRaycastManager arRaycastManager;
        private static List<ARRaycastHit> hits = new List<ARRaycastHit>();


        void Start(){
            // plane_detected = false;
            arPlaneManager = GetComponent<ARPlaneManager>();
            arRaycastManager = GetComponent<ARRaycastManager>();

            arPlaneManager.planesChanged += PlaneChanged;
        }

           bool TryGetTouchPosition(out Vector2 touchPosition)
            {
                if(Input.touchCount > 0)
                {
                    touchPosition = Input.GetTouch(0).position;
                    return true;
                }

                touchPosition = default;

                return false;
            }


        private void PlaneChanged(ARPlanesChangedEventArgs args)
        {
            if(args.added != null && Factory_Program.state == State.AR_PLANE_DETECTION)
            {
                ARPlane arPlane = args.added[0];
                Factory_Program.placeOrigo = arPlane.transform.position;
                Factory_Program.logging = " ar position" + arPlane.transform.position;
                Factory_Program.state = State.OBJECT_PLACEMENT;
                // TogglePlaneDetection();
            }
        }

        void Update()
        {
            if(!TryGetTouchPosition(out Vector2 touchPosition))
            {
                return;
            }
            if(touchPosition.y < Screen.height * 0.2f)  //ahova a gombok kerülnek ott ne figyelje
            {
                return;
            }
            if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon) && Factory_Program.state == State.OBJECT_PLACEMENT)
            {
                var hitPose = hits[0].pose;
                Factory_Program.placeOrigo = hitPose.position;
            }
        }


        void SetAllPlanesActive(bool value)
        {
            foreach (var plane in arPlaneManager.trackables)
                plane.gameObject.SetActive(value);
        }

        public void TogglePlaneDetection()
        {
            arPlaneManager.enabled = !arPlaneManager.enabled;
            if (arPlaneManager.enabled)
            {
                SetAllPlanesActive(true);
            }
            else
            {
                SetAllPlanesActive(false);
            }
        }
    }
}
