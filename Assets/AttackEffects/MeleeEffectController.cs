using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEffectController : MonoBehaviour, IAttackEffect {
    [SerializeField] Animator anim;
    Transform character;

    public void Initialize(Transform character, float speed) {
        this.character = character;
        anim.SetFloat("EffectSpeed", speed);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject != character.gameObject) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                int push_direction;
                if (collision.transform.position.x < character.position.x) push_direction = -1;
                else push_direction = 1;
                enemy.Damage(push_direction);
            } else if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
                CharacterController enemy = collision.gameObject.GetComponentInParent<CharacterController>();
                int push_direction;
                if (collision.transform.position.x < character.position.x) push_direction = -1;
                else push_direction = 1;
                enemy.Hurt(push_direction);
            }
        }
    }

    public Transform GetOwner() {
        return character;
    }
}
