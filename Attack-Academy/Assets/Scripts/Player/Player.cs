using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

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
    private bool idle = true;

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
        Debug.Log(currentDirection);
    }

    public override void Update()
    {
        base.Update();

        if(move)
        {
            Move();
        }
        else
        {
            animator.SetTrigger("Idle");
            idle = true;
        }
        previousVelocity = agent.velocity;

        Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        orientation = target;
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

        if(!agent.hasPath)
        {
            move = false;
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
