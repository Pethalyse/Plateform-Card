using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControleur : MonoBehaviour
{

    [SerializeField]
    private PersonnageData PersoData;

    private string nom;
    private string description;
    private GameObject sprite;

    private int health;
    private int damage;
    //private int speed;
    private int mouvements;

    public int healthNow;
    public int mouvementsNow;

    private int[] competences;

    public GameObject onPlate;

    void Start()
    {
        if (PersoData != null) { Load(PersoData); }
    }
    private void Load(PersonnageData data)
    {
        nom = data.persoName;
        description = data.persoDescription;
        health = data.persoHealth;
        damage = data.persoDamage;
        //speed = data.persoSpeed;
        competences = data.persoCompetences;
        mouvements = data.mouvements;

        healthNow = health;
        mouvementsNow = mouvements;

        sprite = data.persoPrefab;
    }

}
