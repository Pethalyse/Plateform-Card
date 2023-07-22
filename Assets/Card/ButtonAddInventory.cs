using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAddInventory : MonoBehaviour
{
    [SerializeField]
    private GestionnaireJoueur gestionnaireJoueur;
    [SerializeField]
    private BattleControleur battleControleur;
    private int teamIndex;
    private bool canClick = false;

    private TextMeshProUGUI textButton;

    private void Awake()
    {
        textButton= GetComponentInChildren<TextMeshProUGUI>();
    }

    public void addInv()
    {
        if (canClick)
        {
            gestionnaireJoueur.addToInventory(battleControleur.teamEnemy[teamIndex]);
        }
    }

    public void setTeamIndex(int teamIndex) { this.teamIndex = teamIndex; textButton.text = battleControleur.teamEnemy[teamIndex].GetPersonnageData().persoName; canClick = true;}
}
