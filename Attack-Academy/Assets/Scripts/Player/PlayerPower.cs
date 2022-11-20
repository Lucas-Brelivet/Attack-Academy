using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

[System.Serializable]
public class PlayerSpell
{
    public Utility.MagicType magicType;
    public Utility.SpellType spellType;
    public float costMult;
    public float costBase;
    public float powerMult;
    public float distMax;
    public float cost { get; set; }
    public float power { get; set; }
}



public class PlayerPower : MonoBehaviour
{
    Player player;
    Animator animator;
    [SerializeField] public PlayerSpell[] playerSpells;
    [SerializeField] private float cooldownAttack;

    [SerializeField] private float durationConeSpell;
    [SerializeField] GameObject cone;

    [SerializeField] private float durationZoneSpell;
    [SerializeField] GameObject zone;

    private bool canAttack = true;

    private Controls controls;


    private void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        controls = new Controls();
        controls.Player.Enable();
        controls.Player.Attack1.performed += AttackOne;
        controls.Player.Attack2.performed += AttackTwo;
        controls.Player.Attack3.performed += AttackThree;
        cone.SetActive(false);
        zone.SetActive(false);
    }

    private void Update()
    {
        UpdateCostAndPower();
    }


    void UpdateCostAndPower()
    {
        foreach(var playerSpell in playerSpells)
        {
            playerSpell.cost = playerSpell.costBase + Mathf.Min(player.minDistToStele[playerSpell.magicType],playerSpell.distMax) * playerSpell.costMult;
            playerSpell.power = Mathf.Min(player.minDistToStele[playerSpell.magicType], playerSpell.distMax) * playerSpell.powerMult;
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
            if (number < playerSpells.Length )
            {
                var spell = spells.ElementAtOrDefault(number);
                if (spell != null && player.mana >= spell.cost)
                {
                    if (spell.spellType == Utility.SpellType.Cone)
                    {
                        ConeAttack(spell.power, spell.magicType);
               
                        player.ConsumeMana(spell.cost);
                    }
                    else if (spell.spellType == Utility.SpellType.Zone)
                    {
                      
                        player.ConsumeMana(spell.cost);
                        ZoneAttack(spell.power, spell.magicType);
                    }

                    AnimateCharacter();

                    StartCoroutine(Cooldown());
                }
            }          
        }
    }

    void AnimateCharacter()
    {
        Utility.Direction direction;
        Vector2 mouseDirection = (Camera.main.ScreenToWorldPoint(controls.Player.MousePosition.ReadValue<Vector2>()) - transform.position);

        if(Mathf.Abs(mouseDirection.x) > Mathf.Abs(mouseDirection.y))
        {
            direction = mouseDirection.x > 0 ? Utility.Direction.Right : Utility.Direction.Left;
        }
        else
        {
            direction = mouseDirection.y > 0 ? Utility.Direction.Up : Utility.Direction.Down;
        }

        switch(direction)
            {
                case Utility.Direction.Down:
                    animator.SetTrigger("AttackDown");
                    break;
                case Utility.Direction.Left:
                    animator.SetTrigger("AttackLeft");
                    break;
                case Utility.Direction.Up:
                    animator.SetTrigger("AttackUp");
                    break;
                case Utility.Direction.Right:
                    animator.SetTrigger("AttackRight");
                    break;
            }
    }

    IEnumerator Cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldownAttack);
        canAttack = true;
    }

    IEnumerator CooldownCone()
    {
        Player.Instance.attacking = true;
        Player.Instance.agent.isStopped = true;
        Player.Instance.controls.Player.Move.Disable();
        yield return new WaitForSeconds(durationConeSpell);
        Player.Instance.controls.Player.Move.Enable();
        Player.Instance.agent.isStopped = false;
        Player.Instance.attacking = false;
        cone.SetActive(false);
    }
  

    void ConeAttack(float damage, Utility.MagicType magic)
    {
        cone.GetComponent<ConeAttack>().magic = magic;
        cone.GetComponent<ConeAttack>().damage = damage;
        cone.SetActive(true);
        StartCoroutine(CooldownCone());
    }


    void ZoneAttack(float damage,Utility.MagicType magic)
    {
        zone.GetComponent<ZoneAttack>().magic = magic;
        zone.GetComponent<ZoneAttack>().damage = damage;
        zone.SetActive(true);
        zone.GetComponent<ZoneAttack>().placeZone();
       
        StartCoroutine(CooldownZone());
    }

    IEnumerator CooldownZone()
    {
        Player.Instance.attacking = true;
        Player.Instance.agent.isStopped = true;
        Player.Instance.controls.Player.Move.Disable();
        yield return new WaitForSeconds(durationConeSpell);
        Player.Instance.controls.Player.Move.Enable();
        Player.Instance.agent.isStopped = false;
        Player.Instance.attacking = false;
        zone.SetActive(false);
    }
}
