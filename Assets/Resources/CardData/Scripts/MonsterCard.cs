using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName ="New Monster Data", menuName = "Create Card/Monster")]
public class MonsterCard : CardData {

    public float dmg;
    public float hp;
    public float cost;
    public CardEffects cardEffect;

    public enum CardEffects {
        none, nowarmup, onspawn, postturn, preturn, ondeath
    }
}
