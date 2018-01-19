using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenuAttribute(fileName = "New Weapon Data", menuName = "Create Card/Weapon")]
public class WeaponCard : CardData {

    public float dmg;
    public float durability;
    public float cost;
    public CardEffects cardEffect;

    
}
