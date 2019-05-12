using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NoodleActivator : MonoBehaviour
{
    public GameObject noodleWeapon;
    public GameObject model;

    private IEnumerator coroutine;

    private Quaternion startRotation;
    public Vector3 targetRotation;
    
    
    private void Start()
    {
        startRotation = model.transform.localRotation;
    }

    void Update()
    {
        model.SetActive(noodleWeapon.gameObject.GetActive());  
    }

    public void StartAnimation()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = null;

        coroutine = Animate();
        StartCoroutine(coroutine);
    }

    private IEnumerator Animate()
    {
        model.transform.localRotation = startRotation;

        float startTime = 0f, animTime = .25f;

        float percent = 0f;

        while (percent < 1)
        {
            percent = startTime / animTime;
            percent = Mathf.Clamp01(percent);

            model.transform.localRotation =
                Quaternion.Lerp(model.transform.localRotation, Quaternion.Euler(targetRotation), percent);
            
            startTime += Time.deltaTime;
            
            yield return null;
        }

        startTime = percent = 0;
        
        while (percent < 1)
        {
            percent = startTime / animTime;
            percent = Mathf.Clamp01(percent);

            model.transform.localRotation =
                Quaternion.Lerp(model.transform.localRotation, Quaternion.identity, percent);
            
            startTime += Time.deltaTime;
            
            yield return null;
        }
        
        model.transform.localRotation  = Quaternion.identity;
    }
}
