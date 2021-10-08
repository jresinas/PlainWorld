using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Equip/New Shield", order = 51)]
public class EquipShield : ScriptableObject {
    public Sprite image;
    public float xOffset;
    public float yOffset;
}
