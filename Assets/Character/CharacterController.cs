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
        jump.Jump();
    }
    public bool IsJumping() => !jump.IsGrounded(); //jump.IsJumping();
    public bool IsGrounded() => jump.IsGrounded();

    public void Attack() {
        if (!block.IsBlocking()) attack.Attack();
    }
    public bool IsAttacking() => attack.IsAttacking();

    public void Hurt(int direction) {
        if (!hurt.IsInvencible() && !IsBlockSuccess(direction) && mount.IsMount()) mount.Dismount();
        hurt.Hurt(direction, IsBlockSuccess(direction));
    }

    public bool IsHurt() => hurt.IsHurt();

    public void Block() {
        block.Block();
    }

    public void Unblock() {
        block.Unblock();
    }

    public bool IsBlocking() => block.IsBlocking();

    bool IsBlockSuccess(int hitDirection) {
        return block.IsBlocking() && ((look.IsLookingRight() && hitDirection < 0) || (!look.IsLookingRight() && hitDirection > 0));
    }

    public void Mount() {
        List<HorseController> nearHorses = mount.SearchHorse();
        if (nearHorses.Count > 0) {
            HorseController horse = nearHorses[0];
            StopAnimation();
            mount.Mount(horse);
        }
    }

    public void Dismount() {
        mount.Dismount();
    }

    public HorseController GetMount() => mount.GetMount();

    public bool IsMount() => mount.IsMount();

    void StopAnimation() {
        anim.SetBool("Walk", false);
        anim.SetBool("Jump", false);
        anim.SetBool("Mount", false);
        anim.SetBool("Block", false); 
    }

    public bool IsBusy() {
        return anim.GetBool("Walk") ||
            anim.GetBool("Jump") ||
            anim.GetBool("Mount") ||
            anim.GetBool("Block");
    }
}
