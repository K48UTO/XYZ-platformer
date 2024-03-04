using UnityEngine;

namespace Scripts.Components
{
    public class AddCoins : MonoBehaviour
    {
        [SerializeField]
        private int coinsToAdd = 1; // Количество монет, которое будет добавлено. Это значение можно настраивать в инспекторе Unity.

        // Измененный метод для принятия GameObject
        public void AddCoinsToHero(GameObject heroObject)
        {
            // Пытаемся найти компонент Hero на переданном объекте.
            Hero hero = heroObject.GetComponent<Hero>();

            // Если компонент Hero найден
            if (hero != null)
            {
                // Вызываем метод AddCoin и передаем количество монет.
                hero.AddCoin(coinsToAdd);
            }
        }
    }
}
