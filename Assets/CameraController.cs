using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] Transform player;

    void LateUpdate() {
        transform.position = new Vector3(player.position.x, transform.position.y, -10);
    }
}
