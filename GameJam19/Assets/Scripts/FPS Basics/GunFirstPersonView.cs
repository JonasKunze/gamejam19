using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class GunFirstPersonView : MonoBehaviour {

public Weapon weapon;
    float timeBetweenBullets = 0.2f;
     float range = 100.0f;
    public Transform moveme;

    private float timer;
    private Ray shootRay;
    private RaycastHit shootHit;
    private ParticleSystem gunParticles;
    private LineRenderer gunLine;
    Vector3 originalpos;
    Quaternion orginalrot;

    Quaternion randomrot;
    bool fire;

    public GameEvent shootevent;
    // Called when script awake in editor
    void Awake() {
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        originalpos=moveme.transform.localPosition;
        orginalrot=moveme.transform.localRotation;
        //updateRecoil();
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;

        bool shooting = CrossPlatformInputManager.GetButton("Fire1");

        if (shooting && timer >= timeBetweenBullets && Time.timeScale != 0) {
            
            timeBetweenBullets = 1f/((float)weapon.AtkPerSec+0.000001f);
            Shoot();
        }

        //anim.SetBool("Firing", shooting);
    }
    private void FixedUpdate()
    {
        if(fire){
            FireMoveAndRot(2);
            fire=false;
        }else {
            moveme.localPosition = Vector3.Lerp(moveme.localPosition,originalpos,0.5f);
            moveme.localRotation = Quaternion.Slerp(moveme.localRotation,orginalrot,0.5f);
        }
    }

    // Disable the shooting effects
    public void DisableEffects() {
        gunLine.enabled = false;
    }

    // Shoot!
    void Shoot() {
        // set weapon depending stuff:
        range=weapon.range;
        timer = 0.0f;
        gunParticles.Stop();
        gunParticles.Play();
        //recoil
        randomrot=Random.rotation;
        fire=true;
        shootevent.Raise();
        //StartCoroutine(updateRecoil());
    }
    IEnumerator updateRecoil (){
        //randomrot.y=Quaternion.Euler(0,0,0).y;

        //moveme.localPosition-=Vector3.forward*weapon.damage;
        FireMoveAndRot(1);
        yield return new WaitForSeconds(timeBetweenBullets*.1f);
        FireMoveAndRot(1);
        yield return new WaitForSeconds(timeBetweenBullets*.1f);
        for (int i =0;i<8;i++){
            moveme.localPosition = Vector3.Lerp(moveme.localPosition,originalpos,0.3f);
            moveme.localRotation = Quaternion.Slerp(moveme.localRotation,orginalrot,0.3f);
            yield return new WaitForSeconds(timeBetweenBullets*.1f);
        }
        moveme.localPosition=originalpos;
        moveme.localRotation=orginalrot;
        yield return null;
    }
    void FireMoveAndRot(){
        FireMoveAndRot(1f);
    }
    void FireMoveAndRot(float power){
        moveme.localPosition-=Vector3.forward*weapon.damage*0.01f*power;
        moveme.localRotation = Quaternion.Slerp(moveme.localRotation,randomrot,power*0.1f*weapon.damage/(weapon.damage+200));
    }

}

