using UnityEngine;


namespace Photon.Chat.DemoChat.Utilities
{
    // small script to avoid clicks picking inactive UI elements
    public class IgnoreUiRaycastWhenInactive : MonoBehaviour, ICanvasRaycastFilter
    {
        public bool IsRaycastLocationValid(Vector2 screenPoint, UnityEngine.Camera eventCamera)
        {
            return gameObject.activeInHierarchy;
        }
    }
}