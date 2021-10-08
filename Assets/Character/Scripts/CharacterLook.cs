using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLook : MonoBehaviour {
    public void Look(int direction) {
        if (direction > 0 && !IsLookingRight()) {
            Turn(1);
        } else if (direction < 0 && IsLookingRight()) {
            Turn(-1);
        }
    }

    void Turn(int direction) {
        transform.localScale = new Vector2(-direction, 1);
    }

    public bool IsLookingRight() {
        return transform.localScale.x == -1;
    }
}
