using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseController : MonoBehaviour {
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D collider;

    float WALK_SPEED = 14f;
    float RUN_SPEED = 30f;
    bool lookRight = false;

    private void Update() {
        if (rb.velocity.x != 0) {
            anim.SetBool("Walk", true);
            if (Mathf.Abs(rb.velocity.x) > WALK_SPEED) anim.SetBool("Run", true);
            else anim.SetBool("Run", false);
        } else Idle();
    }


    public void Move(int direction) {
        rb.AddForce(new Vector2(direction * 8, 0), ForceMode2D.Force);
        if (rb.velocity.x > 0 && !lookRight) Turn(1);
        else if (rb.velocity.x < 0 && lookRight) Turn(-1);
        if (rb.velocity.x > RUN_SPEED) rb.velocity = new Vector2(RUN_SPEED, rb.velocity.y);
        if (rb.velocity.x < -RUN_SPEED) rb.velocity = new Vector2(-RUN_SPEED, rb.velocity.y);
    }

    public void Idle() {
        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);
    }

    void Turn(int direction) {
        transform.localScale = new Vector2(-direction, 1);
        lookRight = (direction == 1);
    }

    public void Mount() {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sortingOrder = -2;
    }

    public void Dismount() {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sortingOrder = -5;
    }
}
