using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
    MeleeCrush,
    MeleeSlash,
    MeleePierce,
    Wand,
    Staff
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Equip/New Weapon", order = 51)]
public class EquipWeapon : ScriptableObject {
    public Sprite image;
    public float xOffset;
    public float yOffset;
    public WeaponType type;
}
