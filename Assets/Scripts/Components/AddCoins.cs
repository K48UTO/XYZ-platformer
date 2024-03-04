using UnityEngine;

namespace Scripts.Components
{
    public class AddCoins : MonoBehaviour
    {
        [SerializeField]
        private int coinsToAdd = 1; // ���������� �����, ������� ����� ���������. ��� �������� ����� ����������� � ���������� Unity.

        // ���������� ����� ��� �������� GameObject
        public void AddCoinsToHero(GameObject heroObject)
        {
            // �������� ����� ��������� Hero �� ���������� �������.
            Hero hero = heroObject.GetComponent<Hero>();

            // ���� ��������� Hero ������
            if (hero != null)
            {
                // �������� ����� AddCoin � �������� ���������� �����.
                hero.AddCoin(coinsToAdd);
            }
        }
    }
}
