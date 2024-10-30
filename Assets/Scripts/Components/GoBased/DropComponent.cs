using UnityEngine;

[System.Serializable]
public class DropItem
{
    public GameObject prefab; 
    public int dropChance; // Вероятность выпадения этого предмета
    public int minQuantity = 1; // Минимальное количество выпадаемых предметов
    public int maxQuantity = 1; // Максимальное количество выпадаемых предметов
}

namespace Scripts.Components.GoBased
{
    public class DropComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target; // Место, где будут появляться предметы
        [SerializeField] private DropItem[] _dropItems; // Массив предметов для выпадения
        [SerializeField] private float throwForce = 5f; // Сила, с которой объекты будут выбрасываться
        [SerializeField] private float throwAngle = 45f; // Угол конуса выброса в градусах

        [ContextMenu("Drop")]
        public void Drop()
        {
            Debug.Log("Метод вызван");

            foreach (var dropItem in _dropItems)
            {
                if (Random.Range(0, 101) < dropItem.dropChance) // Проверяем, выпадет ли этот предмет
                {
                    int quantity = Random.Range(dropItem.minQuantity, dropItem.maxQuantity + 1); // Генерируем случайное количество предметов
                    for (int j = 0; j < quantity; j++) // Создаем указанное количество предметов
                    {
                        var instantiate = Instantiate(dropItem.prefab, _target.position, Quaternion.identity);

                        // Вычисляем направление выброса в пределах конуса
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