using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour {
    [SerializeField] CharacterController character;
    [SerializeField] CharacterController player;
    Transform lookingAt;
    [SerializeField] bool enemy = true;
    [SerializeField] bool ally = false;

    // Increment for attacking level
    float ATA_LEVEL_INC = 0.005f;
    // Attack decrement during counter attack
    float ATA_COUNT_DEC = 0.04f;
    // Increment for defense level
    float DEF_LEVEL_INC = 0.014f;
    // Defense increment during combat distance
    float DEF_COMBAT_INC = 0.001f;

    [SerializeField] float ATA_LEVEL = 5;
    [SerializeField] float DEF_LEVEL = 5;

    float defenseCombatDistanceValue;
    float defenseCarefulDistanceValue;
    float attackValue;
    float counterAttackValue;

    [SerializeField] float DANGER_DISTANCE = 7f;
    [SerializeField] float MIDDLE_DISTANCE = 40f;
    [SerializeField] float CAREFUL_DISTANCE = 15f;
    [SerializeField] float NEAR_DISTANCE = 3f; // 5f; //5f;
    [SerializeField] float MAX_ATTACK_DISTANCE = 4.8f; //6f; //5.2f;
    [SerializeField] float MIN_ATTACK_DISTANCE = 2.5f; //2f;

    void Start() {
        defenseCombatDistanceValue = DEF_LEVEL * DEF_LEVEL_INC + DEF_COMBAT_INC;
        defenseCarefulDistanceValue = DEF_LEVEL * DEF_LEVEL_INC;
        attackValue = ATA_LEVEL * ATA_LEVEL_INC;
        counterAttackValue = ATA_LEVEL * ATA_LEVEL_INC - ATA_COUNT_DEC;
    }

    private void Update() {
        lookingAt = player.transform;
        if (!character.IsHurt()) {
            CharacterController enemyChar = lookingAt.GetComponent<CharacterController>();
            //if (!enemyChar.IsAttacking()) character.Unblock();
            if (!IsDangerous(lookingAt)) character.Unblock();

            if (enemy) {
                Debug.Log("ES ENEMIGO");
                if (IsAttackable(lookingAt)) {
                    Debug.Log("ATACABLE");
                    Look(lookingAt);
                    MeleeCombat(lookingAt);
                } else if (IsCarefulDistance(lookingAt)) {
                    Debug.Log("CUIDADO");
                    FollowCarefully(lookingAt);
                } else if (IsMiddleDistance(lookingAt)) {
                    Debug.Log("MEDIA");
                    Follow(lookingAt);
                } else if (IsNear(lookingAt)) {
                    Debug.Log("CERCA");
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
        //if (enemy.IsAttacking()) {
        if (IsDangerous(target)) {
            //if (rnd > 0.925f) Block();
            //else Follow2(target);
            //DefenseAction(false, () => { Follow2(target); });
            if (rnd < defenseCarefulDistanceValue) Block();
            else Follow(target);
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
        Debug.Log("MELEE");
        CharacterController enemy = target.GetComponent<CharacterController>();
        float rnd = Random.Range(0f, 1f);
        //if (enemy.IsAttacking()) {
        if (IsDangerous(target)) {
            Debug.Log("PELIGROSO");
            //if (rnd > 0.925f) Block();
            //else if (rnd > 0.921f) MeleeAttack();
            //AttackDefenseAction(true);
            if (rnd < defenseCombatDistanceValue) Block();
            else if (rnd < defenseCombatDistanceValue + counterAttackValue) MeleeAttack();
        } else {
            Debug.Log("NO PELIGROSO");
            Debug.Log(rnd);
            //if (rnd > 0.96f) MeleeAttack();
            //AttackAction();
            if (rnd < attackValue) MeleeAttack();
        }
    }

    void Block() {
        character.Idle();
        character.Block();
    }

    void MeleeAttack() {
        Debug.Log("MeleeAttack");
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

    bool IsDangerous(Transform target) {
        CharacterController enemy = target.GetComponent<CharacterController>();
        if (enemy.IsAttacking() && !enemy.IsEndingAttack() && Vector2.Distance(target.position, transform.position) <= DANGER_DISTANCE) return true;
        GameObject[] attacks = GameObject.FindGameObjectsWithTag("AttackEffect");
        foreach (GameObject attack in attacks) {
            if (attack.GetComponent<IAttackEffect>().GetOwner() != transform && Vector2.Distance(attack.transform.position, transform.position) <= DANGER_DISTANCE) return true;
        }
        return false;
    }


    /*
    void DefenseAction(bool isCombatPosition, System.Action callbackMethod = null) {
        float rnd = Random.Range(0f, 1f);
        if (isCombatPosition) {
            if (rnd < (DEF_LEVEL * DEF_LEVEL_INC + DEF_COMBAT_INC)) Block();
            else if (callbackMethod != null) callbackMethod.Invoke();
        } else {
            if (rnd < (DEF_LEVEL * DEF_LEVEL_INC)) Block();
            else if (callbackMethod != null) callbackMethod.Invoke();
        }
    }

    void AttackAction(System.Action callbackMethod = null) {
        Debug.Log(ATA_LEVEL * ATA_LEVEL_INC);
        float rnd = Random.Range(0f, 1f);
        if (rnd < (ATA_LEVEL * ATA_LEVEL_INC)) MeleeAttack();
        else if (callbackMethod != null) callbackMethod.Invoke();
    }

    void AttackDefenseAction(bool isCombatPosition, System.Action callbackMethod = null) {
        float rnd = Random.Range(0f, 1f);
        if (isCombatPosition) {
            if (rnd < (DEF_LEVEL * DEF_LEVEL_INC + DEF_COMBAT_INC)) Block();
            else if (rnd < (DEF_LEVEL * DEF_LEVEL_INC + DEF_COMBAT_INC) + (ATA_LEVEL * ATA_LEVEL_INC)) MeleeAttack();
            else if (callbackMethod != null) callbackMethod.Invoke();
        } else {
            if (rnd < (DEF_LEVEL * DEF_LEVEL_INC)) Block();
            else if (rnd < (DEF_LEVEL * DEF_LEVEL_INC) + (ATA_LEVEL * ATA_LEVEL_INC)) MeleeAttack();
            else if (callbackMethod != null) callbackMethod.Invoke();
        }
    }
    */
}
