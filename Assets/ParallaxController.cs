using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour {
    float length, startPos;
    [SerializeField] Transform cam;
    [SerializeField] float parallaxEffect;

    void Start() {
        startPos = transform.position.x;
        //length = 32;
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        length = sprite.bounds.size.x;// sprite.sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    void FixedUpdate() {
        float temp = (cam.position.x * (1 - parallaxEffect));
        float dist = (cam.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
