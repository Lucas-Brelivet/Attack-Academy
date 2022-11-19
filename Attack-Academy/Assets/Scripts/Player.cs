using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
public class Player : Entity
{
    public static Player Instance { get; private set; }
    NavMeshAgent agent;

    [HideInInspector]
    public Controls controls;

    //The point towards which the player is moving
    private Vector2 moveTarget;

    private bool move = false;

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

    void Start()
    {
        base.Start();
        controls = new Controls();
        controls.Player.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.ChangeMagicType.performed += OnChangeMagicType;
    }

    void Update()
    {
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
        if((moveTarget - (Vector2) transform.position).magnitude < movementSpeed * Time.deltaTime)
        {
            move = false;
        }
    }

    private void OnChangeMagicType(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>();
        int sign = delta < 0 ? -1 : 1;
        ScrollMagicType(sign);
    }
}
