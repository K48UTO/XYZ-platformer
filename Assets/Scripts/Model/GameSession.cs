
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Model
{
    public class GameSession : MonoBehaviour
    {

        [SerializeField] private PlayerData _data;

        [SerializeField] int onLvlStartCoins;
        [SerializeField] int onLvlStartHP;
        [SerializeField] bool onLvlStartIsArmed;

        public PlayerData Data => _data;
        private void Awake()
        {
            if (IsSessionExist())
            {
                DestroyImmediate(gameObject);

            }
            else
            {
                DontDestroyOnLoad(this);
          
            }
        }

        private void Start()
        {
            onLvlStartCoins = _data.Coins;
            onLvlStartHP = _data.HP;
            onLvlStartIsArmed = _data.IsArmed;
        }

        public void IfDie()
        {
            _data.Coins = onLvlStartCoins;
            _data.HP = onLvlStartHP;
            _data.IsArmed = onLvlStartIsArmed;
        }
        private bool IsSessionExist()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                {
                    return true;
                }

            }
            return false;
        }
    }

}
