using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{

    public int teamIndex;
    public BattleControleur battleControleur;
    public GestionnaireJoueur gestionnaireJoueur;

    public static CardManager cardManagerSelec = null;

    private GameObject prefabCard;
    private Sprite splashART;
    public string nom;
    private string description;
    private int health;
    private int damage;
    private int mouvements;
    private int range;
    private int AACost;
    private int[] competences;

    private bool isInit = false;

    [SerializeField]
    private Image compenentSplashART;
    [SerializeField]
    public TextMeshProUGUI compenentMouvement;
    [SerializeField]
    private TextMeshProUGUI compenentName;
    [SerializeField]
    public TextMeshProUGUI compenentHealth;
    [SerializeField]
    public TextMeshProUGUI compenentCost;
    [SerializeField]
    public TextMeshProUGUI compenentDamage;

    private void Awake()
    {
        battleControleur= GameObject.Find("battleControleur").GetComponent<BattleControleur>();
        gestionnaireJoueur = GameObject.Find("playerControleur").GetComponent<GestionnaireJoueur>();
    }

    public void init(PersonnageData data)
    {

        splashART = data.splashART;

        nom = data.persoName;
        description = data.persoDescription;
        health = data.persoHealth;
        damage = data.persoDamage;
        mouvements = data.mouvements;
        range = data.persoAARange;
        AACost = data.persoAACost;
        competences= data.persoCompetences;

        compenentSplashART.sprite = splashART;
        compenentName.text = nom;
        compenentMouvement.text = ""+mouvements;
        compenentCost.text = ""+AACost;
        compenentHealth.text = "" + health;
        compenentDamage.text = "" + damage;

        isInit= true;

    }

    public int getTeamPlayerIndex() { return teamIndex; }
    public void setTeamPlayerIndex(int i) { teamIndex = i; }

    public bool getIsInit() { return isInit; }
    public void setIsInit(bool isInit) { this.isInit = isInit; }

}
