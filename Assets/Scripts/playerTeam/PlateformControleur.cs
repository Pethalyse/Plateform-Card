using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlateformControleur : MonoBehaviour
{

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private BattleControleur battleControleur;
    private PlateformControleur plateformControleur;
    private Renderer rendererPlate;

    private PersonnageControleur persoSelectionne;
    public bool occupe = false;

    [SerializeField]
    public List<PlateformControleur> voisins = new List<PlateformControleur>();

    //statics
    private static int nbMaxPlat = 50;
    private static List<Vector3> allPlateVectors = new List<Vector3>();
    private static List<PlateformControleur> allPlate = new List<PlateformControleur>();

    private void Awake()
    {
        plateformControleur = GetComponent<PlateformControleur>();
        rendererPlate = GetComponent<Renderer>();
        battleControleur = GameObject.Find("battleControleur").GetComponent<BattleControleur>();
    }

    void Start()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.1f)) { voisins.Add(hit.transform.GetComponent<PlateformControleur>()); }
        if (Physics.Raycast(transform.position, -transform.forward, out hit, 1.1f)) { voisins.Add(hit.transform.GetComponent<PlateformControleur>()); }
        if (Physics.Raycast(transform.position, transform.right, out hit, 1.1f)) { voisins.Add(hit.transform.GetComponent<PlateformControleur>()); }
        if (Physics.Raycast(transform.position, -transform.right, out hit, 1.1f)) { voisins.Add(hit.transform.GetComponent<PlateformControleur>()); }

    }

    void Update()
    {
        //recup le perso selectionne
        persoSelectionne = battleControleur.persoSelectionne;

        //colorie la plateform en gris si la case n'as aucun lieu avec l'etat du jeu et le personnage selectionne
        if (!battleControleur.plateOnRange.Contains(plateformControleur) && !occupe)
        {
            changeColor(Color.gray);
        }

        if (battleControleur.teamJoueur_Enemy != null) {

            //verifie si le plateau est vide ou non
            foreach (PersonnageControleur gO in battleControleur.teamJoueur_Enemy)
            {
                if (gO.onPlate == plateformControleur)
                {
                    occupe = true;
                    break;
                }
                else
                {
                    occupe = false;
                }
            }
        }
    }

    private void OnMouseDown()
    {

        //phase de placements des personnages
        if (battleControleur.etatJeux == State.Preparation)
        {
            foreach (PersonnageControleur gO in battleControleur.teamJoueurClone)
            {
                if (gO != persoSelectionne)
                {
                    if (gO.onPlate == plateformControleur)
                    {
                        gO.mouvement(persoSelectionne.onPlate, persoSelectionne.onPlate.transform.position);
                        persoSelectionne.mouvement(plateformControleur, transform.position);
                        break;
                    }

                }
            }
        }

        //phase de jeux
            //MouvementsCamera
        else if(!occupe && battleControleur.etatJeux == State.Mouvements)
        {

            if (persoSelectionne.mouvementsNow > 0)
            {

                if (battleControleur.plateOnRange.Contains(plateformControleur))
                {
                    //eviter une reduction trop grande de PM dû a une vitesse de click trop grande
                    System.Threading.Thread.Sleep(10);
                    battleControleur.PMNow -= battleControleur.plateOnRange.Count;
                    persoSelectionne.mouvementsNowMoins(battleControleur.plateOnRange.Count);
                    persoSelectionne.mouvement(plateformControleur, transform.position);

                }
            }
        }
            //attaques
        else if(battleControleur.etatJeux == State.Attaques && battleControleur.PMNow >= persoSelectionne.AACost)
        {

            if (occupe)
            {

                foreach (PersonnageControleur go in battleControleur.teamJoueur_Enemy)
                {
                    if (go.onPlate == plateformControleur)
                    {
                        persoSelectionne.dealDamage(go);

                    }
                }
            }

            battleControleur.PMNow -= persoSelectionne.AACost;
        }
    }

    private void OnMouseExit()
    {
        //a enlever peut-etre
        System.Threading.Thread.Sleep(30);
        battleControleur.plateHover = false;
    }

    private void OnMouseOver()
    {
        if (battleControleur.etatJeux != State.InMenu && battleControleur.etatJeux != State.BeforePreparation && battleControleur.etatJeux != State.Preparation)
        {


            if (battleControleur.etatJeux == State.Mouvements && battleControleur.plateOnRange.Contains(plateformControleur) && persoSelectionne.mouvementsNow > 0)
            {
                battleControleur.plateHover = true;
                battleControleur.plateOnRange = persoSelectionne.plateOnRange(persoSelectionne.AARange);
                battleControleur.plateOnRange = cheminVersPerso(persoSelectionne.mouvementsNow);
                battleControleur.plateOnRange.Remove(persoSelectionne.onPlate);
            }
            else{ battleControleur.plateHover = false; }
        }
    }

    public void createPlateau(Vector3 pos)
    {
        if (nbMaxPlat == 0)
        {
        }
        else
        {

            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            PlateformControleur g = go.GetComponent<PlateformControleur>();
            allPlateVectors.Add(pos);
            allPlate.Add(g);
            nbMaxPlat--;

            List<string> card = new List<string>();

            if (!ifCooExist(new Vector3((float)(pos.x + 1.1), pos.y, pos.z)))
            {
                card.Add("r");
            }
            if (!ifCooExist(new Vector3((float)(pos.x - 1.1), pos.y, pos.z)))
            {
                card.Add("l");
            }
            if (!ifCooExist(new Vector3(pos.x, pos.y, (float)(pos.z + 1.1))))
            {
                card.Add("f");
            }
            if (!ifCooExist(new Vector3(pos.x, pos.y, (float)(pos.z - 1.1))))
            {
                card.Add("b");
            }

            if (card.Count > 0)
            {
                switch (card[Random.Range(0, card.Count)])
                {
                    case "r":
                        {
                            g.createPlateau(new Vector3((float)(pos.x + 1.1), pos.y, pos.z));
                            break;
                        }
                    case "l":
                        {
                            g.createPlateau(new Vector3((float)(pos.x - 1.1), pos.y, pos.z));
                            break;
                        }
                    case "f":
                        {

                            g.createPlateau(new Vector3(pos.x, pos.y, (float)(pos.z + 1.1)));

                            break;
                        }
                    case "b":
                        {
                            g.createPlateau(new Vector3(pos.x, pos.y, (float)(pos.z - 1.1)));
                            break;
                        }
                }
            }
        }
    }

    private bool ifCooExist(Vector3 vec)
    {
        foreach(Vector3 v in allPlateVectors)
        {
            float xDistance = Mathf.Abs(v.x - vec.x);
            float zDistance = Mathf.Abs(v.z - vec.z);
            if (xDistance < 1 && zDistance < 1) { return true; }
        }

        return false;
    }

    public List<PlateformControleur> getAllPlate()
    {
        return allPlate;
    }

    public void setOccupe(bool value){occupe = value;}

    public HashSet<PlateformControleur> cheminVersPerso(int mouvementsNow, PlateformControleur plateLook = null)
    {
        HashSet<PlateformControleur> result = new HashSet<PlateformControleur>();
        
        if(plateLook == null)
        {
            plateLook = plateformControleur;
        }

        if(plateLook != persoSelectionne.onPlate && mouvementsNow == 0)
        {
            return new HashSet<PlateformControleur>();
        }
        result.Add(plateLook);
        
        for(int i=0; i<plateLook.voisins.Count(); i++)
        {
            if (battleControleur.plateOnRange.Contains(plateLook.voisins[i]))
            {
                HashSet<PlateformControleur> list = plateLook.voisins[i].cheminVersPerso(mouvementsNow - 1);
                if (list.Count >= 1 && !list.Contains(plateLook)){result.AddRange(list);  return result;}
            }
            else if (plateLook.voisins[i] == persoSelectionne.onPlate)
            {
                result.Add(plateLook.voisins[i]);
                return result;
            }
        }

        if(result.Count == 1) { result.Remove(plateLook); }
        

        return result;
    }

    public PlateformControleur getPlateformControleur() { return plateformControleur; } 
    public Renderer getRenderer() { return rendererPlate;}

    internal void changeColor(Color color)
    {
        rendererPlate.material.color = color;
    }
}
