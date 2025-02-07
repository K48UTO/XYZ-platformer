using Scripts.Creatures;
using UnityEngine;

namespace Scripts.Components.Collectable
{
    public class AddCoins : MonoBehaviour
    {
        [SerializeField]
        private int coinsToAdd = 1; 
        public void AddCoinsToHero(GameObject heroObject)
        {
            Hero hero = heroObject.GetComponent<Hero>();

            if (hero != null)
            {
                hero.AddCoin(coinsToAdd);
            }
        }
    }
}
