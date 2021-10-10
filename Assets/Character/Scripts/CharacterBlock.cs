using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBlock : MonoBehaviour {
    [SerializeField] Animator anim;
    bool isBlocking = false;
    bool isHoldBlocking = false;

    public void Block() {
        anim.SetBool("Block", true);
        isBlocking = true;
    }

    public void HoldBlock() {
        isHoldBlocking = true;
    }

    public void EndBlock() {
        isBlocking = false;
    }

    public void Unblock() {
        anim.SetBool("Block", false);
        isHoldBlocking = false;
    }

    public bool IsBlocking() {
        //return anim.GetBool("Block");
        return isBlocking;
    }

    public bool IsHoldBlocking() {
        return isHoldBlocking;
    }
}
