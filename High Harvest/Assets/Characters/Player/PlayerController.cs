using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollision = new List<RaycastHit2D>();
    Animator animator;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate(){
        TryMove(movementInput);
    }


    private bool DetectCollision(Vector2 movement){
        int count = rb.Cast(
            movement,
            movementFilter,
            castCollision,
            moveSpeed * Time.fixedDeltaTime + collisionOffset
        );
        return count > 0;
    }
    private void TryMove(Vector2 movement){
        // Test for collisions in x and y and set movements accordingly
        float moveY;
        float moveX;
        if (!DetectCollision(new Vector2(0, movement.y))){
            moveY = movement.y;
        } else {
            moveY = 0f;
        }
        if (!DetectCollision(new Vector2(movement.x, 0))){
            moveX = movement.x;
        } else {
            moveX = 0f;
        }

        // Move the player based on movement input and collisions
        rb.MovePosition(rb.position + new Vector2(moveX, moveY) * moveSpeed * Time.fixedDeltaTime);

        // Set animation booleans based on the direction the player is moving
        if (moveX > 0f || (moveX == 0f && moveY != 0f)){
            animator.SetBool("movingRight", true);
            animator.SetBool("movingLeft", false);
        } else if (moveX < 0f){
            animator.SetBool("movingRight", false);
            animator.SetBool("movingLeft", true);
        } else {
            animator.SetBool("movingRight", false);
            animator.SetBool("movingLeft", false);
        }
    }
    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();
    }
}
