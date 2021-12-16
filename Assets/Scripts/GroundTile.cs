using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private float angle = 45 ;  //angle entre -89 et 89 degrés et pas 0  
    private float scale_x = (float)1 ;  //origine
    private float scale_y = (float)1 ;  //origine
    private float size_y = 2f ; //origine
    private float size_x = 2f ; //origine

    public void SetAngle(float new_angle){
        this.angle = new_angle ;
    }

    public float GetSize_y(){
        return size_y;
    }

    public float GetSize_x(){
        return size_x;
    }

    public float GetScale_x(){
        return scale_x;
    }
    
    public void Initialize(float résolution){
        Transform transf = this.gameObject.GetComponent<Transform>() ;
        int x = 1 ;
	    if(angle<0){
            x = -1 ;
            angle = -1*angle ;
        }
        if (angle < 0.1f && angle > -0.1f)
        {
            angle = 1f;
        }
        transf.localScale = new Vector3(x*scale_x*résolution,(angle/45*scale_y)*résolution,1) ;
        scale_x *= x*résolution ;
        scale_y *= (float)(angle/45)*résolution ;
        size_y *= scale_y ;
        size_x *= scale_x ;
        //Debug.Log(" "+scale_x+" "+scale_y+" "+size_y);
    }

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {}

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!collision.collider.CompareTag("Patte"))
        {
            if (collision.gameObject.GetComponentInParent<Creature>() != null)
            {
                collision.gameObject.GetComponentInParent<Creature>().kill(true);
            }
        }
    }
}
