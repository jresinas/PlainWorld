using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] Collider2D col;
    [SerializeField] LayerMask floorLayer;

    [SerializeField] float JUMP_VELOCITY = 20f;

    public void Jump() {
        anim.SetBool("Jump", true);
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
    }

    public bool IsGrounded() {
        float offset = 0.02f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, offset, floorLayer);
        return raycastHit.collider != null;
    }

    public bool IsJumping() {
        return anim.GetBool("Jump");
    }
}
