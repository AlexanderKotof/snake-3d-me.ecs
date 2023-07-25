using UnityEngine;

namespace Game.Camera
{
    public class CameraAspect : MonoBehaviour
    {
        public Vector3 landScapePosition;
        public Vector3 portaitPosition;

        void Update()
        {
            var orientation = Screen.orientation;
            if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight)
            {
                transform.position = landScapePosition;
            }
            else
            {
                transform.position = portaitPosition;
            }
        }
    }
}