
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Model
{
    public class GameSession : MonoBehaviour
    {

        [SerializeField] private PlayerData _data;
        [SerializeField] private PlayerData _savedData;
       

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
     
            _savedData = _data;
        }

        public void SaveData ()
        {
            _savedData = _data;
        }

        public void LoadData ()
        {
            _data = _savedData;
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
