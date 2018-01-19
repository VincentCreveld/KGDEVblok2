public class MonsterBehaviour : CardBehaviour {
    public MonsterCard monsterData;

    public int hp;
	public int dmg;
	public int cost;
	public MonsterCard.CardEffects cardEffect;

	public void SetupCardValues() {
		SetDefaults();
		SetupUIValues();
		RenderValues();
	}

	private void RenderValues() {
        disName.text = name;
        disFlavorText.text = flavorText;
        disDmg.text = dmg.ToString();
        disHp.text = hp.ToString();
        disCost.text = cost.ToString();
    }

    private void SetDefaults() {
        cardPrefab = monsterData.cardPrefab;
        cardClass = monsterData.cardClass;
        sprite = monsterData.sprite;
        name = monsterData.name;
        flavorText = monsterData.flavorText;
        dmg = (int)monsterData.dmg;
        hp = (int)monsterData.hp;
        cost = (int)monsterData.cost;
        cardEffect = monsterData.cardEffect;
    }

}
