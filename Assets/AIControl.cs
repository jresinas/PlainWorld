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
    [SerializeField] float CAREFUL_DISTANCE = 15f;
    [SerializeField] float NEAR_DISTANCE = 5f; //5f;
    [SerializeField] float MAX_ATTACK_DISTANCE = 6f; //5.2f;
    [SerializeField] float MIN_ATTACK_DISTANCE = 2f;

    private void Update() {
        lookingAt = player.transform;
        if (!character.IsHurt()) {
            if (character.IsBlocking()) character.Unblock();
            
            if (enemy) {
                if (IsAttackable(lookingAt)) {
                    Look(lookingAt);
                    MeleeCombat(lookingAt);
                } else if (IsCarefulDistance(lookingAt)) {
                    FollowCarefully(lookingAt);
                } else if (IsMiddleDistance(lookingAt)) {
                    Follow(lookingAt);
                } else if (IsNear(lookingAt)) {
                    WalkAway(lookingAt);
                }
            } else if (ally) {
                if (IsMiddleDistance(lookingAt) || IsCarefulDistance(lookingAt)) {
                    Follow(lookingAt);
                } else if (IsNear(lookingAt)) {
                    Look(lookingAt);
                }
            } else {
                if (IsNear(lookingAt)) Look(lookingAt);
            }
        } else {
            //character.Move(0);
            character.Idle();
        }
    }


    void Follow(Transform target) {
        character.Move(GetDirection(target));
    }

    void FollowCarefully(Transform target) {
        CharacterController enemy = target.GetComponent<CharacterController>();
        float rnd = Random.Range(0f, 1f);
        if (enemy.IsAttacking()) {
            if (rnd > 0.1f) Block();
            else Follow(target);
            //Block();
        } else {
            Follow(target);
        }
    }

    void Look(Transform target) {
        //character.90);
        character.Idle();
        character.Look(GetDirection(target));
    }

    void WalkAway(Transform target) {
        character.Move(-GetDirection(target));
    }

    void MeleeCombat(Transform target) {
        CharacterController enemy = target.GetComponent<CharacterController>();
        float rnd = Random.Range(0f, 1f);
        if (enemy.IsAttacking()) {
            if (rnd > 0.2f) Block();
            else if (rnd > 0.196f) MeleeAttack();
            //else if (rnd > 0.2f) MeleeAttack();
        } else {
            if (rnd > 0.96f) MeleeAttack();
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
        character.Attack();
    }

    bool IsMiddleDistance(Transform target) {
        return Vector2.Distance(target.position, transform.position) <= MIDDLE_DISTANCE && Vector2.Distance(target.position, transform.position) > CAREFUL_DISTANCE;
    }

    bool IsCarefulDistance(Transform target) {
        return Vector2.Distance(target.position, transform.position) <= CAREFUL_DISTANCE && Vector2.Distance(target.position, transform.position) > NEAR_DISTANCE;
    }

    bool IsNear(Transform target) {
        return Vector2.Distance(target.position, transform.position) <= NEAR_DISTANCE;
    }

    bool IsAttackable(Transform target) {
        return Vector2.Distance(target.position, transform.position) <= MAX_ATTACK_DISTANCE && Vector2.Distance(target.position, transform.position) >= MIN_ATTACK_DISTANCE;
    }

    int GetDirection(Transform target) {
        if (target.position.x < transform.position.x) return -1;
        if (target.position.x > transform.position.x) return 1;
        else return 0;
    }
}
