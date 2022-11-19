using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerSpell
{
    public Utility.MagicType magicType;
    public Utility.SpellType spellType;
    public int costMult;
    public int powerMult;
    public float distMax;
    public int cost { get; set; }
    public int power { get; set; }
}



public class PlayerPower : MonoBehaviour
{
    Player player;
    [SerializeField] PlayerSpell[] playerSpells;
    [SerializeField] private float cooldown;
    private bool canAttack = true;

    private Controls controls;

    private void Start()
    {
        player = GetComponent<Player>();
        controls = new Controls();
        controls.Player.Enable();
        controls.Player.Attack1.performed += AttackOne;
        controls.Player.Attack2.performed += AttackTwo;
        controls.Player.Attack3.performed += AttackThree;
    }

    private void Update()
    {
        UpdateCostAndPower();

    }


    void UpdateCostAndPower()
    {
        foreach(var playerSpell in playerSpells)
        {
            playerSpell.cost = 1 + (int)Mathf.Min(player.minDistToStele[playerSpell.magicType],playerSpell.distMax) * playerSpell.costMult;
            playerSpell.power = (int)Mathf.Min(player.minDistToStele[playerSpell.magicType], playerSpell.distMax) * playerSpell.powerMult;
        }
    }

    void AttackOne(InputAction.CallbackContext context)
    {
        Attack(0);
    }

    void AttackTwo(InputAction.CallbackContext context)
    {
        Attack(1);
    }

    void AttackThree(InputAction.CallbackContext context)
    {
        Attack(2);
    }

    void Attack(int number)
    {
        if (canAttack)
        {
            var spells = playerSpells.Where(x => x.magicType == player.currentMagicType);
            if (number < playerSpells.Length)
            {
                var spell = spells.ElementAtOrDefault(number);
                if (spell != null)
                {
                    //doattack
                    print(spell.cost);
                    Cooldown();
                }
            }          
        }
    }

    IEnumerator Cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
