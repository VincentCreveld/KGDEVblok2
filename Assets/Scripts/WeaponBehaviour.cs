public class WeaponBehaviour : CardBehaviour {
    public WeaponCard weaponData;

	public int dmg;
	public int durability;
	public int cost;
	public WeaponCard.CardEffects cardEffect;

	public void SetupCardValues() {
		SetDefaults();
		SetupUIValues();
		RenderValues();
	}

	private void RenderValues() {
        disName.text = name;
        disFlavorText.text = flavorText;
        disDmg.text = dmg.ToString();
        disHp.text = durability.ToString();
        disCost.text = cost.ToString();
    }

    private void SetDefaults() {
        cardPrefab = weaponData.cardPrefab;
        cardClass = weaponData.cardClass;
        sprite = weaponData.sprite;
        name = weaponData.name;
        flavorText = weaponData.flavorText;
        dmg = (int)weaponData.dmg;
        durability = (int)weaponData.durability;
        cost = (int)weaponData.cost;
        cardEffect = weaponData.cardEffect;
    }

}
