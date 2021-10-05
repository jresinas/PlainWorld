using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBlock : MonoBehaviour {
    [SerializeField] Animator anim;

    bool isHoldBlocking = false;

    public void Block() {
        anim.SetBool("Block", true);
    }

    public void HoldBlock() {
        isHoldBlocking = true;
    }

    public void Unblock() {
        anim.SetBool("Block", false);
        isHoldBlocking = false;
    }

    public bool IsBlocking() {
        return anim.GetBool("Block");
    }

    public bool IsHoldBlocking() {
        return isHoldBlocking;
    }
}
