
using Scripts.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
      
        public void ReloadOnDie()
        {
            var session = FindObjectOfType<GameSession>();
            session.GetComponent<GameSession>().IfDie();
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        public void Reload ()
        {
            var session = FindObjectOfType<GameSession>();
            DestroyImmediate(session);

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            
        }
    }
}

