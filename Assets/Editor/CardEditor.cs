using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class CardEditor : EditorWindow {

	private static GameObject spellPrefab;
	private static GameObject monsterPrefab;
	private static GameObject weaponPrefab;

	private Color backgroundColor = new Color(0.85f, 0.85f, 0.85f);
	private Color defaultColor = new Color(0.9f, 0.9f, 0.9f);
	private Color warriorColor;
	private Color mageColor;
	private Color rogueColor;

	private Texture2D headerTexture;
	private Texture2D classBorderTexture; //change color based on class selected
	private Texture2D backgroundTexture;
	private Texture2D classBackgroundTexture;
	private Texture2D previewTexture;

	private static CardData currentCard;

	public enum SelectedCard { weapon, spell, monster }
	public static SelectedCard selectedCard = SelectedCard.weapon;

	public static WeaponCard weaponCard;
	public static SpellCard spellCard;
	public static MonsterCard monsterCard;

	private Rect headerRect;
	private Rect classBorderRect;
	private Rect backgroundRect;
	private Rect classBackgroundRect;
	private Rect buttonRect;
	private Rect previewRect;

	private bool isApplied = false;
	private Sprite lastSprite;

	[MenuItem("Window/Card Creator/Card Creator")]
	static public void OpenWindow() {
		CardEditor window = (CardEditor)GetWindow(typeof(CardEditor));
		window.minSize = new Vector2(300, 495);
		window.maxSize = new Vector2(400, 495);
		window.Show();
	}

	//Dit is de editor equivalent van Start en Awake
	private void OnEnable() {
		InitData();
		InitTextures();
	}

	//Dit is de editor equivalent van Update
	private void OnGUI() {
		DrawLayouts();
		DrawHeader();
		DrawBackground();
	}

	public static void InitData() {
		weaponCard = (WeaponCard)ScriptableObject.CreateInstance(typeof(WeaponCard));
		spellCard = (SpellCard)ScriptableObject.CreateInstance(typeof(SpellCard));
		monsterCard = (MonsterCard)ScriptableObject.CreateInstance(typeof(MonsterCard));
		currentCard = weaponCard;
		spellPrefab = Resources.Load<GameObject>("Prefabs/Spell") as GameObject;
		monsterPrefab = Resources.Load<GameObject>("Prefabs/Monster") as GameObject;
		weaponPrefab = Resources.Load<GameObject>("Prefabs/Weapon") as GameObject;

	}

	private void InitTextures() {
		warriorColor = new Color(0.478f, 0.117f, 0.117f);
		mageColor = new Color(0.137f, 0.482f, 0.627f);
		rogueColor = new Color(0.091f, 0.102f, 0.110f);

		headerTexture = new Texture2D(1, 1);
		headerTexture.SetPixel(0, 0, backgroundColor);
		headerTexture.Apply();

		classBorderTexture = new Texture2D(1, 1);
		classBorderTexture.SetPixel(0, 0, defaultColor);
		classBorderTexture.Apply();

		backgroundTexture = new Texture2D(1, 1);
		backgroundTexture.SetPixel(0, 0, backgroundColor);
		backgroundTexture.Apply();

		classBackgroundTexture = new Texture2D(1, 1);
		classBackgroundTexture.SetPixel(0, 0, backgroundColor);
		classBackgroundTexture.Apply();

		previewTexture = new Texture2D(1, 1);
		previewTexture.SetPixel(0, 0, Color.magenta);
		previewTexture.Apply();

	}

	private void DrawLayouts() {
		CheckTextures();

		headerRect.x = 5;
		headerRect.y = 5;
		headerRect.width = Screen.width - 10;
		headerRect.height = 90;

		previewRect.x = Screen.width - 110;
		previewRect.y = 10;
		previewRect.width = 100;
		previewRect.height = 80;

		classBorderRect.x = 0;
		classBorderRect.y = 0;
		classBorderRect.width = Screen.width;
		classBorderRect.height = Screen.height;

		backgroundRect.x = 5;
		backgroundRect.y = 100;
		backgroundRect.width = Screen.width - 10;
		backgroundRect.height = 190;

		classBackgroundRect.x = 5;
		classBackgroundRect.y = 295;
		classBackgroundRect.width = Screen.width - 10;
		classBackgroundRect.height = 145;

		buttonRect.x = 5;
		buttonRect.y = 445;
		buttonRect.width = Screen.width - 10;
		buttonRect.height = 45;

		DrawClassBorder();
		GUI.DrawTexture(backgroundRect, backgroundTexture);
		GUI.DrawTexture(classBackgroundRect, backgroundTexture);
		GUI.DrawTexture(headerRect, headerTexture);
		GUI.DrawTexture(previewRect, previewTexture);
		GUI.DrawTexture(buttonRect, headerTexture);

	}

	private void DrawHeader() {
		GUILayout.BeginArea(headerRect);
		GUILayout.Label("Card creator");

		switch(selectedCard) {
			case SelectedCard.weapon:
				currentCard = weaponCard;
				currentCard.cardPrefab = weaponPrefab;
				break;
			case SelectedCard.spell:
				currentCard = spellCard;
				currentCard.cardPrefab = spellPrefab;
				break;
			case SelectedCard.monster:
				currentCard = monsterCard;
				currentCard.cardPrefab = monsterPrefab;
				break;
		}
		GUILayout.EndArea();
	}

	private void DrawBackground() {
		GUILayout.BeginArea(backgroundRect);

		DrawCardSettings();

		GUILayout.EndArea();

		GUILayout.BeginArea(classBackgroundRect);
		switch(selectedCard) {
			case SelectedCard.weapon:
				DrawWeaponSettings();
				break;
			case SelectedCard.spell:
				DrawSpellSettings();
				break;
			case SelectedCard.monster:
				DrawMonsterSettings();
				break;
		}

		GUILayout.EndArea();

		GUILayout.BeginArea(buttonRect);

		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Reset all values", GUILayout.Height(30))) {
			InitData();
			InitTextures();
		}
		if(currentCard.sprite == null) {
			EditorGUILayout.HelpBox("To create a card you need a [Sprite].", MessageType.Warning);
		}
		else if(currentCard.name == null || currentCard.name.Length < 1) {
			EditorGUILayout.HelpBox("To create a card you need a [Name].", MessageType.Warning);
		}
		else if(GUILayout.Button("Create card", GUILayout.Height(30))) {
			SaveCard();
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.EndArea();
	}

	//Checks if any of the textures is null, then reinitialises them.
	private void CheckTextures() {
		if(!headerTexture || !classBorderTexture || !backgroundTexture || !classBackgroundTexture || !previewTexture) {
			InitTextures();
		}
	}

	//Changes the background color based on the selected class in the editor
	private void DrawClassBorder() {
		switch(currentCard.cardClass) {
			case CardData.ClassLimitation.none:
				classBorderTexture.SetPixel(0, 0, defaultColor);
				break;
			case CardData.ClassLimitation.rogue:
				classBorderTexture.SetPixel(0, 0, rogueColor);
				break;
			case CardData.ClassLimitation.warrior:
				classBorderTexture.SetPixel(0, 0, warriorColor);
				break;
			case CardData.ClassLimitation.mage:
				classBorderTexture.SetPixel(0, 0, mageColor);
				break;
			default:
				classBorderTexture.SetPixel(0, 0, defaultColor);
				break;
		}
		classBorderTexture.Apply();
		GUI.DrawTexture(classBorderRect, classBorderTexture);
	}

	//Displays all universal card input fields
	private void DrawCardSettings() {

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Class limitation");
		currentCard.cardClass = (CardData.ClassLimitation)EditorGUILayout.EnumPopup(currentCard.cardClass);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Card art");
		currentCard.sprite = EditorGUILayout.ObjectField(currentCard.sprite, typeof(Sprite), false) as Sprite;
		if(currentCard.sprite != lastSprite)
			isApplied = false;

		if(currentCard.sprite != null && !isApplied) {
			UpdatePreview(currentCard.sprite);
		}
		if(currentCard.sprite == null) {
			previewTexture = new Texture2D(1, 1);
			previewTexture.SetPixel(0, 0, Color.magenta);
			previewTexture.Apply();
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Label("Card name");
		currentCard.name = EditorGUILayout.TextField(currentCard.name);

		GUILayout.Label("Flavor Text");
		currentCard.flavorText = EditorGUILayout.TextField(currentCard.flavorText);

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Card Type");
		selectedCard = (SelectedCard)EditorGUILayout.EnumPopup(selectedCard);
		EditorGUILayout.EndHorizontal();
	}

	//Displays the settings for monster cards
	private void DrawMonsterSettings() {
		GUILayout.Label("Monster Card creator");
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Damage");
		monsterCard.dmg = Mathf.Abs(Mathf.RoundToInt(EditorGUILayout.FloatField(monsterCard.dmg)));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Health");
		monsterCard.hp = Mathf.Abs(Mathf.RoundToInt(EditorGUILayout.FloatField(monsterCard.hp)));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Cost");
		monsterCard.cost = Mathf.Abs(Mathf.RoundToInt(EditorGUILayout.FloatField(monsterCard.cost)));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Card effect");
		monsterCard.cardEffect = (MonsterCard.CardEffects)EditorGUILayout.EnumPopup(monsterCard.cardEffect);
		EditorGUILayout.EndHorizontal();

		currentCard = monsterCard;
		CopySettings();
	}

	//Displays the settings for spell cards
	private void DrawSpellSettings() {
		GUILayout.Label("Spell Card creator");
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Damage (negative damage heals)");
		spellCard.dmg = Mathf.RoundToInt(EditorGUILayout.FloatField(spellCard.dmg));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Cost");
		spellCard.cost = Mathf.Abs(Mathf.RoundToInt(EditorGUILayout.FloatField(spellCard.cost)));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Spell Type");
		spellCard.spellType = (SpellCard.SpellType)EditorGUILayout.EnumPopup(spellCard.spellType);
		EditorGUILayout.EndHorizontal();

		currentCard = spellCard;
		CopySettings();
	}

	//Displays the settings for weapon cards
	private void DrawWeaponSettings() {
		GUILayout.Label("Weapon Card creator");
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Damage");
		weaponCard.dmg = Mathf.Abs(Mathf.RoundToInt(EditorGUILayout.FloatField(weaponCard.dmg)));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Durability");
		weaponCard.durability = Mathf.Abs(Mathf.RoundToInt(EditorGUILayout.FloatField(weaponCard.durability)));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Cost");
		weaponCard.cost = Mathf.Abs(Mathf.RoundToInt(EditorGUILayout.FloatField(weaponCard.cost)));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Card effect");
		weaponCard.cardEffect = (WeaponCard.CardEffects)EditorGUILayout.EnumPopup(weaponCard.cardEffect);
		EditorGUILayout.EndHorizontal();

		currentCard = weaponCard;
		CopySettings();
	}

	//Equalizes the universal data across all cards
	private void CopySettings() {
		weaponCard.cardClass = currentCard.cardClass;
		weaponCard.sprite = currentCard.sprite;
		weaponCard.name = currentCard.name;
		weaponCard.flavorText = currentCard.flavorText;

		spellCard.cardClass = currentCard.cardClass;
		spellCard.sprite = currentCard.sprite;
		spellCard.name = currentCard.name;
		spellCard.flavorText = currentCard.flavorText;

		monsterCard.cardClass = currentCard.cardClass;
		monsterCard.sprite = currentCard.sprite;
		monsterCard.name = currentCard.name;
		monsterCard.flavorText = currentCard.flavorText;
	}

	//Saves of the scriptable object and prefab
	private void SaveCard() {
		switch(selectedCard) {
			case SelectedCard.weapon:
				GameObject weaponPrefab = SaveCard("Weapons/");
				if(weaponPrefab.GetComponent<WeaponBehaviour>() == null) {
					weaponPrefab.AddComponent<WeaponBehaviour>();
				}
				weaponPrefab.GetComponent<WeaponBehaviour>().weaponData = weaponCard;
				weaponPrefab.GetComponent<WeaponBehaviour>().SetupCardValues();

				break;
			case SelectedCard.spell:
				GameObject spellPrefab = SaveCard("Spells/");
				if(!spellPrefab.GetComponent<SpellBehaviour>()) {
					spellPrefab.AddComponent<SpellBehaviour>();
				}
				spellPrefab.GetComponent<SpellBehaviour>().spellData = spellCard;
				spellPrefab.GetComponent<SpellBehaviour>().SetupCardValues();

				break;
			case SelectedCard.monster:
				GameObject monsterPrefab = SaveCard("Monsters/");
				if(!monsterPrefab.GetComponent<MonsterBehaviour>()) {
					monsterPrefab.AddComponent<MonsterBehaviour>();
				}
				monsterPrefab.GetComponent<MonsterBehaviour>().monsterData = monsterCard;
				monsterPrefab.GetComponent<MonsterBehaviour>().SetupCardValues();

				break;
		}
	}

	private GameObject SaveCard(string path) {
		//Application.persistentDataPath
		string dataPath = "Assets/Resources/CardData/Data/" + path + currentCard.name + ".asset";
		AssetDatabase.CreateAsset(currentCard, dataPath);
		string resourcesPath = "Assets/Resources/CardData/Cards/" + path + currentCard.name + ".prefab";
		string prefabPath = AssetDatabase.GetAssetPath(currentCard.cardPrefab);
		AssetDatabase.CopyAsset(prefabPath, resourcesPath);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		return (GameObject)AssetDatabase.LoadAssetAtPath(resourcesPath, typeof(GameObject));
	}

	private void UpdatePreview(Sprite s) {
		//previewTexture = new Texture2D(100, 80);
		if(!isApplied) {
			previewTexture = s.texture;
			lastSprite = s;
			previewTexture.Apply();
			isApplied = true;
		}
	}
}

public class LoadStenXML : EditorWindow {

	public BlockDataContainer container;
	public int blockToLoad = 0;
	public int containerLength;
	public string currentBlockLoaded;
	private string resourcesPath;

	[MenuItem("Window/Card Creator/Load Sten's XML")]
	static public void OpenWindow() {
		LoadStenXML window = (LoadStenXML)GetWindow(typeof(LoadStenXML));
		window.minSize = new Vector2(200, 100);
		window.maxSize = new Vector2(200, 100);
		window.Show();
	}

	private void Awake() {
		resourcesPath = Application.dataPath + "/Resources";
	}

	private void OnEnable() {
		LoadCardData();
	}

	private void OnGUI() {
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Block To Load (0-" + (containerLength-1).ToString() + "): ");
		blockToLoad = Mathf.Abs(Mathf.RoundToInt(Mathf.Clamp(EditorGUILayout.FloatField(blockToLoad), 0, containerLength -1)));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		
		if(blockToLoad < containerLength) {
			currentBlockLoaded = container.createdBlockList[blockToLoad].name;
			GUILayout.Label("Block name: " + currentBlockLoaded);
		}
		else {
			GUILayout.Label("Block index out of range");
		}
		EditorGUILayout.EndHorizontal();
		if(blockToLoad > containerLength) {
			EditorGUILayout.HelpBox("Index Out of range!", MessageType.Warning);
		}
		else if(GUILayout.Button("Load Sten's XML to a card!", GUILayout.Height(20))) {
			CardEditor.OpenWindow();
			SetupReflection();
		}
	}

	public void LoadCardData() {
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(BlockDataContainer));
		string path = resourcesPath + "/XML_Sten/Block_XML.xml";
		FileStream fileStream = new FileStream(path, FileMode.Open);
		container = xmlSerializer.Deserialize(fileStream) as BlockDataContainer;
		containerLength = container.createdBlockList.Count;
		fileStream.Close();
	}

	private void SetupReflection() {
		LoadCardData();
		CardDataReflection reflection = new CardDataReflection();
		BlockData loadedBlock = container.createdBlockList[blockToLoad];
		reflection.name = loadedBlock.name;
		reflection.FlavorText = loadedBlock.cat;
		reflection.CardType = loadedBlock.tier;
		reflection.health = loadedBlock.xSize;
		reflection.damage = loadedBlock.ySize;
		reflection.cost = loadedBlock.tier;
		SubmitReflection(reflection);
	}

	private void SubmitReflection(CardDataReflection reflection) {
		switch(reflection.cardType) {
			case CardData.CardType.monster:
				CardEditor.selectedCard = CardEditor.SelectedCard.monster;
				CardEditor.monsterCard.dmg = reflection.damage;
				CardEditor.monsterCard.hp = reflection.health;
				CardEditor.monsterCard.name = reflection.name;
				CardEditor.monsterCard.sprite = reflection.sprite;
				CardEditor.monsterCard.cost = reflection.cost;
				CardEditor.monsterCard.flavorText = reflection.flavorText;
				break;
			case CardData.CardType.spell:
				CardEditor.selectedCard = CardEditor.SelectedCard.spell;
				CardEditor.spellCard.dmg = reflection.damage;
				CardEditor.spellCard.name = reflection.name;
				CardEditor.spellCard.sprite = reflection.sprite;
				CardEditor.spellCard.cost = reflection.cost;
				CardEditor.spellCard.flavorText = reflection.flavorText;
				break;
			case CardData.CardType.weapon:
				CardEditor.selectedCard = CardEditor.SelectedCard.weapon;
				CardEditor.weaponCard.dmg = reflection.damage;
				CardEditor.weaponCard.durability = reflection.health;
				CardEditor.weaponCard.name = reflection.name;
				CardEditor.weaponCard.sprite = reflection.sprite;
				CardEditor.weaponCard.cost = reflection.cost;
				CardEditor.weaponCard.flavorText = reflection.flavorText;
				break;
		}
	}
}

public class BlockDataContainer {
	[XmlArray("BlockDataArray")]
	[XmlArrayItem("BlockData")]
	public List<BlockData> createdBlockList = new List<BlockData>();
}

public class BlockData {
	//Type can be multiple
	[XmlAttribute("Name")]
	public string name;
	public int explosive;
	public int cat;
	public int sturdy;
	public int ghost;
	public int water;
	public int swap;
	public int slime;
	public int clock;
	//Size can be only one
	public int xSize;
	public int ySize;
	//Tier
	public int tier;
}

public class CardDataReflection {
	//Block name
	public string name;
	//block xSize
	public int health;
	//block tier
	public int cost;
	//block ySize
	public int damage;
	//block cat = 1
	public string flavorText;

	public int FlavorText {
		set {
			if(value == 0)
				flavorText = "Not meow";
			else
				flavorText = "Meow";
		}
	}
	//default filler image
	public Sprite sprite = Resources.Load<Sprite>(Application.dataPath + "/Resources/CardArt");

	//convert block tier to card type
	public CardData.CardType cardType;
	public int CardType {
		set {
			switch(value) {
				case 1:
					cardType = CardData.CardType.monster;
					break;
				case 2:
					cardType = CardData.CardType.monster;
					break;
				case 3:
					cardType = CardData.CardType.spell;
					break;
				case 4:
					cardType = CardData.CardType.spell;
					break;
				case 5:
					cardType = CardData.CardType.weapon;
					break;
				case 6:
					cardType = CardData.CardType.weapon;
					break;
				default:
					cardType = CardData.CardType.monster;
					break;
			}
		}
	}
}
