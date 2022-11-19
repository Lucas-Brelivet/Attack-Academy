using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class playerSpell
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
    [SerializeField] playerSpell[] playerSpells;


    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        UpdateCostAndPower();
        print(playerSpells[0].cost);
        print(playerSpells[0].power);

    }


    void UpdateCostAndPower()
    {
        foreach(var playerSpell in playerSpells)
        {
            playerSpell.cost = 1 + (int)Mathf.Min(player.minDistToStele[playerSpell.magicType],playerSpell.distMax) * playerSpell.costMult;
            playerSpell.power = (int)Mathf.Min(player.minDistToStele[playerSpell.magicType], playerSpell.distMax) * playerSpell.powerMult;
        }
    }


}
