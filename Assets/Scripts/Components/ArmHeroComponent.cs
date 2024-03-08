using Scripts.Creatures;
using UnityEngine;

namespace Scripts.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {
        public void ArmHero (GameObject go)
        {
            var hero  = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.ArmHero();
            }
        }
  
    }
}

