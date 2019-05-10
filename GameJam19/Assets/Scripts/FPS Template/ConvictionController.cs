using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ConvictionController : Photon.MonoBehaviour {

    public FloatVar dmgMult;

    private float transformTime = 2f;

    private int goodness = 0;
    private int badness = 0;

    public Text levelText;
    public Slider goodSlider;
    public Slider badSlider;
    private Color sliderBgColor = new Color(0.674f, 0.937f, 1f);
    private Color sliderGlowColor = new Color(0.968f, 0.986f, 0.219f);

    public IntVariable level;
    private int goodLvl = 1;
    private int badLvl = 1;

    [SerializeField] IntVariable bonusMaxHP;

    bool transformed = false;
    Queue<float> toBeTransformed = new Queue<float>();

    private void Start() {
        if (!photonView.isMine) {
            goodSlider.transform.parent.parent.gameObject.SetActive(false);
            enabled = false;
            return;
        }
        level.Value=1;

        goodSlider.maxValue = 1f;
        goodSlider.value = 0f;
        goodSlider.wholeNumbers = false;

        badSlider.maxValue = 1f;
        badSlider.value = 0f;
        badSlider.wholeNumbers = false;

        GenerateConviction(true, 20);
        GenerateConviction(false, 20);
    }

    public void GenerateConviction(bool good, int amount) {
        if (good) {
            goodness += amount * goodLvl;
        } else {
            badness += amount * badLvl;
        }
        RedrawBars();
    }

    private void RedrawBars() {
        Vector2 temp = new Vector2(goodness, badness);
        temp.Normalize();
        float str = (goodness + badness) / (level.Value * 100f);
        goodSlider.value = temp.x * str;
        badSlider.value = temp.y * str;
        if (goodSlider.value >= 1f) {
            LevelUp(true);
        } else if (badSlider.value >= 1f) {
            LevelUp(false);
        }
    }

    private void LevelUp(bool good) {
        goodness = badness = 0;
        level.Value++;
        levelText.text = level.Value.ToString();
        transform.localScale *= 1.5f;
        GetComponent<FirstPersonController>().m_RunSpeed *= 1.35f;
        GetComponent<FirstPersonController>().m_WalkSpeed *= 1.35f;
        GetComponent<FirstPersonController>().m_JumpSpeed *= 1.35f;
        GetComponent<PlayerHealth>().GainMaxHealth( (level.Value - 1) * bonusMaxHP.Value);
        dmgMult.value += (level.Value - 1) * (bonusMaxHP.Value * 0.9f);
        RenderSettings.fogEndDistance += level.Value * 10;

        if (good) {
            badLvl++;
            toBeTransformed.Enqueue(transformTime * goodLvl);
            TransformationQueuer();
            if (badLvl >= goodLvl + 3) {
                badSlider.transform.Find("Background").GetComponent<Image>().color = sliderGlowColor;
            } else {
                goodSlider.transform.Find("Background").GetComponent<Image>().color = sliderBgColor;
            }
        } else {
            goodLvl++;
            toBeTransformed.Enqueue(-transformTime * badLvl);
            TransformationQueuer();
            if (goodLvl >= badLvl + 3) {
                goodSlider.transform.Find("Background").GetComponent<Image>().color = sliderGlowColor;
            } else {
                badSlider.transform.Find("Background").GetComponent<Image>().color = sliderBgColor;
            }
        }
        GenerateConviction(good, 25);
    }

    private void TransformationQueuer() {
        if (transformed || toBeTransformed.Count == 0) {
            return;
        }
        float next = toBeTransformed.Dequeue();
        if (next > 0f) {
            //gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            GetComponent<FirstPersonController>().m_GravityMultiplier = 0f;
            StartCoroutine(TransformBack(next));
        } else {
            next *= -1f;
            //gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            GetComponent<PlayerHealth>().invincible = true;
            StartCoroutine(TransformBack(next));
        }
    }

    IEnumerator TransformBack(float timer) {
        transformed = true;
        yield return new WaitForSeconds(timer);
        //gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        GetComponent<FirstPersonController>().m_GravityMultiplier = 2f;
        GetComponent<PlayerHealth>().invincible = false;
        transformed = false;
        TransformationQueuer();
    }
}
