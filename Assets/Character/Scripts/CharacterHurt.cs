using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurt : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;

    [SerializeField] float HURT_STUN_TIME = 0.5f;
    [SerializeField] float HURT_INV_TIME = 2;
    [SerializeField] float BLINKING_INTERVAL = 0.1f;
    [SerializeField] Vector2 DAMAGE_PUSH = new Vector2(20, 2);
    [SerializeField] Vector2 BLOCK_PUSH = new Vector2(10, 2);

    bool isInvencible = false;
    bool isHurt = false;

    public int Hurt(int direction, bool isBlockSuccess, bool softHit) {
        if (!isInvencible && !isBlockSuccess) {
            //StopAttack();
            anim.SetTrigger("Hurt");
            if (!softHit) rb.velocity = (Vector2.right * DAMAGE_PUSH.x * direction + Vector2.up * DAMAGE_PUSH.y);
            else rb.velocity = (Vector2.right * DAMAGE_PUSH.x * direction + Vector2.up * DAMAGE_PUSH.y)/2;
            isHurt = true;
            isInvencible = true;
            StartCoroutine(HurtStun());
            StartCoroutine(HurtInvencible());
            return 1;
        } else if (isBlockSuccess) {
            if (!softHit) rb.velocity = (Vector2.right * BLOCK_PUSH.x * direction + Vector2.up * BLOCK_PUSH.y);
            else rb.velocity = (Vector2.right * BLOCK_PUSH.x * direction + Vector2.up * BLOCK_PUSH.y)/2;
            return 0;
        } else return -1;
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

    public bool IsHurt() {
        return isHurt;
    }

    public bool IsInvencible() {
        return isInvencible;
    }
}
