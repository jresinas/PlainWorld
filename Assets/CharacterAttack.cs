using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour {
    [SerializeField] Animator anim;
    //[SerializeField] GameObject effect;
    EquipWeapon equipedWeapon;

    public void Attack(EquipWeapon weapon) {
        equipedWeapon = weapon;
        if (weapon != null) {
            string animationName = weapon.animationType.ToString();
            int animationLayer = GetWeaponAnimationLayer();
            anim.SetLayerWeight(animationLayer, 1);
            anim.SetFloat("AttackSpeed", equipedWeapon.animationSpeed);
            anim.SetTrigger(animationName);
        }
    }

    public bool IsAttacking() {
        int animationLayer = GetWeaponAnimationLayer();
        return anim.GetLayerWeight(animationLayer) > 0;
    }

    public void CreateEffect() {
        GameObject effectObj = Instantiate(equipedWeapon.effect, transform.position + Vector3.right * equipedWeapon.xEffectOffset * transform.localScale.x + Vector3.up * equipedWeapon.yEffectOffset, transform.rotation, transform);
        AttackController attack = effectObj.GetComponent<AttackController>();
        //attack.character = transform;
        attack.Initialize(transform, equipedWeapon.animationSpeed);
        //Destroy(effectObj, 0.3f);
        Destroy(effectObj, 0.6f/equipedWeapon.animationSpeed);
    }

    int GetWeaponAnimationLayer() {
        if (equipedWeapon != null) {
            string animationName = equipedWeapon.animationType.ToString();
            return anim.GetLayerIndex(animationName);
        } else return 0;
    }
}
