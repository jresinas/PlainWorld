using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMount : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;

    bool isMount = false;
    MountController mount;

    void Update() {
        if (isMount) {
            transform.position = mount.GetMount().transform.position;
            transform.localScale = mount.transform.localScale;
        }
    }

    public void Mount(MountController horse) {
        rb.gravityScale = 0;
        //StopAnimation();
        anim.SetBool("Mount", true);
        horse.Mount();
        isMount = true;
        mount = horse;
    }

    public void Dismount() {
        rb.gravityScale = 4;
        anim.SetBool("Mount", false);
        mount.Dismount();
        isMount = false;
        mount = null;
    }

    public List<MountController> SearchHorse() {
        List<MountController> near = new List<MountController>();
        MountController[] horses = FindObjectsOfType<MountController>();
        foreach (MountController horse in horses) {
            if (Vector2.Distance(horse.transform.position, transform.position) < 2) {
                near.Add(horse);
            }
        }
        return near;
    }

    public bool IsMount() {
        return isMount;
    }

    public MountController GetMount() {
        return mount;
    }
}
