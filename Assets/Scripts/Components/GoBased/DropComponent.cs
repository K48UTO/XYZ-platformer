using UnityEngine;

[System.Serializable]
public class DropItem
{
    public GameObject prefab; 
    public int dropChance; 
    public int minQuantity = 1; 
    public int maxQuantity = 1;
}

namespace Scripts.Components.GoBased
{
    public class DropComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target; 
        [SerializeField] private DropItem[] _dropItems; 
        [SerializeField] private float throwForce = 5f; 
        [SerializeField] private float throwAngle = 45f; 
        [ContextMenu("Drop")]
        public void Drop()
        {
            foreach (var dropItem in _dropItems)
            {
                if (Random.Range(0, 101) < dropItem.dropChance) 
                {
                    int quantity = Random.Range(dropItem.minQuantity, dropItem.maxQuantity + 1);
                    for (int j = 0; j < quantity; j++) 
                    {
                        var instantiate = Instantiate(dropItem.prefab, _target.position, Quaternion.identity);

                        float angle = Random.Range(-throwAngle, throwAngle);
                        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

                        if (instantiate.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                        {
                            rb.velocity = direction * throwForce;
                        }

                        instantiate.transform.localScale = _target.lossyScale;
                    }
                }
            }
        }
    }

}