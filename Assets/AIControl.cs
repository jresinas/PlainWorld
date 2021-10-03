using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour {
    [SerializeField] CharacterController character;
    [SerializeField] CharacterController player;
    Transform lookingAt;
    [SerializeField] bool enemy = true;
    [SerializeField] bool ally = false;


    [SerializeField] float MIDDLE_DISTANCE = 40f;
    [SerializeField] float NEAR_DISTANCE = 4f; //5f;
    [SerializeField] float MAX_ATTACK_DISTANCE = 5f; //5.2f;
    [SerializeField] float MIN_ATTACK_DISTANCE = 2f;

    private void Update() {
        lookingAt = player.transform;
        if (!character.IsHurt()) {
            if (character.IsBlocking()) character.Unblock();
            
            if (enemy) {
                if (IsWatchable(lookingAt)) {
                    Follow(lookingAt);
                } else if (IsAttackable(lookingAt)) {
                    Look(lookingAt);
                    MeleeCombat(lookingAt);
                } else if (IsNear(lookingAt)) {
                    WalkAway(lookingAt);
                }
            } else if (ally) {
                if (IsWatchable(lookingAt)) {
                    Follow(lookingAt);
                } else if (IsNear(lookingAt)) {
                    Look(lookingAt);
                }
            } else {
                if (IsNear(lookingAt)) Look(lookingAt);
            }
        } else {
            character.Idle();
        }
    }


    void Follow(Transform target) {
        if (target.position.x < transform.position.x) character.Move(-1);
        else character.Move(1);
    }
    
    void Look(Transform target) {
        character.Idle();
        if (target.position.x < transform.position.x) character.Turn(-1);
        if (target.position.x > transform.position.x) character.Turn(1);
    }

    void WalkAway(Transform target) {
        if (target.position.x < transform.position.x) character.Move(1);
        else character.Move(-1);
    }

    void MeleeCombat(Transform target) {
        CharacterController enemy = target.GetComponent<CharacterController>();
        float rnd = Random.Range(0f, 1f);
        if (enemy.IsAttacking()) {
            if (rnd > 0.3f) Block();
            else if (rnd > 0.299f) MeleeAttack();
            //else if (rnd > 0.2f) MeleeAttack();
        } else {
            if (rnd > 0.99f) MeleeAttack();
            //if (rnd > 0.3f) MeleeAttack();
        }

        /*
        if (enemy.IsAttacking()) {
            if (rnd > 0.3f) Block();
            else if (rnd > 0.2f) MeleeAttack();
        } else if (enemy.IsBlocking()) {
            if (rnd > 0.9) MeleeAttack();
        } else {
            if (rnd > 0.3f) MeleeAttack();
        }
        */
    }

    void Block() {
        character.Block();
    }

    void MeleeAttack() {
        character.Slam();
    }

    bool IsWatchable(Transform target) {
        return Vector2.Distance(target.position, transform.position) <= MIDDLE_DISTANCE && Vector2.Distance(target.position, transform.position) > NEAR_DISTANCE;
    }

    bool IsNear(Transform target) {
        return Vector2.Distance(target.position, transform.position) < NEAR_DISTANCE;
    }

    bool IsAttackable(Transform target) {
        return Vector2.Distance(target.position, transform.position) <= MAX_ATTACK_DISTANCE && Vector2.Distance(target.position, transform.position) >= MIN_ATTACK_DISTANCE;
    }
}
