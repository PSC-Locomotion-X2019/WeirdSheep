using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloorManager : MonoBehaviour
{
    static public float angle_max = 15f;
    static public int nb_tuiles_instanciées_t0 = 3;
    public GameObject GroundTile;
    public GameObject DirtTile;
    public GameObject Personnage;
    static public float cache_sol = 20f;
    static public float résolution = 5f;

    static public GameObject[] tableau_Pistes;
    public static float trackWidth = 40f;

    int nb_tuiles = 0;
    float y = 0f;
    public static float taille_origine;


    void Add(float r)
    {
        Vector3 nouvelle_Position = new Vector3(nb_tuiles * résolution * taille_origine, y, 0f);
        for (int i = 0; i < tableau_Pistes.Length; i++)
        {
            GameObject sol = Instantiate(GroundTile, tableau_Pistes[i].transform);
            sol.name = "Sol n° " + nb_tuiles;
            GroundTile g = sol.GetComponent<GroundTile>();
            float angle = (2 * r - 1) * angle_max;
            g.SetAngle(angle);
            g.Initialize(résolution);
            if (angle > 0)
            {
                y += g.GetSize_y() / 2;
            }
            else
            {
                y -= g.GetSize_y() / 2;
            }
            sol.GetComponent<Transform>().position += new Vector3(nb_tuiles * résolution * taille_origine, y, 0f);
            GameObject dirt=Instantiate(DirtTile, sol.transform);
            dirt.transform.position -= new Vector3(0,g.GetSize_y(),0);
            if (angle > 0)
            {
                y -= g.GetSize_y() / 2;
            }
            else
            {
                y += g.GetSize_y() / 2;
            }
            if (i == tableau_Pistes.Length - 1)
            {
                if (angle > 0)
                {
                    y += g.GetSize_y();
                }
                else
                {
                    y -= g.GetSize_y();
                }
            }
        }
        nb_tuiles++;
    }

    // Start is called before the first frame update
    public void Initialize()
    {
        nb_tuiles = 0;
        y = 0;
        taille_origine = GroundTile.GetComponent<GroundTile>().GetSize_x();
        for (int i = 0; i < nb_tuiles_instanciées_t0; i++)
        {
            this.Add(0.45f);
        }
    }

    static public float Altitude(int n, int num_creature){
        //donne la hauteur de la enième tuile

        Transform t = tableau_Pistes[num_creature+1].GetComponent<Transform>().Find("Sol n° "+n.ToString());
        if(t==null) { return 0.0f; }
        float Y = t.position.y ;
        GroundTile g= tableau_Pistes[num_creature+1].GetComponentsInChildren<GroundTile>()[n] ;
        float taille_y = g.GetSize_y() ;
        int x = 1;
        if(g.GetScale_x()<0){
            x=-1;
        }
        return (Y+taille_y/(résolution)*x) ;
    }

    static public float Altitude(float x, int num_creature){
        float r = x/(résolution*taille_origine);
        int n = (int)UnityEngine.Mathf.Floor(r) ;
        float y1 = Altitude(n,num_creature);
        float y2 = Altitude(n+1,num_creature);
        return y1+(r-n)*(y2-y1) ; 
    }


    // Update is called once per frame
    void Update()
    {
        Transform trans = Personnage.GetComponent<Transform>();
        if (trans.position[0] > nb_tuiles * résolution - cache_sol)
        {
            this.Add(Random.value);
        }
    }
}