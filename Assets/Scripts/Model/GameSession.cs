
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
            Debug.Log("Save" + _savedData.HP);
            _savedData = _data.Clone();
        }

        public void Load()
        {
            Debug.Log("Метод зазгрузки вызван");
            Debug.Log($"Было Хп в _data на момент вызова метода: {_data.HP}");
            _data = _savedData.Clone();
            Debug.Log($"Стало Хп в _data {_data.HP}");
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
