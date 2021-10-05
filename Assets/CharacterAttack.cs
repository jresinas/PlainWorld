using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour {
    [SerializeField] Animator anim;

    public void Attack() {
        //if (!IsBlocking()) {
            anim.SetLayerWeight(1, 1);
            anim.SetTrigger("Slam");
        //}
    }

    public bool IsAttacking() {
        return anim.GetLayerWeight(1) > 0;
    }
}
