using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Schema;

public class CardBehaviour : MonoBehaviour{

	protected GameObject cardPrefab;
	public CardData.ClassLimitation cardClass;
	public CardData.CardType cardType;
	protected Sprite sprite;
	public string name;
	public string flavorText;

    protected Image disClassLimitation;
    protected Text disName;
    protected Text disFlavorText;
    protected Text disDmg;
    protected Text disHp;
    protected Text disCost;

    protected Color warriorColor;
    protected Color mageColor;
    protected Color rogueColor;

    protected void SetupUIValues() {
        warriorColor = new Color(0.478f, 0.117f, 0.117f);
        mageColor = new Color(0.137f, 0.482f, 0.627f);
        rogueColor = new Color(0.091f, 0.102f, 0.110f);
        foreach(Transform c in transform) {
            switch(c.name) {
                case "Name":
                    disName = c.GetChild(0).GetComponent<Text>();
                    break;
                case "Sprite":
                    c.GetComponent<Image>().color = Color.white;
                    c.GetComponent<Image>().sprite = sprite;
                    break;
                case "FlavorText":
                    disFlavorText = c.GetChild(0).GetComponent<Text>();
                    break;
                case "Damage":
                    disDmg = c.GetChild(0).GetComponent<Text>();
                    break;
                case "Health":
                    disHp = c.GetChild(0).GetComponent<Text>();
                    break;
                case "Cost":
                    disCost = c.GetChild(0).GetComponent<Text>();
                    break;
                case "Class":
                    disClassLimitation = c.GetComponent<Image>();
                    switch(cardClass) {
                        case CardData.ClassLimitation.none:
                            break;
                        case CardData.ClassLimitation.warrior:
                            disClassLimitation.color = warriorColor;
                            break;
                        case CardData.ClassLimitation.mage:
                            disClassLimitation.color = mageColor;
                            break;
                        case CardData.ClassLimitation.rogue:
                            disClassLimitation.color = rogueColor;
                            break;
                    }
                    break;
                case "Background":
                    break;
                default:
                    Debug.Log("Illegal object in card prefab");
                    break;
            }
            
        }
    }
}


