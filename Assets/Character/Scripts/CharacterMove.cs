using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;

    [SerializeField] float MOVE_SPEED = 6f;

    public void Move(int direction, bool isGrounded) {
        //Look(direction);

        //if (!IsBlocking()) {
            if (isGrounded) anim.SetBool("Walk", true);
            rb.velocity = new Vector2(direction * MOVE_SPEED, rb.velocity.y);
        //}
    }

    public void Idle() {
        anim.SetBool("Walk", false);
    }
}
