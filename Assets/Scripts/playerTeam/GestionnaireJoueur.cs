using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GestionnaireJoueur : MonoBehaviour
{
    [SerializeField]
    private List<PersonnageControleur> teamJoueur;
    [SerializeField]
    private List<PersonnageControleur> inventoryJoueur;

    private void Start()
    {
        Debug.Log(teamJoueur[1]);
        Debug.Log(inventoryJoueur[3]);
    }

    public List<PersonnageControleur> getTeamJoueur() { return teamJoueur; }
    public List<PersonnageControleur> getInventoryJoueur() { return inventoryJoueur; }
    public void addToInventory(PersonnageControleur personnage) { inventoryJoueur.Add(personnage); }

    public void addToTeam(PersonnageControleur personnage)
    {
        if (inventoryJoueur.Contains(personnage))
        {
            teamJoueur.Add(personnage);
        }
    }

    public void removeFromTeam(PersonnageControleur personnage) { teamJoueur.Remove(personnage); }

    public Dictionary<int, List<PersonnageControleur>> getStockMap()
    {
        Dictionary<int, List<PersonnageControleur>> map = new Dictionary<int, List<PersonnageControleur>>();
        int i = 0;
        int l = 0;

        foreach(PersonnageControleur p in inventoryJoueur)
        {
            bool b = false;
            if (teamJoueur.Contains(p)) 
            {
                if(!(l == 0)){l--;}
                b=true; 
                Debug.Log("ok"); 
            }

            if (!b)
            {
                if (!map.ContainsKey(i))
                {
                    map[i] = new List<PersonnageControleur> { p };
                }
                else
                {
                    map[i].Add(p);
                }

                l++;
                if(l == 20) { l = 0; i++; }
                
            }

        }
            

        return map;
    }

}
