using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            Transform character = transform.parent.parent;
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            int push_direction;
            if (collision.transform.position.x < character.position.x) push_direction = -1;
            else push_direction = 1;
            enemy.Damage(push_direction);
        }
    }
}
