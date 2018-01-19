using UnityEngine;

public class CardData : ScriptableObject {
    public GameObject cardPrefab;
    public ClassLimitation cardClass;
    public Sprite sprite;
    public string name;
    public string flavorText;


    public enum ClassLimitation {
        none, warrior, mage, rogue
    }

	public enum CardType {
		weapon, spell, monster
	}

	public enum CardEffects {
		none, nowarmup, onspawn, postturn, preturn, ondeath
	}
}
