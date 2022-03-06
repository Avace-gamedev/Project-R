using System;
using System.Linq;
using System.Reflection;
using Avace.Backend.Interfaces.Logging;
using Avace.Backend.Kernel.Injection;
using Cinemachine;
using Input;
using Input.Attributes;
using Misc;
using Unity.Mathematics;
using UnityEngine;

namespace Camera
{
    public class CameraController : CustomMonoBehaviour
    {
        public static CameraController Instance => Injector.TryGet<CameraController>();

        public CinemachineVirtualCamera vCam;

        [Range(0, 5f)]
        public float zoomSpeed = 1f;

        [Range(1f, 20f)]
        public float minZoom = 5f;

        [Range(10f, 50f)]
        public float maxZoom = 15f;

        protected override void RegisterInjectionBindings()
        {
            Injector.BindSingleton(this);
        }

        protected override void UnregisterInjectionBindings()
        {
            Injector.RemoveAll<CameraController>();
            
        }

        private void Start()
        {
            if (!vCam)
            {
                throw new InvalidOperationException($"No {typeof(CinemachineVirtualCamera)} provided");
            }
        }

        /// <summary>
        /// Diff must be between -1 and 1.
        /// </summary>
        public void Zoom(float diff)
        {
            if (!vCam)
            {
                return;
            }
            
            float diffClamped = math.clamp(diff, -1, 1);
            float newSize = vCam.m_Lens.OrthographicSize - diffClamped * zoomSpeed; 
            vCam.m_Lens.OrthographicSize = math.clamp(newSize, minZoom, maxZoom);
        }

        [RegisterInputCallback(InputType.CameraZoom)]
        public static void OnCameraZoom(CameraZoomInput input)
        {
            Instance.Zoom(input.Value);
        }

        private void OnValidate()
        {
            if (vCam)
            {
                return;
            }

            CinemachineVirtualCamera[] vCams = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
            switch (vCams.Length)
            {
                case 0:
                    return;
                case 1:
                    vCam = vCams.Single();
                    break;
                default:
                    Debug.LogWarning(
                        $"Found multiple {typeof(CinemachineVirtualCamera)}, cannot choose. Please specify a virtual camera for the camera controller.");
                    break;
            }
        }
    }
}