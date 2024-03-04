using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private GameObject _objectToDestroy;

        public void DestroyObject()
        {

            Destroy(_objectToDestroy);
        }
    }
}