using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour {
    [SerializeField] CharacterController character;

    float THRESHOLD_HORIZONTAL_INPUT = 0.25f;

    void Update() {
        //if (character.IsBlocking()) character.Unblock();


        float inputHorizontal = Input.GetAxis("Horizontal");

        if (!character.IsHurt() && !character.IsMount()) {
            if (inputHorizontal < -THRESHOLD_HORIZONTAL_INPUT) {
                character.Move(-1);
            } else if (inputHorizontal > THRESHOLD_HORIZONTAL_INPUT) {
                character.Move(1);
            } else {
                character.Idle();
            }

            if (Input.GetButtonDown("Jump")) {
                character.Jump();
            }

            if (Input.GetButtonDown("Fire3")) {
                character.Attack();
            }

            if (Input.GetButtonDown("Fire2")) {
                character.Mount();
            }

            if (Input.GetButton("Block")) {
                character.Block();
            } else {
                character.Unblock();
            }
        } else if (!character.IsHurt() && character.IsMount()) {
            HorseController mount = character.GetMount();
            if (inputHorizontal < -THRESHOLD_HORIZONTAL_INPUT) {
                mount.Move(-1);
            } else if (inputHorizontal > THRESHOLD_HORIZONTAL_INPUT) {
                mount.Move(1);
            }

            if (Input.GetButtonDown("Fire2")) {
                character.Dismount();
            }

            if (Input.GetButtonDown("Fire3")) {
                character.Attack();
            }

            if (Input.GetButtonDown("Jump") && mount.IsGrounded() && !mount.IsJumping()) {
                mount.Jump();
            }
        }
    }
}
