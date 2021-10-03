using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour {
    [SerializeField] CharacterController character;

    float THRESHOLD_HORIZONTAL_INPUT = 0.25f;

    void Update() {
        float inputHorizontal = Input.GetAxis("Horizontal");

        if (!character.IsHurt() && !character.IsMount()) {
            if (!character.IsBlocking()) {
                if (inputHorizontal < -THRESHOLD_HORIZONTAL_INPUT) {
                    character.Move(-1);
                } else if (inputHorizontal > THRESHOLD_HORIZONTAL_INPUT) {
                    character.Move(1);
                } else {
                    character.Idle();
                }

                if (Input.GetButtonDown("Jump") && character.IsGrounded() && !character.IsJumping()) {
                    character.Jump();
                }

                if (Input.GetButtonDown("Fire3") && !character.IsAttacking()) {
                    character.Slam();
                }

                if (Input.GetButtonDown("Fire2")) {
                    List<HorseController> nearHorses = character.SearchHorse();
                    if (nearHorses.Count > 0) {
                        character.Mount(nearHorses[0]);
                    }
                }
            }

            if (Input.GetButton("Block") && !character.IsBusy()) {
                character.Block();
            }

            if (character.IsBlocking() && !Input.GetButton("Block")) character.Unblock();

        } else if (!character.IsHurt() && character.IsMount()) {
            HorseController mount = character.GetMount();
            if (inputHorizontal < -THRESHOLD_HORIZONTAL_INPUT) {
                mount.Move(-1);
            } else if (inputHorizontal > THRESHOLD_HORIZONTAL_INPUT) {
                mount.Move(1);
            } else {
                //mount.Idle();
            }

            if (Input.GetButtonDown("Fire2")) {
                character.Dismount();
            }

            if (Input.GetButtonDown("Fire3") && !character.IsAttacking()) {
                character.Slam();
            }

            if (Input.GetButtonDown("Jump") && mount.IsGrounded() && !mount.IsJumping()) {
                mount.Jump();
            }
        }
    }
}
