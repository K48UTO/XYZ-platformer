using Scripts.Creatures;
using UnityEngine;

public class SuperJumpEffect : MonoBehaviour
{
    [SerializeField] private int _value;

    public void GetSuperJump(GameObject target)
    {
        var SuperJump = target.GetComponent<Hero>();
        if (SuperJump != null)
        {
            SuperJump.SuperJumpPotion(_value);
        }
    }
}
