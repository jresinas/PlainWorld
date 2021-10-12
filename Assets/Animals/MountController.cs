using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountController : MonoBehaviour {
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D collider;
    [SerializeField] LayerMask floorLayer;
    [SerializeField] Transform mount;

    float WALK_SPEED = 10f;
    float RUN_SPEED = 32f;
    float RUN_ACCEL = 8;
    float JUMP_VELOCITY = 25f;

    public bool isJumping = false;

    private void Update() {
        if (rb.velocity.x != 0) {
            anim.SetBool("Walk", true);
            if (Mathf.Abs(rb.velocity.x) > WALK_SPEED) anim.SetBool("Run", true);
            else anim.SetBool("Run", false);
        } else Idle();
    }

    public Transform GetMount() {
        return mount;
    }

    #region Move
    public void Move(int direction) {
        rb.AddForce(new Vector2(direction * RUN_ACCEL, 0), ForceMode2D.Force);
        if (rb.velocity.x > 0 && !IsLookingRight()) Turn(1);
        else if (rb.velocity.x < 0 && IsLookingRight()) Turn(-1);
        if (rb.velocity.x >= RUN_SPEED) rb.velocity = new Vector2(RUN_SPEED, rb.velocity.y);
        if (rb.velocity.x <= -RUN_SPEED) rb.velocity = new Vector2(-RUN_SPEED, rb.velocity.y);
    }

    public void Idle() {
        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);
    }

    void Turn(int direction) {
        // Fix mount position when flip
        transform.position += new Vector3(mount.localPosition.x * 2 * direction, 0);
        transform.localScale = new Vector2(-direction, 1);
    }

    bool IsLookingRight() {
        return transform.localScale.x == -1;
    }
    #endregion

    #region Jump
    public void Jump() {
        anim.SetBool("Jump", true);
        isJumping = true;
        rb.velocity += Vector2.up * JUMP_VELOCITY;
        StartCoroutine(Raising());
    }


    IEnumerator Raising() {
        //yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitForSeconds(0.1f);
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

    public bool IsGrounded() {
        float offset = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, offset, floorLayer);
        return raycastHit.collider != null;
    }

    public bool IsJumping() {
        return isJumping;
    }
    #endregion

    #region Mount
    public void Mount() {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sortingOrder = -2;
    }

    public void Dismount() {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sortingOrder = -5;
    }
    #endregion
}
