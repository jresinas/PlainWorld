using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEffectController : MonoBehaviour, IAttackEffect {
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sprite;

    float FADE_OUT_TIME = 2;
    float PROJ_HIT_CENTER = 0.6f;
    float PROJ_HIT_OFFSET = 0.3f;

    Transform character;
    int direction;
    float speed;
    bool isMoving = true;
    

    // Update is called once per frame
    void Update() {
        if (direction != 0 && isMoving) 
            rb.velocity = Vector2.right * speed * direction;
    }

    public void Initialize(Transform character, float speed) {
        this.character = character;
        this.speed = speed;
        transform.localScale = character.localScale;
        direction = (int)-character.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject != character.gameObject && isMoving) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                int push_direction;
                if (collision.transform.position.x < character.position.x) push_direction = -1;
                else push_direction = 1;
                enemy.Damage(push_direction);
                Destroy(gameObject);
            } else if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
                CharacterController enemy = collision.gameObject.GetComponentInParent<CharacterController>();
                int push_direction;
                if (collision.transform.position.x < character.position.x) push_direction = -1;
                else push_direction = 1;
                int result = enemy.Hurt(push_direction, true);
                Impact(enemy.transform, result);
            }
        }
    }

    void Impact(Transform enemy, int result) {
        if (result >= 0) {
            float xOffset = Random.Range(-PROJ_HIT_OFFSET, PROJ_HIT_OFFSET);
            float yOffset = Random.Range(-PROJ_HIT_OFFSET, PROJ_HIT_OFFSET);
            Vector3 vOffset = new Vector2(xOffset, yOffset);
            anim.SetTrigger("Impact");
            isMoving = false;
            Destroy(rb);
            if (result == 1) {
                //Destroy(gameObject);
                Transform headTransform = enemy.Find("Body").Find("Head");
                transform.parent = headTransform;
                //transform.parent = enemy;
                sprite.sortingOrder = 3;
                
            } else {
                Transform shieldTransform = enemy.Find("Body").Find("LHand").Find("Shield");
                transform.parent = shieldTransform;
                sprite.sortingOrder = 5;
            }
            transform.localPosition = Vector3.zero - new Vector3(PROJ_HIT_CENTER, 0) + vOffset;
        }
    }
   
    IEnumerator FadeOut() {
        Color color = sprite.material.color;
        for (float f = 0; f <= FADE_OUT_TIME; f += Time.deltaTime) {
            color.a = Mathf.Lerp(1f, 0f, f / FADE_OUT_TIME);
            sprite.material.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
}
