using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fov_change_on_click : MonoBehaviour {

    public Weapon weapon;
    float fov,fovadd;
    [Range(0, 170f)]
    public float fovstart = 60f;
    [Range(0,0.5f)]
    public float strength = 0.05f;
    [Range(-10, 10f)]
    public float add = 1f;
    Camera cam;

    public AudioSource audi;
    public AudioClip clipperino;

	// Use this for initialization
	void Start () {
        if (GetComponent<Camera>())
        {
            cam = GetComponent<Camera>();
            fovstart = cam.fieldOfView;
        }


    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Camera>())
        {
            if (cam.fieldOfView < 170 && Input.anyKeyDown)
            {
                cam.fieldOfView += add;
               // if (audi) audi.PlayOneShot(clipperino, SoundManager.getSoundVolume());
            }

                if (((add>0)&&(cam.fieldOfView - fovstart)>2)|| ((add < 0) && (cam.fieldOfView - fovstart) < -2)) cam.fieldOfView=Mathf.Lerp(cam.fieldOfView, fovstart, strength);
            
        }
	}
}
