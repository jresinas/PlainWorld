using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour {
    [SerializeField] Animator anim;
    [SerializeField] GameObject effect;

    public void Attack() {
        //if (!IsBlocking()) {
            anim.SetLayerWeight(1, 1);
            anim.SetTrigger("Slam");
        //}
    }

    public bool IsAttacking() {
        return anim.GetLayerWeight(1) > 0;
    }

    public void CreateEffect() {
        //GameObject effectObj = Instantiate(effect, transform.position + Vector3.right * -2.8f * transform.localScale.x + Vector3.up * 1.55f, transform.rotation);
        //effectObj.transform.localScale = transform.localScale;
        GameObject effectObj = Instantiate(effect, transform.position + Vector3.right * -2.8f * transform.localScale.x + Vector3.up * 1.55f, transform.rotation, transform);
        AttackController attack = effectObj.GetComponent<AttackController>();
        attack.character = transform;
        Destroy(effectObj, 0.5f);
    }
}
