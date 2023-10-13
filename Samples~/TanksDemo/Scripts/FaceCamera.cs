using UnityEngine;

namespace Agava.MirrorServerApi.Samples.TanksDemo
{
    public class FaceCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.forward = _mainCamera.transform.forward;
        }
    }
}
