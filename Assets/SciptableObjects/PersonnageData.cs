using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PersonnageData", menuName = "My Game/Personnage Data")]
public class PersonnageData : ScriptableObject
{

    //card
    public string persoName;
    public string persoDescription;
    public GameObject persoPrefab;
    public Sprite splashART;


    //personnage
    public int persoHealth = 100;
    public int persoDamage = 10;
    public int mouvements = 1;
    public int persoAARange = 2;
    public int persoAACost = 3;

    public int[] persoCompetences = new int[3];
}
