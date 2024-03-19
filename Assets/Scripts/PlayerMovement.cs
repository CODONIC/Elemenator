using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f; // Adjust this value to control the speed

   
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 joystickInput = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);

        MoveCharacter(joystickInput);
        UpdateAnimation(joystickInput);
    }

    void MoveCharacter(Vector3 input)
    {
        transform.position += input * moveSpeed * Time.deltaTime;
    }
    
    void UpdateAnimation(Vector3 input)
    {
        if (input != Vector3.zero)
        {
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

   
}