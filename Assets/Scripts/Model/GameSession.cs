
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
                
                DontDestroyOnLoad(this);
          
            }
        }

        private void Start()
        {
            StartCoroutine(CallMethodWithDelay(2f));
        }

        public void Save ()
        {
            Debug.Log("Save" + _savedData.HP);
            _savedData = _data.Clone();
        }

        public void Load()
        {
            Debug.Log($"Loading data: HP before = {_data.HP}");
            _data = _savedData.Clone();
            Debug.Log($"HP after = {_data.HP}");
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

        IEnumerator CallMethodWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Save();
        }
    }

}
