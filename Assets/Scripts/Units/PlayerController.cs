using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CombatUnit
{
    public InputAction WASDMoveInput;
    public InputAction ControllerLeftStickInput;
    public InputAction KeyboardInteractionInput;

    private Vector2 playerInputDirection;
    private Vector2 lookDirection;
    
    public float speed = 350;
    
    public Animator animator;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        WASDMoveInput.Enable();
        ControllerLeftStickInput.Enable();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInputDirection = WASDMoveInput.ReadValue<Vector2>();
        if(playerInputDirection == Vector2.zero)
        {
            playerInputDirection = ControllerLeftStickInput.ReadValue<Vector2>();
        }
        if (playerInputDirection != Vector2.zero ) 
        {
            lookDirection.Set(playerInputDirection.x, playerInputDirection.y);
            lookDirection.Normalize(); //needed?
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
        }
        animator.SetFloat("Speed", playerInputDirection.magnitude);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameState != GameState.NEUTRAL)
        {
            return;
        }
        body.velocity = playerInputDirection * speed * Time.deltaTime;
    }
   
}
