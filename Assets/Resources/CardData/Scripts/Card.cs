using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Card : MonoBehaviour {

    //This class functions as the bootstrapper for the XML-to-Unity conversion

    //Get source image from XML
    private Sprite sprite;

    //Get as text from XML name is used to pair the card to the values
    private string name = "";
    private string flavorText = "";

    //Convert from flag
    private CardValues cardValues;
    private int oDmg;   //damage from init
    private int oHp;    //health from init
    private int oCost;  //cost from init

    //Values for Gameplay
    private int dmg;    //current damage(with modifiers)
    private int hp;     //current health
    private int cost;   //current cost

    //Card display references
    private Transform cardParent;
    private Text disName;
    private Text disFlavorText;
    private Text disDmg;
    private Text disHp;
    private Text disCost;

    private bool isSetup = false;

    private AudioSource audiosource;

    private void Awake() {
        cardValues = new CardValues();
        audiosource = GetComponent<AudioSource>();
        cardParent = gameObject.transform;
        foreach(Transform c in transform) {
            switch(c.name) {
                case "Name":
                    disName = c.GetChild(0).GetComponent<Text>();
                    break;
                case "Sprite":
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
                case "Background":
                    break;
                default:
                    Debug.Log("Illegal object in card prefab");
                    break;
            }
        }
    }

    private void Start() {
        SetupCard();
    }

    public void SetupCard() {
        //Get data from XML (input scriptable obj??)
        //Initialise card values
        //Update empty card prefab
        //Set cardvalues
        isSetup = true;
    }

    public CardValues GetValues() {
        return cardValues;
    }

    public bool IsSetup() {
        return isSetup;
    }
}

public struct CardValues {
    int damage;
    int health;
}