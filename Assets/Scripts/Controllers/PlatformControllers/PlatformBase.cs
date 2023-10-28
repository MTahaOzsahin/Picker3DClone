using UnityEngine;

namespace Controllers.PlatformControllers
{
    public enum PlatformType
    {
        Normal,
        CheckPoint,
        End
    }
    public class PlatformBase : MonoBehaviour
    {
        public PlatformType selectedPlatformType;
    }
}
