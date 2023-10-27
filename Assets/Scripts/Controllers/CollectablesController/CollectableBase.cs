using UnityEngine;

namespace Controllers.CollectablesController
{
    public enum CollectableType
    {
        Sphere,
        Cube,
        Capsule
    }
    public class CollectableBase : MonoBehaviour
    {
        public bool IsActive;
        public CollectableType SelectedCollectableType;
    }
}
