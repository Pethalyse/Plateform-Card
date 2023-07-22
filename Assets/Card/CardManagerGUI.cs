using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManagerGUI : CardManager
{
    [SerializeField]
    private bool cardEnemy = false;
    [SerializeField]
    private ButtonAddInventory buttonAddInventory;
    // Start is called before the first frame update
    void Start()
    {
        if (cardEnemy) { teamIndex = Random.Range(0, battleControleur.teamEnemy.Count); }
    }

    // Update is called once per frame
    void Update()
    {
        if (!cardEnemy)
        {
            if (battleControleur.teamJoueurClone.Count > teamIndex)
            {
                if (!getIsInit()) { init(battleControleur.teamJoueur[teamIndex].GetPersonnageData()); }
                else { update(battleControleur.teamJoueurClone[teamIndex]); }
                if (teamIndex == battleControleur.indexPersoSelectionne) { cardManagerSelec = this; }
            }
            
        }
        else
        {
            if (!getIsInit()) { init(battleControleur.teamEnemy[teamIndex].GetPersonnageData()); }
        }
    }

    public void changePos()
    {
        Vector3 v = gameObject.transform.position;
        gameObject.transform.position = cardManagerSelec.transform.position;
        cardManagerSelec.transform.position = v;

        battleControleur.changePersoSelect(teamIndex);
    }

    public virtual void update(PersonnageControleur pC)
    {
        compenentMouvement.text = "" + pC.mouvementsNow;
        compenentCost.text = "" + pC.AACost;
        compenentHealth.text = "" + pC.healthNow;
        compenentDamage.text = "" + pC.damage;
    }

    public void changeButtonIndex()
    {
        buttonAddInventory.setTeamIndex(teamIndex);
    }
}
