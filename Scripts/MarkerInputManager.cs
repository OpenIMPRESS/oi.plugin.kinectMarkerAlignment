using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using oi.plugin.transform;
#if !UNITY_EDITOR && UNITY_METRO
using HoloToolkit.Unity.InputModule;
#endif

namespace oi.plugin.kinectMarkerAlignment {

    public class MarkerInputManager : MonoBehaviour
#if !UNITY_EDITOR && UNITY_METRO
        , IInputClickHandler 
#endif
        {

        TransformSender transformSender;
        private bool measuring = false;
        private bool gazeOn = false;


        // Use this for initialization
        void Start() {
            transformSender = GetComponent<TransformSender>();
        }

        // Update is called once per frame
        void Update() {

            if (transformSender.IsMeasuring()) {
                if (!measuring) {
                    measuring = true;
                    SetColor(new Color(255, 0, 0));
                }
            } else if (measuring) {
                measuring = false;
                if(gazeOn)
                    SetColor(new Color(0, 255, 0));
                else
                    SetColor(new Color(220, 220, 220));
            }

        }

        void OnGazeIn() {
            gazeOn = true;
            if (!measuring)
                SetColor(new Color(0, 255, 0));
        }

        void OnGazeOut() {
            gazeOn = false;
            if (!measuring)
                SetColor(new Color(220, 220, 220));
        }

        void SetColor(Color c) {
            foreach (Transform child in transform) {
                if (child.name.Contains("MarkerBorder"))
                    child.GetComponent<Renderer>().material.color = c;
            }
        }

#if !UNITY_EDITOR && UNITY_METRO
        void IInputClickHandler.OnInputClicked(InputClickedEventData eventData) {
            transformSender.MeasureAndSend();
        }
#endif
    }
}