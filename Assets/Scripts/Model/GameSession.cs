
using System.Collections;
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
                Destroy(gameObject);

            }
            else
            {
                Save();
                DontDestroyOnLoad(this);
          
            }
        }

        public void Save()
        {
            Debug.Log("Save");
            _savedData = _data.Clone();
        }

        public void Load()
        {
            Debug.Log("Load");
            _data = _savedData.Clone();
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
