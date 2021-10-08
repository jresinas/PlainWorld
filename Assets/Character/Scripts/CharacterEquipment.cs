using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour {
    [SerializeField] SpriteRenderer charHelmet;
    [SerializeField] SpriteRenderer charWeapon;
    [SerializeField] SpriteRenderer charShield;

    [SerializeField] EquipHelmet equipHelmet;
    [SerializeField] EquipWeapon equipWeapon;
    [SerializeField] EquipShield equipShield;

    [SerializeField] SpriteRenderer charHair;
    [SerializeField] SpriteRenderer charFacialHair;

    void Start() {
        if (equipHelmet != null) SetHelmet(equipHelmet);
        if (equipWeapon != null) SetWeapon(equipWeapon);
        if (equipShield != null) SetShield(equipShield);
    }

    void SetHelmet(EquipHelmet helmet) {
        charHelmet.sprite = helmet.image;
        charHelmet.transform.localPosition = new Vector2(helmet.xOffset, helmet.yOffset);
        charHair.enabled = !helmet.hideHair;
        charFacialHair.enabled = !helmet.hideFacialHair;
    }

    void SetWeapon(EquipWeapon weapon) {
        charWeapon.sprite = weapon.image;
        charWeapon.transform.localPosition = new Vector2(weapon.xOffset, weapon.yOffset);
    }

    void SetShield(EquipShield shield) {
        charShield.sprite = shield.image;
        charShield.transform.localPosition = new Vector2(shield.xOffset, shield.yOffset);
    }

    public EquipWeapon GetWeapon() {
        return equipWeapon;
    }
}
