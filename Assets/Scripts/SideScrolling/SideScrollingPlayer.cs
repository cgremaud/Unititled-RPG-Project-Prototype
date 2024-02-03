using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SideScrollingPlayer : MonoBehaviour
{
    public InputAction WASDMoveAction;
    private float playerInputDirection;
    private float lookDirection;
    public float speed;

    public Animator animator;
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        WASDMoveAction.Enable();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
