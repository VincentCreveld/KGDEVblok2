using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenuAttribute(fileName = "New Spell Data", menuName = "Create Card/Spell")]
public class SpellCard : CardData {

    public float dmg;
    public float cost;
    public SpellType spellType;

    public enum SpellType {
        all, single, random
    }
}
