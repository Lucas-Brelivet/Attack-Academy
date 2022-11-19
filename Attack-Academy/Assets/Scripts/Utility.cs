using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public enum MagicType
    {
        Fire,
        Ice,
        Poison,
        Earth
    };

    public enum EnemyState
    {
        Attack,
        ApprochToAttack,
        StayAtDistance
    }

    public enum SpellType
    {
        Zone,
        Bullet, 
        Cone
    }
}
