using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackEffect {
    public void Initialize(Transform character, float speed);

    public Transform GetOwner();
}
