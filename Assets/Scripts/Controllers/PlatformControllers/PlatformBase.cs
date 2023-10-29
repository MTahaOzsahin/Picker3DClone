using UnityEngine;

namespace Controllers.PlatformControllers
{
    public enum PlatformType
    {
        Normal,
        CheckPoint
    }
    public class PlatformBase : MonoBehaviour
    {
        public PlatformType selectedPlatformType;
    }
}
