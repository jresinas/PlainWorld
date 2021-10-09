using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
    Melee,
    Range
}

public enum AnimationType {
    Crush,
    Pierce,
    Slash,
    Bow
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Equip/New Weapon", order = 51)]
public class EquipWeapon : ScriptableObject {
    public Sprite image;
    public float xOffset;
    public float yOffset;
    public WeaponType type;
    public int layerOrder;

    public GameObject effect;
    public float animationSpeed;
    public AnimationType animationType;
    public float xEffectOffset;
    public float yEffectOffset;
    public float lifeTime;
}
