using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D collider;

    [SerializeField] CharacterLook look;
    [SerializeField] CharacterMove move;
    [SerializeField] CharacterJump jump;
    [SerializeField] CharacterAttack attack;
    [SerializeField] CharacterHurt hurt;
    [SerializeField] CharacterBlock block;
    [SerializeField] CharacterMount mount;
    [SerializeField] CharacterEquipment equipment;

    public void Look(int direction) {
        look.Look(direction);
    }

    public void Move(int direction) {
        look.Look(direction);
        if (!block.IsBlocking()) move.Move(direction, jump.IsGrounded());
    }

    public void Idle() {
        move.Idle();
    }

    public void Jump() {
        if  (jump.IsGrounded() && !jump.IsJumping()) jump.Jump();
    }

    public void Attack() {
        Debug.Log("Attack: Blocking:" + block.IsBlocking() + "IsAttackgin" + IsAttacking());
        if (!block.IsBlocking() && !IsAttacking()) attack.Attack(equipment.GetWeapon());
    }

    public bool IsAttacking() => attack.IsAttacking();
    public bool IsEndingAttack() => attack.IsEndingAttack();

    public int Hurt(int direction, bool softHit = false) {
        if (!softHit && !hurt.IsInvencible() && !IsBlockSuccess(direction) && mount.IsMount()) mount.Dismount();
        return hurt.Hurt(direction, IsBlockSuccess(direction), softHit);
    }

    public bool IsHurt() => hurt.IsHurt();

    public void Block() {
        if (!IsBusy() && !IsAttacking()) block.Block();
    }

    public void Unblock() {
        if (block.IsBlocking()) block.Unblock();
    }

    bool IsBlockSuccess(int hitDirection) {
        return block.IsBlocking() && ((look.IsLookingRight() && hitDirection < 0) || (!look.IsLookingRight() && hitDirection > 0));
    }

    public void Mount() {
        List<MountController> nearHorses = mount.SearchHorse();
        if (nearHorses.Count > 0) {
            MountController horse = nearHorses[0];
            StopAnimation();
            mount.Mount(horse);
        }
    }

    public void Dismount() {
        mount.Dismount();
    }

    public MountController GetMount() => mount.GetMount();

    public bool IsMount() => mount.IsMount();

    void StopAnimation() {
        anim.SetBool("Walk", false);
        anim.SetBool("Jump", false);
        anim.SetBool("Mount", false);
        anim.SetBool("Block", false); 
    }

    bool IsBusy() {
        return anim.GetBool("Walk") ||
            anim.GetBool("Jump") ||
            anim.GetBool("Mount") ||
            anim.GetBool("Block");
    }

    public void ResetState() {
        StopAnimation();
        block.Unblock();
        block.EndBlock();
        attack.StopAttack();
    }
}
