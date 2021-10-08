using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Helmet", menuName = "Equip/New Helmet", order = 51)]
public class EquipHelmet : ScriptableObject {
    public Sprite image;
    public float xOffset;
    public float yOffset;
    public bool hideHair;
    public bool hideFacialHair;
}
