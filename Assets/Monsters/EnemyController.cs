using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] Transform player;

    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    float MOVE_SPEED = 3.5f;
    float PLAYER_DISTANCE = 1;
    float REACTION_TIME = 0.5f;

    bool lookRight = false;
    Vector2? destiny;
    bool isHurt = false;

    void Start() {
        destiny = player.transform.position;
        StartCoroutine(Search());
    }

    void Update() {
        //if (player.position.x < transform.position.x - PLAYER_DISTANCE) {
        //    Move(-1);
        //} else if (player.position.x > transform.position.x + PLAYER_DISTANCE) {
        //    Move(1);
        //} else {
        //    Idle();
        //}

        if (destiny != null && !isHurt) {
            if (((Vector2)destiny).x < transform.position.x - PLAYER_DISTANCE) {
                Move(-1);
            } else if (((Vector2)destiny).x > transform.position.x + PLAYER_DISTANCE) {
                Move(1);
            } else {
                Idle();
            }
        }
    }

    IEnumerator Search() {
        yield return new WaitForSeconds(REACTION_TIME);
        destiny = player.transform.position;
        StartCoroutine(Search());
    }

    void Move(int direction) {
        //anim.SetBool("Walk", true);
        rb.velocity = new Vector2(direction * MOVE_SPEED, rb.velocity.y);

        if (direction > 0 && !lookRight) {
            Turn(1);
        } else if (direction < 0 && lookRight) {
            Turn(-1);
        }
    }

    void Idle() {
        //anim.SetBool("Walk", false);
    }

    void Turn(int direction) {
        transform.localScale = new Vector2(-direction, 1);
        lookRight = (direction == 1);
    }

    /*
    void OnCollisionEnter2D(Collision2D col2) {
        Collider2D col = col2.otherCollider;
        Debug.Log("Collision " + col);
        Debug.Log(col.gameObject.layer);
        if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
            var rel = transform.position - col.transform.position;
            Debug.Log(rel);
            if (rel.x < 5f) { // if we are over the other
                Debug.Log("Coll-2");
                rb.AddForce(rel * 200, ForceMode2D.Impulse); // push us away from the other player
            }
        }
    }
    
    void OnCollisionStay2D(Collision2D col2) {
        Collider2D col = col2.collider;
        Debug.Log("Collision " + col);
        if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
            float rel = transform.position.x - col.transform.position.x;
            Debug.Log(rel);
            //if (rel.x < 1f) { // if we are over the other
            Debug.Log("Coll-2");
            Debug.Log(rel);
            rb.AddForce(Vector2.right * Mathf.Sign(rel) * 500); // push us away from the other player

            //}
        }
    }
    */

    public void Damage(int direction) {
        Debug.Log("Enemy Damage");
        destiny = null;
        //rb.AddForce(Vector2.right * 2000 * direction);
        rb.velocity = Vector2.right * 15 * direction;
        isHurt = true;
        StartCoroutine(HurtWait());
    }

    IEnumerator HurtWait() {
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            int push_direction;
            if (collision.transform.position.x < transform.position.x) push_direction = -1;
            else push_direction = 1;
            //rb.AddForce(Vector2.right * 2000 * push_direction + Vector2.up * 200);

            CharacterController character = collision.gameObject.GetComponent<CharacterController>();
            character.Damage(push_direction);
        }
    }
}
