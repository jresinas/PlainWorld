using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D collider;
    [SerializeField] LayerMask floorLayer;

    float THRESHOLD_HORIZONTAL_INPUT = 0.25f;
    float MOVE_SPEED = 6f;
    float JUMP_VELOCITY = 20f;
    float HURT_STUN_TIME = 0.5f;
    float HURT_INV_TIME = 3;
    float BLINKING_INTERVAL = 0.1f;
    bool lookRight = false;
    bool isJumping = false;
    bool isHurt = false;
    bool isInvencible = false;

    void Update() {
        float inputHorizontal = Input.GetAxis("Horizontal");

        if (!isHurt) {
            if (inputHorizontal < -THRESHOLD_HORIZONTAL_INPUT) {
                Move(-1);
            } else if (inputHorizontal > THRESHOLD_HORIZONTAL_INPUT) {
                Move(1);
            } else {
                Idle();
            }

            if (Input.GetButtonDown("Jump") && IsGrounded() && !isJumping) {
                Jump();
            }

            if (Input.GetButtonDown("Fire3") && !IsAttacking()) {
                Slam();
            }
        }
    }

    void Move(int direction) {
        anim.SetBool("Walk", true);
        rb.velocity = new Vector2(direction * MOVE_SPEED, rb.velocity.y);

        if (direction > 0 && !lookRight) {
            Turn(1);
        } else if (direction < 0 && lookRight) {
            Turn(-1);
        }
    }

    void Idle() {
        anim.SetBool("Walk", false);
    }

    void Turn(int direction) {
        transform.localScale = new Vector2(-direction, 1);
        lookRight = (direction == 1);
    }

    void Jump() {
        anim.SetBool("Jump", true);
        isJumping = true;
        rb.velocity = Vector2.up * JUMP_VELOCITY;
        StartCoroutine(Raising());
    }

    
    IEnumerator Raising() {
        yield return new WaitUntil(() => !IsGrounded());
        StartCoroutine(InAir());
    }

    IEnumerator InAir() { 
        yield return new WaitUntil(() => IsGrounded());
        Landing();
    }

    void Landing() {
        anim.SetBool("Jump", false);
        isJumping = false;
    }
    

    bool IsGrounded() {
        float offset = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, offset, floorLayer);
        return raycastHit.collider != null;
    }

    void Slam() {
        anim.SetLayerWeight(1, 1);
        anim.SetTrigger("Slam");
    }

    bool IsAttacking() {
        return anim.GetLayerWeight(1) > 0;
    }

    public void Damage(int direction) {
        if (!isInvencible) {
            Debug.Log("Character Damage");
            anim.SetTrigger("Hurt");
            //rb.AddForce(Vector2.right * 2000 * direction + Vector2.up * 200);
            rb.velocity = (Vector2.right * 20 * direction + Vector2.up * 2);
            isHurt = true;
            isInvencible = true;
            StartCoroutine(HurtStun());
            StartCoroutine(HurtInvencible());
        }
    }

    IEnumerator HurtStun() {
        yield return new WaitForSeconds(HURT_STUN_TIME);
        isHurt = false;
    }

    IEnumerator HurtInvencible() {
        StartCoroutine(BlinkEffect());
        yield return new WaitForSeconds(HURT_INV_TIME);
        isInvencible = false;
    }


    IEnumerator BlinkEffect() {
        float value = 0f;
        while (isInvencible) {
            SetAlpha(value);
            if (value > 0) value = 0;
            else value = 255;
            yield return new WaitForSeconds(BLINKING_INTERVAL);
        }
        SetAlpha(1);
    }

    void SetAlpha(float alpha) {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>(true);
        foreach (SpriteRenderer sprite in sprites) {
            Color color = sprite.color;
            color.a = alpha;
            sprite.color = color;
        }
    }
}
