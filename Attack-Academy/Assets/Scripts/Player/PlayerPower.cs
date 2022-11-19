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
    public float powerMult;
    public float distMax;
    public float cost { get; set; }
    public float power { get; set; }
}



public class PlayerPower : MonoBehaviour
{
    Player player;
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
            playerSpell.cost = 1 + Mathf.Min(player.minDistToStele[playerSpell.magicType],playerSpell.distMax) * playerSpell.costMult;
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
            if (number < playerSpells.Length)
            {
                var spell = spells.ElementAtOrDefault(number);
                if (spell != null)
                {
                    if (spell.spellType == Utility.SpellType.Cone)
                    {
                        ConeAttack(spell.power);
                    }
                    else if (spell.spellType == Utility.SpellType.Zone)
                    {
                        ZoneAttack(spell.power);
                    }



                    StartCoroutine(Cooldown());
                }
            }          
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
        yield return new WaitForSeconds(durationConeSpell);
        cone.SetActive(false);
    }
  

    void ConeAttack(float damage)
    {
        cone.GetComponent<ConeAttack>().damage = damage;
        cone.SetActive(true);
        StartCoroutine(CooldownCone());
    }


    void ZoneAttack(float damage)
    {
        zone.GetComponent<ZoneAttack>().damage = damage;
        zone.GetComponent<ZoneAttack>().placeZone();
        zone.SetActive(true);
        StartCoroutine(CooldownZone());
    }

    IEnumerator CooldownZone()
    {
        yield return new WaitForSeconds(durationConeSpell);
        zone.SetActive(false);
    }
}
