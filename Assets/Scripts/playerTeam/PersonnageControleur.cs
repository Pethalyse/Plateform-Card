using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PersonnageControleur : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private PersonnageData PersoData;
    private PersonnageControleur personnageControleur;
    private BattleControleur battleControleur;


    private int PMNow;

    public string nom;
    public string description;

    [SerializeField]
    private int health;
    public int healthNow;
    [SerializeField]
    public int damage;
    //private int speed;
    [SerializeField]
    private int mouvements;
    public int mouvementsNow;

    public int AARange;
    public int AACost;

    private int[] competences;

    public PlateformControleur onPlate;

    private void Awake()
    {
        if (PersoData != null) { Load(PersoData); }
        personnageControleur = GetComponent<PersonnageControleur>();
        battleControleur = GameObject.Find("battleControleur").GetComponent<BattleControleur>();

    }

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

        if(healthNow <= 0) { estMort(); }

        PMNow = battleControleur.PMNow;
        if(PMNow < mouvementsNow) { mouvementsNow = PMNow; }

    }

    public PersonnageControleur instanciation(Vector3 gO, PlateformControleur plate = null)
    {

        if(plate == null)
        {
            GameObject c = Instantiate(prefab);
            PersonnageControleur clone = c.GetComponent<PersonnageControleur>();
            clone.SetActive(false);
            return clone;
        }
        else
        {
            onPlate = plate;
            plate.setOccupe(true);

            GameObject c = Instantiate(prefab, gO, Quaternion.identity);
            PersonnageControleur clone = c.GetComponent<PersonnageControleur>();
            return clone;
        }


    }

    public void mouvement(PlateformControleur plate, Vector3 gO)
    {
        onPlate = plate;
        plate.setOccupe(true);
        transform.position = gO;
    }

    private void Load(PersonnageData data)
    {
        nom = data.persoName;
        description = data.persoDescription;
        health = data.persoHealth;
        damage = data.persoDamage;
        competences = data.persoCompetences;
        AARange = data.persoAARange;
        AACost = data.persoAACost;

        mouvements = data.mouvements;

        healthNow = health;
        mouvementsNow = mouvements;

        prefab = data.persoPrefab;

    }

    public void mouvementsNowMoins(int nb){mouvementsNow-=nb;}

    public void resetMouvementNow() { mouvementsNow = mouvements; }

    public HashSet<PlateformControleur> plateOnRange(int range)
    {

        HashSet<PlateformControleur> plates = new HashSet<PlateformControleur>();

        for(int i=0; i < range; i++) 
        {

            if(plates.Count == 0)
            {
                foreach (PlateformControleur voisin in onPlate.voisins)
                {
                    if (voisin.occupe)
                    {
                        if (battleControleur.etatJeux == State.Attaques)
                        {
                            foreach (PersonnageControleur g in battleControleur.teamEnemyClone)
                            {
                                if (g.onPlate == voisin)
                                {
                                    plates.Add(voisin);
                                }
                            }
                        }
                    }
                    else { plates.Add(voisin); }
                }
            }
            else
            {

                bool v = false;
                HashSet<PlateformControleur> save = new HashSet<PlateformControleur>(plates);
                foreach (PlateformControleur plateVoisins in save)
                {
                    v = false;

                    if (battleControleur.etatJeux == State.Attaques)
                    {
                        foreach (PersonnageControleur g in battleControleur.teamEnemyClone)
                        {
                            if (g.onPlate == plateVoisins)
                            {
                                v = true;
                            }
                        }
                    }

                    if (!v)
                    {
                        foreach (PlateformControleur voisin in plateVoisins.voisins)
                        {
                            if (!voisin.occupe) { plates.Add(voisin); }
                            else if (battleControleur.etatJeux == State.Attaques)
                            {
                                foreach (PersonnageControleur g in battleControleur.teamEnemyClone)
                                {
                                    if (g.onPlate == voisin)
                                    {
                                        plates.Add(voisin);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        return plates;

    }

    private void takeDamage(int damage) { healthNow -= damage; }

    public void dealDamage(PersonnageControleur personnage)
    {
        personnage.takeDamage(damage);
    }

    private void estMort()
    {
        battleControleur.mortAlliee(personnageControleur);
        battleControleur.mortEnemy(personnageControleur);
        gameObject.SetActive(false);
   
    }

    internal void SetActive(bool v)
    {
        gameObject.SetActive(v);
    }


    internal PersonnageControleur getPersonnageControleur() { return personnageControleur; }

    public PersonnageData GetPersonnageData() { return PersoData; }
}
