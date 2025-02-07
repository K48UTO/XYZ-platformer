using UnityEngine;

namespace Scripts.Components
{
    public class ParentingControlComponent : MonoBehaviour
    {
        public void SetAsParent(GameObject child)
        {
            if (child != null)
            {
                child.transform.SetParent(transform);
            }
        }

        public void RemoveParent(GameObject child)
        {
            if (child != null && child.transform.parent == transform)
            {
                child.transform.SetParent(null);
            }
        }
    }
}
