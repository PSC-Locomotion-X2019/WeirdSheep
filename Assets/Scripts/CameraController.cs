using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class CameraController : MonoBehaviour
{
    public GameManager gm;

    public GameObject creature_center = null;
    
    public int focused_creature=0;
    private int champion;
    public bool champ_only=false;
    public bool display_muscle=true;
    private Vector3 offset;
    public Vector3 init;

    public Text scoreDisplay;
    public Text IODisplay;
    public Text speedDisplay;

    // Start is called before the first frame update
    public void Start()
    {
        init = transform.position;
        offset = transform.position - creature_center.transform.position;
    }


    private void Update()
    {
        if (champ_only && gm.oldChamp!=focused_creature) { changeFocus(gm.oldChamp); }
        if (champ_only && gm.clones[focused_creature].GetDead()) { champ_only = false; }
        if (creature_center!=null && !gm.clones[focused_creature].GetDead()) {
            transform.position = new Vector3(creature_center.transform.position.x + offset.x, init.y + offset.y, init.z);
            //gm.clones[focused_creature].DisplayMuscles(display_muscle);
        }
        else { nextCreature(+1); }

        champion = gm.score.ToList().IndexOf(gm.score.Max());
        gm.floorM.Personnage = gm.clones[champion].center;
        scoreDisplay.text = gm.clones[focused_creature].GetScore().ToString() + " m\nCréature " + focused_creature.ToString() + " Génération n° " + gm.generationNumber.ToString() + "\nChampion : " + champion.ToString() + "\nScore : " + gm.clones[focused_creature].GetScore().ToString();
        IODisplay.text = "Inputs :" + string.Join(" ", gm.clones[focused_creature].lastInputs) + "\nOutputs : " + string.Join(" ", gm.clones[focused_creature].lastOutputs);

    }



    public void changeFocus(int creatureToWatch)
    {
        if (0 <= creatureToWatch && creatureToWatch < gm.clones.Length)
        {
            if (gm.clones[focused_creature] != null) { gm.clones[focused_creature].Display(false); }
            focused_creature = creatureToWatch;
            creature_center = gm.clones[creatureToWatch].center;
            init.y = -FloorManager.trackWidth * focused_creature;
            gm.clones[focused_creature].Display(true);
        }
    }
    public void changeFocusToChamp()
    {
        changeFocus(gm.score.ToList().IndexOf(gm.score.Max()));
    }
    public void nextCreature(int direction)
    {
        if (GameManager.still_alive == 0 || gm.clones==null) { return; }
        if (!champ_only || gm.clones[champion].GetDead())
        {
            int j = 1;
            while (gm.clones[ReevaluateNumber(focused_creature + gm.nbToSpawn + j*direction)].GetDead() && j<gm.nbToSpawn) { j++; }
            changeFocus(ReevaluateNumber(focused_creature + gm.nbToSpawn + j * direction));
        }
    }
    private int ReevaluateNumber(int i)
    {
        return (int) (i - gm.nbToSpawn * Mathf.FloorToInt(i / gm.nbToSpawn));
    }
    public void toggleChampOnly() { 
        champ_only = !champ_only;
        if (champ_only) { changeFocus(gm.oldChamp); }
    }
    public void toggleMuscleDisplay()
    {
        display_muscle = !display_muscle;
    }

    public void faster(float factor)
    {
        gm.timeScale *= factor;
        speedDisplay.text = gm.timeScale.ToString();
    }
}
