using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour {
    [SerializeField] CharacterController character;
    [SerializeField] CharacterController player;
    Transform lookingAt;
    bool enemy = true;


    [SerializeField] float MIDDLE_DISTANCE = 40f;
    [SerializeField] float NEAR_DISTANCE = 5f;
    [SerializeField] float MAX_ATTACK_DISTANCE = 5.2f;
    [SerializeField] float MIN_ATTACK_DISTANCE = 2f;

    private void Update() {
        lookingAt = player.transform;
        if (!character.IsHurt()) {
            if (IsFar(lookingAt)) {
                Follow(lookingAt);
            } else {
                Look(lookingAt);
            }
            
            if (enemy) {
                if (IsAttackable(lookingAt)) {
                    character.Slam();
                }
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

    bool IsFar(Transform target) {
        return Vector2.Distance(target.position, transform.position) <= MIDDLE_DISTANCE && Vector2.Distance(target.position, transform.position) > NEAR_DISTANCE;
    }

    bool IsAttackable(Transform target) {
        return Vector2.Distance(target.position, transform.position) <= MAX_ATTACK_DISTANCE && Vector2.Distance(target.position, transform.position) >= MIN_ATTACK_DISTANCE;
    }
}
