using Scripts.Creatures;
using UnityEngine;

namespace Scripts.Components.Collectable
{
    public class ArmHeroComponent : MonoBehaviour
    {
        public void ArmHero (GameObject go)
        {
            var hero  = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.AddSword(1);
                Debug.Log("оепедюмн левеи 1");
            }
        }
  
    }
}

