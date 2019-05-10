using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    [Header("Tutorial")]
    public bool showTutorial = true;
    public Text[] tutTexts;
    
    void Start () {
        tutTexts[0].transform.parent.gameObject.SetActive(true);
        foreach (Text t in tutTexts) {
            t.enabled = false;
        }
        if (showTutorial) {
            StartCoroutine(RunTutorial());
        }
    }

    IEnumerator RunTutorial() {
        tutTexts[0].enabled = true;
        for (int i = 1; i < tutTexts.Length; i++) {
            while (!Input.anyKeyDown) {
                yield return null;
            }
            tutTexts[i - 1].enabled = false;
            tutTexts[i].enabled = true;
            while (Input.anyKeyDown) {
                yield return null;
            }
        }
        while (!Input.anyKeyDown) {
            yield return null;
        }
        tutTexts[tutTexts.Length - 1].enabled = false;
    }
}
