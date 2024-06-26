
using Scripts.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload ()
        {
            var session = FindObjectOfType<GameSession>();
     

            var scene = SceneManager.GetActiveScene();
            
            SceneManager.LoadScene(scene.name);
            session.Load();

        }
    }
}

