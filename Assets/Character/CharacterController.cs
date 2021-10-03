using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D collider;
    [SerializeField] LayerMask floorLayer;

    [SerializeField] SpriteRenderer charHair;
    [SerializeField] SpriteRenderer charFacialHair;

    [SerializeField] SpriteRenderer charHelmet;
    [SerializeField] SpriteRenderer charWeapon;
    [SerializeField] SpriteRenderer charShield;

    [SerializeField] EquipHelmet equipHelmet;
    [SerializeField] EquipWeapon equipWeapon;
    [SerializeField] EquipShield equipShield;

    [SerializeField] float MOVE_SPEED = 6f;
    [SerializeField] float JUMP_VELOCITY = 20f;
    [SerializeField] float HURT_STUN_TIME = 0.5f;
    [SerializeField] float HURT_INV_TIME = 3;
    [SerializeField] float BLINKING_INTERVAL = 0.1f;
    [SerializeField] Vector2 DAMAGE_PUSH = new Vector2(20,2);
    [SerializeField] Vector2 BLOCK_PUSH = new Vector2(10,2);
    bool isJumping = false;
    bool isHurt = false;
    bool isMount = false;
    bool isInvencible = false;
    bool isBlocking = false;
    bool isHoldBlocking = false;
    HorseController mount;

    void Start() {
        if (equipHelmet != null) SetHelmet(equipHelmet);
        if (equipWeapon != null) SetWeapon(equipWeapon);
        if (equipShield != null) SetShield(equipShield);
    }

    void Update() {
        if (isMount) {
            transform.position = mount.GetMount().transform.position; 
            //transform.position = new Vector2(mount.transform.position.x, mount.transform.position.y + 1);
            transform.localScale = mount.transform.localScale;
        }        
    }

    #region Move
    public void Move(int direction) {
        if (!IsBlocking()) {
            if (IsGrounded()) anim.SetBool("Walk", true);
            rb.velocity = new Vector2(direction * MOVE_SPEED, rb.velocity.y);
        }

        if (direction > 0 && !IsLookingRight()) {
            Turn(1);
        } else if (direction < 0 && IsLookingRight()) {
            Turn(-1);
        }
    }

    public void Idle() {
        anim.SetBool("Walk", false);
    }

    public void Turn(int direction) {
        transform.localScale = new Vector2(-direction, 1);
    }

    bool IsLookingRight() {
        return transform.localScale.x == -1;
    }
    #endregion

    #region Jump
    public void Jump() {
        anim.SetBool("Jump", true);
        isJumping = true;
        rb.velocity += Vector2.up * JUMP_VELOCITY;
        StartCoroutine(Raising());
    }

    
    IEnumerator Raising() {
        //yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(InAir());
    }

    IEnumerator InAir() {
        yield return new WaitUntil(() => IsGrounded());
        Landing();
    }

    void Landing() {
        anim.SetBool("Jump", false);
        isJumping = false;
    }
    
    public bool IsGrounded() {
        float offset = 0.02f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, offset, floorLayer);
        return raycastHit.collider != null;
    }

    public bool IsJumping() {
        return isJumping;
    }
    #endregion

    #region Attack
    public void Slam() {
        if (!IsBlocking()) {
            anim.SetLayerWeight(1, 1);
            anim.SetTrigger("Slam");
        }
    }

    public bool IsAttacking() {
        return anim.GetLayerWeight(1) > 0;
    }
    #endregion

    #region Hurt
    public void Damage(int direction) {
        //if (!isInvencible && !isBlocking) {
        if (!isInvencible && !IsBlockSuccess(direction)) {
            anim.SetLayerWeight(1,0);
            if (isMount) Dismount();
            anim.SetTrigger("Hurt");
            rb.velocity = (Vector2.right * DAMAGE_PUSH.x * direction + Vector2.up * DAMAGE_PUSH.y);
            isHurt = true;
            isInvencible = true;
            StartCoroutine(HurtStun());
            StartCoroutine(HurtInvencible());
        //} else if (isBlocking) {
        } else if (IsBlockSuccess(direction)) {
            rb.velocity = (Vector2.right * BLOCK_PUSH.x * direction + Vector2.up * BLOCK_PUSH.y);
        }
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

    bool IsBlockSuccess(int hitDirection) {
        return isBlocking && ((IsLookingRight() && hitDirection < 0) || (!IsLookingRight() && hitDirection > 0));
    }
    #endregion

    #region Mount
    public void Mount(HorseController horse) {
        //rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        StopAnimation();
        anim.SetBool("Mount", true);
        horse.Mount();
        isMount = true;
        mount = horse;
    }

    public void Dismount() {
        //rb.velocity = Vector2.zero;
        rb.gravityScale = 4;
        anim.SetBool("Mount", false);
        mount.Dismount();
        isMount = false;
        mount = null;  
    }

    public List<HorseController> SearchHorse() {
        List<HorseController> near = new List<HorseController>();
        HorseController[] horses = FindObjectsOfType<HorseController>();
        foreach (HorseController horse in horses) {
            //if (Mathf.Abs(horse.transform.position.x - transform.position.x) < 2) {
            if (Vector2.Distance(horse.transform.position, transform.position) < 2) {
                near.Add(horse);
            }
        }
        return near;
    }

    public bool IsMount() {
        return isMount;
    }

    public HorseController GetMount() {
        return mount;
    }
    #endregion

    #region Block
    public void Block() {
        anim.SetBool("Block", true);
        isBlocking = true;
    }

    public void HoldBlock() {
        isHoldBlocking = true;
    }

    public void Unblock() {
        anim.SetBool("Block", false);
        isBlocking = false;
        isHoldBlocking = false;
    } 

    public bool IsBlocking() {
        return isBlocking;
    }

    public bool IsHoldBlocking() {
        return isHoldBlocking;
    }
    #endregion

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


    void SetHelmet(EquipHelmet helmet) {
        charHelmet.sprite = helmet.image;
        charHelmet.transform.localPosition = new Vector2(helmet.xOffset, helmet.yOffset);
        charHair.enabled = !helmet.hideHair;
        charFacialHair.enabled = !helmet.hideFacialHair;
    }

    void SetWeapon(EquipWeapon weapon) {
        charWeapon.sprite = weapon.image;
        charWeapon.transform.localPosition = new Vector2(weapon.xOffset, weapon.yOffset);
    }

    void SetShield(EquipShield shield) {
        charShield.sprite = shield.image;
        charShield.transform.localPosition = new Vector2(shield.xOffset, shield.yOffset);
    }
}
