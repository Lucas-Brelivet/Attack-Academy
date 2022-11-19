using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class Player : Entity
{
    public static Player Instance { get; private set; }
    NavMeshAgent agent;
    private Animator animator;

    [HideInInspector]
    public Controls controls;

    public float manaMax = 100f;
    public float mana{get; private set;}

    //The point towards which the player is moving
    private Vector2 moveTarget;

    private bool move = false;

    public Utility.Direction currentDirection{get; private set;} = Utility.Direction.Down;

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
    }

    public override void Update()
    {
        base.Update();
        if(move)
        {
            Move();
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

        Utility.Direction direction = Utility.Direction.Down;
        if(Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.y))
        {
            direction = agent.velocity.x > 0 ? Utility.Direction.Right : Utility.Direction.Left;
        }
        else
        {
            direction = agent.velocity.y > 0 ? Utility.Direction.Up : Utility.Direction.Down;
        }

        if(direction != currentDirection)
        {
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

        /* if((moveTarget - (Vector2) transform.position).magnitude < movementSpeed * Time.deltaTime)
        {
            move = false;
        } */
    }

    private void OnChangeMagicType(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>();
        int sign = delta < 0 ? -1 : 1;
        ScrollMagicType(sign);
        UiManager.Instance?.SelectMagicType(currentMagicType);
    }

    protected override void Die()
    {
        print("ahah nul");
    }

    public void ConsumeMana(float amount)
    {
        mana -= amount;
        UiManager.Instance.UpdateMana();
    }
    public void RecoverMana(float amount)
    {
        mana += amount;
        UiManager.Instance.UpdateHealth();
    }
}
