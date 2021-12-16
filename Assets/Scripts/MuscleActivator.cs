using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class MuscleActivator : MonoBehaviour
{
    public float size = 100;
    private int number;

    private GameObject jointed_bone;
    public SpringJoint2D muscle;

    private int steps;
    private float natural_length;
    public float max_length;
    public float min_length;

    private LineRenderer line;
    public Color relax_color = Color.green;
    public Color flex_color = Color.red;
    private Color current_color = Color.red;
    private bool started = false;
    public bool byPassAutoSize = false;
    public bool displayed = true;

    // Start is called before the first frame update

    
    public void Begin()
    {
        steps = 10;
        muscle = this.gameObject.GetComponent<SpringJoint2D>();
        jointed_bone = muscle.connectedBody.gameObject;
        if (!byPassAutoSize)
        {
            max_length = (jointed_bone.transform.lossyScale.y + transform.lossyScale.y);
            max_length *= 0.9f;
            min_length = 0.105f * max_length / 0.85f;
        }    
        start_line();
        started = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (displayed) { draw_line(); };
        if (muscle.distance > max_length) { muscle.distance = max_length; }
        if (muscle.distance < min_length) { muscle.distance = min_length; }
    }

    public void Set_number(int i) => number = i;
    public int Get_number() => number;

    public void Set_data(float freq) { muscle.frequency = freq; muscle.dampingRatio = 1.0f; muscle.breakForce = float.MaxValue; }

    private void start_line()
    {
        line = this.gameObject.AddComponent<LineRenderer>();
        line.material = Resources.Load("myMaterial", typeof(Material)) as Material;
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;
        line.positionCount = 2;
        line.startColor = current_color;
        line.endColor = current_color;
    }
    private void draw_line()
    {
        if (jointed_bone != null)
        {
            line.SetPosition(0, gameObject.transform.position);
            line.SetPosition(1, jointed_bone.transform.position);
            line.startColor = current_color;
            line.endColor = current_color;
        }
    }

    public void SizeMinusPercent(float ratio_decrement, float time)
    {
        float length_decrement = ratio_decrement * (max_length-min_length);
        float goal_length = muscle.distance - length_decrement;
        if (min_length > goal_length) { goal_length = min_length; }
        if (max_length < goal_length) { goal_length = max_length; }
        StartCoroutine(SpringContract(length_decrement));
    }
    public void SizeToMaxPercent(float ratio)
    {
        float goal_length = (max_length-min_length) * ratio + min_length;
        if (min_length > goal_length) { goal_length = min_length; }
        if (max_length < goal_length) { goal_length = max_length; }
        StartCoroutine(SpringContract( muscle.distance - goal_length));
    }


    private IEnumerator SpringContract(float deltal)
    {
        float dt = (float)(GameManager.MUSCLEDELAY) * 0.001f / (float)steps;
        float dl = deltal / steps;
        for (int t = 0; t < steps; t++)
        {
            muscle.distance -= dl;
            yield return new WaitForSeconds((float)(dt));
        }
    }
}