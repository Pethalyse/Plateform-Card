using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class invocStockage : MonoBehaviour
{

    public List<CardManagerStock> stocks = new List<CardManagerStock>();
    public Dictionary<int, List<PersonnageControleur>> dico;
    public List<Button> changePages;
    private static int page = 0;

    private void Awake()
    {
        foreach(Transform child in transform)
        {
            stocks.Add(child.GetComponent<CardManagerStock>());
        }

        dico = stocks[0].gestionnaireJoueur.getStockMap();
    }

    // Start is called before the first frame update
    void Start()
    {
        int l = 0;
        foreach(CardManagerStock child in stocks)
        {
            child.setTeamPlayerIndex(l); l++;
            
        }
        
    }

    void Update()
    {
        changePages[1].interactable = dico.Count > page+1 ? true : false;
        changePages[0].interactable = page > 0 ? true : false;
    }

    public void changePage(int i)
    {
        page += i;
        dico = stocks[0].gestionnaireJoueur.getStockMap();
        CardManagerStock.dicoChange = true;
        CardManagerStock.changePage(page);

    }
}
