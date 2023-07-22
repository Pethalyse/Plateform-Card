using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BattleControleur : MonoBehaviour
{

    [SerializeField]
    private GestionnaireJoueur playerStock;
    private static int PMMax = 10;
    public int PMNow = PMMax;
    public List<PersonnageControleur> teamJoueur;
    public List<PersonnageControleur> teamEnemy;
    public List<PersonnageControleur> teamJoueurClone;
    public List<PersonnageControleur> teamEnemyClone;
    public List<PersonnageControleur> teamJoueur_Enemy;
    [HideInInspector]
    public int nbPersoInstance = 0;

    //private List<GameObject> prioritySpeed;

    public PersonnageControleur persoSelectionne;
    public int indexPersoSelectionne = 0;

    [SerializeField]
    private GameObject plate;
    private PlateformControleur plateformControleur;
    public State etatJeux = State.InMenu;

    //camera
    [SerializeField]
    private MouvementsCamera cam;

    [HideInInspector]
    public HashSet<PlateformControleur> plateOnRange = new HashSet<PlateformControleur>();

    public bool plateHover = false;

    [SerializeField]
    private List<GameObject> instanceCardPlayer = new List<GameObject>();
    [SerializeField]
    private List<GameObject> instanceCardEnemy = new List<GameObject>();

    private void Awake()
    {
        playerStock = GameObject.Find("playerControleur").GetComponent<GestionnaireJoueur>();
        plateformControleur = plate.GetComponent<PlateformControleur>();
        etatJeux = State.InMenu;
    }

    // Start is called before the first frame update
    void Start()
    {
        //persoSelectionne = teamJoueur[indexPersoSelectionne];

        //spawn enemy
        

    }

    // Update is called once per frame
    void Update()
    {
        if(etatJeux == State.BeforePreparation)
        {
            
            teamJoueur = playerStock.getTeamJoueur();


            //faire que les x premiers spawn dans le champs de la caméra

            plateformControleur.createPlateau(transform.position);
            cam.mouveCamOnVector(transform.position + transform.up * 10);

            /*foreach (PersonnageControleur go in teamJoueur)
            {
                PersonnageControleur g = Instantiate(go);
                g.SetActive(false);
            }*/

            instanciatePersonnage();

            foreach (GameObject go in instanceCardPlayer)
            {
                go.SetActive(false);
            }
            for (int i = 0; i < teamJoueurClone.Count; i++)
            {
                instanceCardPlayer[i].SetActive(true);
            }

            /*foreach (GameObject go in instanceCardPlayer)
            {
                go.SetActive(false);
            }*/

            etatJeux = State.Preparation;
        }
        else if (etatJeux == State.Preparation)
        {
            persoSelectionne = teamJoueurClone[indexPersoSelectionne];
            actualisationColor();
            
            /*if (Input.GetKeyDown("f"))
            {
                if (indexPersoSelectionne == teamJoueurClone.Count - 1)
                {
                    indexPersoSelectionne = 0;
                }
                else
                {
                    indexPersoSelectionne++;
                }

                persoSelectionne = teamJoueurClone[indexPersoSelectionne];
            }*/

            if (Input.GetKeyDown("d"))
            {

                etatJeux = State.Mouvements;
                cam.mouveCamOnVector(persoSelectionne.transform.position);

            }

        }
        else if(etatJeux != State.InMenu)
        {

            actualisationColor();

            if (teamEnemyClone.Count == 0) 
            {
                etatJeux = State.FinDeGame; Debug.Log("Fin du niveau");
                foreach (GameObject go in instanceCardPlayer)
                {
                    go.SetActive(true);
                }
            }

            if (Input.GetKeyDown("z") || PMNow == 0)
            {
                PMNow = PMMax;
                foreach (PersonnageControleur go in teamJoueurClone)
                {
                    go.resetMouvementNow();
                }
            }

            /*if (Input.GetKeyDown("f"))
            {
                if (indexPersoSelectionne == teamJoueurClone.Count - 1)
                {
                    indexPersoSelectionne = 0;
                }
                else
                {
                    indexPersoSelectionne++;
                }

                persoSelectionne = teamJoueurClone[indexPersoSelectionne];
                cam.mouveCamOnVector(persoSelectionne.transform.position);
            }*/

            //cam follow le perso selectionné
            if(Input.GetKey("space"))
            {
                cam.mouveCamOnVector(persoSelectionne.transform.position);
            }

            if (Input.GetKeyDown("g"))
            {
                if(etatJeux == State.Mouvements)
                {
                    etatJeux = State.Attaques;
                }
                else
                {
                    etatJeux = State.Mouvements;
                }
            }

            //Actions etat mouvements
            if (etatJeux == State.Mouvements)
            {
                //affichage de la range de mouvement
                if (!plateHover) { plateOnRange = persoSelectionne.plateOnRange(persoSelectionne.mouvementsNow); }

                foreach (PlateformControleur plate in plateOnRange)
                {
                    plate.changeColor(new Color(1f, 0, 0, 0.5882353f));
                }

            }
            //Actions etat attaques
            else if (etatJeux == State.Attaques && PMNow >= persoSelectionne.AACost)
            {
                //affichage de la range d'attaque
                plateOnRange = persoSelectionne.plateOnRange(persoSelectionne.AARange);

                foreach (PlateformControleur plate in plateOnRange)
                {
                    plate.changeColor(new Color(0, 0, 1f, 0.5882353f));
                }

                foreach(PersonnageControleur p in teamEnemyClone)
                {
                    if (plateOnRange.Contains(p.onPlate))
                    {
                        p.onPlate.changeColor(new Color(1f, 0f, 1f, 0.4f));
                    }
                }
            }
            else if (etatJeux == State.Attaques && (PMNow <= persoSelectionne.AACost || PMNow == 0))
            {
                plateOnRange.Clear();
            }

            
        }

    }

    private void actualisationColor()
    {
        foreach (PersonnageControleur p in teamJoueurClone)
        {
            p.onPlate.changeColor(new Color(0f, 1f, 1f, 0.4f));
        }
        foreach (PersonnageControleur p in teamEnemyClone)
        {
            p.onPlate.changeColor(new Color(1f, 1f, 0f, 0.4f));
        }

        persoSelectionne.onPlate.changeColor(new Color(0f, 1f, 0f, 0.4f));
    }

    private void instanciatePersonnage()
    {

        List<PlateformControleur> allPlate = plateformControleur.getAllPlate();

        foreach (PersonnageControleur go in teamEnemy)
        {
            do
            {
                int index = Random.Range(0, allPlate.Count);
                if (!allPlate[index].occupe)
                {
                    PersonnageControleur g = go.instanciation(allPlate[index].transform.position, allPlate[index]);
                    teamEnemyClone.Add(g);
                    teamJoueur_Enemy.Add(g);
                    break;
                }

            }while(true);
        }

        foreach (PersonnageControleur go in teamJoueur)
        {
            do
            {
                int index = Random.Range(0, allPlate.Count);
                if (!allPlate[index].occupe)
                {
                    PersonnageControleur g = go.instanciation(allPlate[index].transform.position, allPlate[index]);
                    teamJoueurClone.Add(g);
                    teamJoueur_Enemy.Add(g);
                    break;
                }

            } while (true);
        }
    }

    /*
    public List<GameObject> priorityPlaysTeamJoueur(List<GameObject> player, List<GameObject> enemy)
    {

        List<GameObject> save = new List<GameObject>(player);
        save.AddRange(enemy);
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < player.Count+enemy.Count; i++)
        {
            GameObject s = moreSpeed(save);
            result.Add(s);
            save.Remove(s);
        }

        return result;


    }

        private GameObject moreSpeed(List<GameObject> teamJoueurV)
    {

        bool v = false;

        foreach (GameObject go in teamJoueurV)
        {
            v = false;
            GameObject s = GameObject.Find(go.name + "(Clone)");
            foreach (GameObject go2 in teamJoueurV)
            {
                GameObject s2 = GameObject.Find(go2.name + "(Clone)");
                if (go != go2 && s.GetComponent<PersonnageControleur>().getSpeed() < s2.GetComponent<PersonnageControleur>().getSpeed())
                {
                    v = true;
                    break;
                }
            }

            if (!v) { return go; }
        }

        return null;
    }*/

    public void mortAlliee(PersonnageControleur go){if (teamJoueurClone.Contains(go)){teamJoueurClone.Remove(go);}}
    public void mortEnemy(PersonnageControleur go) { if (teamEnemyClone.Contains(go)) {teamEnemyClone.Remove(go);}}

    public void switchBeforePreparation() { etatJeux = State.BeforePreparation; }
    public void switchInMenu() { etatJeux = State.InMenu; }

    public void changePersoSelect(int indice)
    {
        if(teamJoueurClone.Count > indice)
        {
            indexPersoSelectionne = indice;
            persoSelectionne = teamJoueurClone[indexPersoSelectionne];
            cam.mouveCamOnVector(persoSelectionne.transform.position);
        }
    }

}

public enum State
{
    Preparation,
    TourEnemy,
    Mouvements,
    Attaques,
    InMenu,
    BeforePreparation,
    FinDeGame
}


