using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LineRenderer))]

public class Player : Entity
{
    public static Player Instance { get; private set; }
    public NavMeshAgent agent{get; private set;}
    private Animator animator;

    private LineRenderer lr;

    [HideInInspector]
    public Controls controls;

    public float manaMax = 100f;
    public float manaRegen = 1f;
    public float mana{get; private set;}

    //The point towards which the player is moving
    private Vector2 moveTarget;

    private bool move = false;
    private bool idle = true;
    [HideInInspector] public bool attacking = false;

    public Utility.Direction currentDirection{get; private set;} = Utility.Direction.Down;

    public Vector2 orientation;

    private Vector3 previousVelocity;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //navmesh
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public override void Start()
    {
        base.Start();
        controls = new Controls();
        controls.Player.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.ChangeMagicType.performed += OnChangeMagicType;

        mana = manaMax;
        UiManager.Instance.UpdateHealth();
        UiManager.Instance.UpdateMana();

        animator = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
    }

    public override void Update()
    {
        base.Update();

        mana += manaRegen*Time.deltaTime;
        UiManager.Instance.UpdateMana();

        if(attacking)
        {
            idle = false;
        }
        if(move)
        {
            if(!attacking)
            {
                Move();
                ChooseAnimation();
            }
        }
        else if(!idle && !attacking)
        {
            animator.SetTrigger("Idle");
            idle = true;
        }
        

        previousVelocity = agent.velocity;

        Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        orientation = target;

        float minDist = float.PositiveInfinity;
        Stele minStele = null;
        foreach (Stele stele in steleList)
        {
            float dist = Vector2.Distance(stele.transform.position, transform.position);
            if (dist < minDist)
            {
                dist = minDist;
                minStele = stele;
            }
        }

        if (minStele != null)
        {
            lr.enabled = true;
            Debug.Log("Working: " + minStele.transform.position);
            lr.SetPosition(1, minStele.transform.position - transform.position);
            float dist = Mathf.Clamp((minStele.transform.position - transform.position).magnitude, 0, maxDistance);
            float alpha = dist / maxDistance;
            switch (minStele.magicType)
            {
                case Utility.MagicType.Fire:
                    lr.startColor = new Color(1, 0, 0, alpha);
                    lr.endColor = new Color(1, 0, 0, (1-alpha)/2);
                    break;
                case Utility.MagicType.Ice:
                    lr.startColor = new Color(0, 0, 1, alpha);
                    lr.endColor = new Color(0, 0, 1, (1-alpha)/2);
                    break;
                case Utility.MagicType.Lightning:
                    lr.startColor = new Color(1, 1, 0, alpha);
                    lr.endColor = new Color(1, 1, 0, (1-alpha)/2);
                    break;
                case Utility.MagicType.Wind:
                    lr.startColor = new Color(0, 1, 0, alpha);
                    lr.endColor = new Color(0, 1, 0, (1-alpha)/2);
                    break;
            }
        }
        else
        {
            lr.enabled = false;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        move = true;
    }

    private void Move()
    {
        if(controls.Player.Move.IsPressed())
        {
            Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
            moveTarget = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        //transform.Translate((moveTarget - (Vector2)(transform.position)).normalized * movementSpeed * Time.deltaTime);
        agent.SetDestination(new Vector3(moveTarget.x, moveTarget.y, transform.position.z));


        if((agent.destination - transform.position).magnitude <= Mathf.Abs(agent.baseOffset) + 0.001f)
        {
            move = false;
        }
    }

    private void ChooseAnimation()
    {
        Utility.Direction direction = currentDirection;
        if(Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.y))
        {
            direction = agent.velocity.x > 0 ? Utility.Direction.Right : Utility.Direction.Left;
        }
        else
        {
            direction = agent.velocity.y > 0 ? Utility.Direction.Up : Utility.Direction.Down;
        }

        if(direction != currentDirection || idle)
        {
            idle = false;
            currentDirection = direction;
            switch(currentDirection)
            {
                case Utility.Direction.Down:
                    animator.SetTrigger("WalkDown");
                    break;
                case Utility.Direction.Left:
                    animator.SetTrigger("WalkLeft");
                    break;
                case Utility.Direction.Up:
                    animator.SetTrigger("WalkUp");
                    break;
                case Utility.Direction.Right:
                    animator.SetTrigger("WalkRight");
                    break;
            }
        }
    }

    private void OnChangeMagicType(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>();
        int sign = delta < 0 ? -1 : 1;
        ScrollMagicType(sign);
        UiManager.Instance?.SelectMagicType(currentMagicType);
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        UiManager.Instance.UpdateHealth();
    }

    protected override void Die()
    {
        print("Player Dead");
        SceneManager.LoadScene("MenuScene");
    }

    public void ConsumeMana(float amount)
    {
        mana -= amount;
        UiManager.Instance.UpdateMana();
    }
    public void RecoverMana(float amount)
    {
        mana += amount;
        UiManager.Instance.UpdateMana();
    }

    void OnDestroy()
    {
        controls.Dispose();
    }
}
