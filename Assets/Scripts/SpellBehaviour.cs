public class SpellBehaviour : CardBehaviour {
    public SpellCard spellData;

	public int dmg;
	public int cost;
	public SpellCard.SpellType spellType;

	public void SetupCardValues() {
		SetDefaults();
		SetupUIValues();
		RenderValues();
	}

    private void RenderValues() {
        disName.text = name;
        disFlavorText.text = flavorText;
        disCost.text = cost.ToString();
    }

    private void SetDefaults() {
        cardPrefab = spellData.cardPrefab;
        cardClass = spellData.cardClass;
        sprite = spellData.sprite;
        name = spellData.name;
        flavorText = spellData.flavorText;
        dmg = (int)spellData.dmg;
        cost = (int)spellData.cost;
        spellType = spellData.spellType;
    }

}
