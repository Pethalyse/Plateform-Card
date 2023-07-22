using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class CardManagerStock : CardManager
{
    [SerializeField]
    private bool onTeam = false;
    private static int page;
    private Dictionary<int, List<PersonnageControleur>> dico;
    public static bool dicoChange = true;


    private void Update()
    {
        if (!onTeam)
        {
            if (dicoChange) { setIsInit(false); dicoChange = false; }
            if (!getIsInit())
            {
                dico = gestionnaireJoueur.getStockMap();
                Debug.Log(page);
                if (dico[page].Count > getTeamPlayerIndex())
                {
                    init(dico[page][getTeamPlayerIndex()].GetPersonnageData());
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
           

        }
        else
        {
            if (gestionnaireJoueur.getTeamJoueur().Count > getTeamPlayerIndex())
            {
                if (!getIsInit())
                {
                    init(gestionnaireJoueur.getTeamJoueur()[getTeamPlayerIndex()].GetPersonnageData());
                }
            }

        }

    }

    public void onButtonClicked()
    {
        if (cardManagerSelec == null && !onTeam)
        {
            cardManagerSelec = this;
        }
        else if (cardManagerSelec != null && onTeam)
        {
            //setTeamPlayerIndex(cardManagerSelec.getTeamPlayerIndex());

            setIsInit(false);

            gestionnaireJoueur.addToTeam(dico[page][cardManagerSelec.getTeamPlayerIndex()]);
            gestionnaireJoueur.removeFromTeam(gestionnaireJoueur.getTeamJoueur()[getTeamPlayerIndex()]);

            dicoChange = true;

            cardManagerSelec = null;
        }
    }

    public static void changePage(int i) { page = i; }

}