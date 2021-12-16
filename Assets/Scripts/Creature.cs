using System.Collections;
using UnityEngine;
using neat;

public class Creature : MonoBehaviour
{
    private float score = 0;
    public bool dead = false;
    private bool deadByFloor = false;

    public bool testMode = false;
    public float testVal;

    public float freq = 10f;

    public GameObject center;
    public Client brainClient;

    public Rigidbody2D[] refBones;
    public MuscleActivator[] muscles;
    public Patte[] contacts;

    private int creatureNumber=0;

    private int inputsNumber;
    private int outputsNumber;

    public float[] lastInputs;
    public float[] lastOutputs;
    public int[] tilesWatched = new int[] { 2, 3, 4 }; //vision à 2,3,4 tuiles devant
    public int nbTilesWatched = 3;

    public bool creatureDisplayed = true;

    public float inity;
    public float[] initAngles;



    private float BRAINPERIOD = GameManager.BRAINDELAY;

    public void Start()
    {
        if (testMode) { Begin(); }
    }
    public void Begin()
    {
        initAngles = new float[refBones.Length];
        tilesWatched = new int[] { 2, 3, 4 };
        inputsNumber = 5*refBones.Length + contacts.Length + tilesWatched.Length+ muscles.Length;
        outputsNumber = muscles.Length;

        float inity = transform.position.y;

        lastOutputs = new float[outputsNumber];
            for (var i = 0; i < muscles.Length; i++)
            {
                muscles[i].Begin();
                muscles[i].Set_number(i);
                muscles[i].Set_data(freq);
                muscles[i].SizeToMaxPercent(0.4f);
            }
        if (!testMode) { StartCoroutine(BrainActivity()); }
        for (int i=0; i < refBones.Length; i++)
        {
            initAngles[i] = refBones[i].transform.eulerAngles.z;
        }
    }

    // Update is called once per frame
    void Update()
    {
        score = center.transform.position.x>0 ? center.transform.position.x: 0f;
        if (testMode)
        {
            for (var i = 0; i < muscles.Length; i++)
            {
                muscles[i].SizeToMaxPercent(testVal);
            }
        }
    }

    private IEnumerator BrainActivity()
    {
        while (!dead)
        {
                var wait = new WaitForSeconds(BRAINPERIOD);
            if (!testMode)
            {
                lastOutputs = brainClient.calculate(GetInputs());
                yield return wait;
            }          
            FeedOutputs(lastOutputs);
        }
        
    }


    public void kill(bool floorKill=false)
    {
        StopAllCoroutines();
        dead = true;
        deadByFloor = floorKill;
        score = center.transform.position.x > 0 ? center.transform.position.x : 0f;
        this.gameObject.SetActive(false);
        GameManager.still_alive--;
    }

    public float GetScore() => score;
    public bool GetDead() => dead;
    public int GetNumber() => creatureNumber;
    public void SetNumber(int nb) => creatureNumber = nb;

    public int[] GetShape() {
        inputsNumber = 5 * refBones.Length + contacts.Length + nbTilesWatched+muscles.Length;
        outputsNumber = muscles.Length;
        return new int[2] { inputsNumber, outputsNumber };
    }
    public float[] GetInputs()
    {
        /***
         * Inputs (normalisés entre -1 et 1) :
         * - Par os de référence
         *      - Angle par rapport à l'angle initial
         *      - Vitesse horizontale
         *      - Vitesse verticale
         *      - Altitude
         *      - Vitesse de rotation
         * - Par contact :
         *      - est en contact (1) ou pas (-1)
         * - Par case de vision :
         *      - Différence d'altitude par rapport au centre de la créature
         * - Par muscle :
         *      - taille relative
         */
        float[] values = new float[inputsNumber];
        for (int i = 0; i < refBones.Length; ++i)
        {
            values[5*i] = (refBones[i].transform.eulerAngles.z-initAngles[i])/80;
            values[5*i+1] = refBones[i].velocity.x/10;
            values[5*i + 2] = refBones[i].velocity.y/10;
            values[5*i+3] = (refBones[i].transform.position.y-FloorManager.Altitude((float) refBones[i].transform.position.x, creatureNumber))/FloorManager.trackWidth;
            values[5*i+4] = (refBones[i].angularVelocity) / 80;
        }
        for (int i = 0; i < contacts.Length; i++) { values[5*refBones.Length+i] = 2*System.Convert.ToSingle(contacts[i].isFloored)-1; }
        for (int i = 0; i < tilesWatched.Length; i++) { values[5 * refBones.Length + contacts.Length + i] = (FloorManager.Altitude((float) (center.transform.position.x + tilesWatched[i]*FloorManager.résolution*FloorManager.taille_origine), creatureNumber)- center.transform.position.y)/FloorManager.trackWidth; }
        for (int i=0; i< muscles.Length; i++) { values[5 * refBones.Length + contacts.Length +tilesWatched.Length+ i] = (muscles[i].muscle.distance - muscles[i].min_length )/ muscles[i].max_length; }
        lastInputs = values;
        return values;
    }

    //public float Remap(float x) => Mathf.Pow(x, 3); // A changer si l'on veut une autre fonction de mappage
    public float Remap(float x) => x;

    public void FeedOutputs(float[] outputs)
    {
        for (var i = 0; i < muscles.Length; i++)
        {
            muscles[i].SizeMinusPercent(Remap(outputs[i]), GameManager.MUSCLEDELAY);
        }
    }

    public void Display(bool disp)
    {
        foreach (Renderer rd in GetComponentsInChildren<SpriteRenderer>())
        {
            rd.enabled = disp;
        }
        foreach (MuscleActivator m in muscles)
        {
            m.displayed = disp;
        }
    }
}
