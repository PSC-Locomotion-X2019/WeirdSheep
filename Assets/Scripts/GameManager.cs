    using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using neat;
using System.IO;

public class GameManager : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform trackPoint;
    public DeathWallController wall;
    public FloorManager floorM;

    public Creature creaPrefab;
    public GameObject pistePrefab;
    public int nbToSpawn=10;

    public Creature[] clones;
    private Client[] generation;
    public float[] score;

    public int oldChamp = 0;

    public static int still_alive;

    public int generationNumber=0;

    public int INPUTSNUMBER;
    public int OUTPUTSNUMBER;

    public float timeScale = 5;
    public static float BASICTIMESTEP =0.0001f;
    public static float BRAINDELAY =2*BASICTIMESTEP;
    public static float MUSCLEDELAY =2*BASICTIMESTEP;
    public static float WALLSPEED =2f;

    public Neat n;


    private void Start()
    {
        still_alive = nbToSpawn;
        INPUTSNUMBER = creaPrefab.GetShape()[0];
        OUTPUTSNUMBER = creaPrefab.GetShape()[1];
        n = new Neat(INPUTSNUMBER, OUTPUTSNUMBER, nbToSpawn);
        Debug.Log(INPUTSNUMBER +" "+ OUTPUTSNUMBER);
        generation = n.GetGeneration();
        //SaveManager.instance.activeSave = new PreSaveData("SansNom", n);
        InstantiateSimulation(true);
        //for (int i = 0; i < 10; i++) { InstantiateSimulation(false); }
    }


    public void InstantiateSimulation(bool first=false)
    {
        if (!first)
        {
            oldChamp= score.ToList().IndexOf(score.Max());
            generation =n.NextGeneration(score);
            //SaveManager.instance.activeSave.neat = n;
            for (int i = 0; i < FloorManager.tableau_Pistes.Length;i++)
            {
                if (i<nbToSpawn && clones[i] != null)
                {
                    Destroy(clones[i].gameObject);
                }
                Destroy(FloorManager.tableau_Pistes[i]);
            }
        }

        still_alive = nbToSpawn;
        score = new float[nbToSpawn];
        clones = new Creature[nbToSpawn];
        FloorManager.tableau_Pistes = new GameObject[nbToSpawn+1]; // + plafond
        FloorManager.tableau_Pistes[0] = Instantiate(pistePrefab, trackPoint.transform.position + new Vector3(0f, FloorManager.trackWidth, 0), new Quaternion(), trackPoint);
        
        for (int i = 0; i < nbToSpawn; i++)
        {
            Vector3 shift = new Vector3(0f, FloorManager.trackWidth* i, 0);
            FloorManager.tableau_Pistes[i+1] = Instantiate(pistePrefab, trackPoint.transform.position - shift, new Quaternion(), trackPoint );
            clones[i] = Instantiate(creaPrefab, spawnPoint.transform.position- shift, new Quaternion(), spawnPoint);

            clones[i].name = "Creature " + i.ToString();
            clones[i].brainClient = generation[i];
            clones[i].SetNumber(i);
            clones[i].Display(false);

            //foreach (Collider2D selfCollider in clones[i].gameObject.GetComponentsInChildren<Collider2D>())
            //{
            //    for(int j = 0; j < i; j++)
            //    {
            //        foreach (Collider2D otherCollider in clones[j].gameObject.GetComponentsInChildren<Collider2D>())
            //        {
            //            Physics2D.IgnoreCollision(selfCollider, otherCollider);
            //        }
            //    }
            //}
        }
        floorM.Initialize();
        for (int i = 0; i < nbToSpawn; i++)
        {
            clones[i].Begin();
        }
        spawnPoint.GetComponent<SpriteRenderer>().enabled=false;
        trackPoint.GetComponent<SpriteRenderer>().enabled = false;
        wall.transform.localScale+=new Vector3(0,(10 * (nbToSpawn + 1) * FloorManager.trackWidth),0);
        wall.Begin();
        clones[0].Display(true);
        generationNumber++;
    }

    void Update()
    {
        Time.timeScale = timeScale;
        for (int i = 0; i < nbToSpawn; i++)
        {
            if (!clones[i].GetDead())
            {
                score[i] = clones[i].GetScore();
            }
        }
        if (still_alive<=0)
        {
            Debug.Log(n.bestscore());
             Debug.Log(n.size());
            //if (generationNumber%5==0){
            //    n.Savebestclient(generationNumber);
            //}
            //Open the File
            StreamWriter sw = new StreamWriter("sol variable nb=200 ,grenouille",append: true);
            sw.WriteLine(n.bestscore());
            sw.Close();
            InstantiateSimulation(false);
        }
        
        //if (SaveManager.instance.hasLoaded)
        //{
        //    n = SaveManager.instance.activeSave.neat;

        //    for (int i = 0; i < FloorManager.tableau_Pistes.Length; i++)
        //    {
        //        if (i < nbToSpawn && clones[i] != null)
        //        {
        //            Destroy(clones[i].gameObject);
        //        }
        //        Destroy(FloorManager.tableau_Pistes[i]);
        //    }
        //    InstantiateSimulation(true); // On ne veut pas créer une nouvelle génération mais simplement recharger la génération qui est dans ActiveSave
        //    SaveManager.instance.hasLoaded = false;
        //}
        
    }


}
