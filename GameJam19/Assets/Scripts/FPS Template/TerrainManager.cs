using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainManager : Photon.MonoBehaviour {

    [SerializeField] Transform loadingParent;
    Slider loadingSlider;

    public static int boundaries;
    [SerializeField] int worldSize = 10;
    [SerializeField] int smallDensity = 15;
    [SerializeField] int mediumDensity = 5;
    [SerializeField] int bigDensity = 1;
    GameObject[,] ground;
    public GameObject groundPrefab;
    public GameObject[] smallAssets;
    public GameObject[] mediumAssets;
    public GameObject[] bigAssets;

    Transform smallAssetsT, mediumAssetsT, bigAssetsT, terrainT;

    private void Start() {
        smallAssetsT = new GameObject("SmallAssets").transform;
        mediumAssetsT = new GameObject("MediumAssets").transform;
        bigAssetsT = new GameObject("BigAssets").transform;
        terrainT = new GameObject("Terrains").transform;
        boundaries = ((worldSize - 1) / 2) * 500;
    }

    public void Make() {
        photonView.RPC("InitializeLoader", PhotonTargets.All);
        StartCoroutine(Making());
    }


    IEnumerator Making() { 
/*

        for (int i = 0; i < worldSize; i++) {
            for (int j = 0; j < worldSize; j++) {
                //Instantiate(groundPrefab, new Vector3((i - worldSize / 2) * 500, 0f, (j - worldSize / 2) * 500), groundPrefab.transform.rotation, terrain);
                photonView.RPC("SpawnNewObject", PhotonTargets.All, "Ground", new Vector2((i - worldSize / 2) * 500, (j - worldSize / 2) * 500), 3);
                Vector2 from;
                from = new Vector2((i - worldSize / 2) * 500 - 250, (j - worldSize / 2) * 500 - 250);
                int amount = Random.Range(smallDensity, 2 * smallDensity);
                for (int k = 0; k < amount; k++) {
                    int index = Random.Range(0, smallAssets.Length - 1);
                    Vector2 pos = from;
                    pos.x += Random.value * 500f;
                    pos.y += Random.value * 500f;
                    photonView.RPC("SpawnNewObject", PhotonTargets.All, smallAssets[index].name, pos, 0);
                }

                amount = Random.Range(mediumDensity, mediumDensity);
                for (int k = 0; k < amount; k++) {
                    int index = Random.Range(0, mediumAssets.Length - 1);
                    Vector2 pos = from;
                    pos.x += Random.value * 500f;
                    pos.y += Random.value * 500f;
                    photonView.RPC("SpawnNewObject", PhotonTargets.All, mediumAssets[index].name, pos, 1);
                }

                amount = Random.Range(bigDensity, bigDensity);
                for (int k = 0; k < amount; k++) {
                    int index = Random.Range(0, bigAssets.Length - 1);
                    Vector2 pos = from;
                    pos.x += Random.value * 500f;
                    pos.y += Random.value * 500f;
                    photonView.RPC("SpawnNewObject", PhotonTargets.All, bigAssets[index].name, pos, 2);
                }

                photonView.RPC("LoadedPart", PhotonTargets.All);
                yield return new WaitForSeconds(0.01f);
            }
        }*/
        yield return null;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            Vector3 dest = player.transform.position;
            dest.y = 10f;
            player.transform.position = dest;
            player.GetComponent<SceneCleaner>().StartCleaning();
        }
        photonView.RPC("FinishedLoading", PhotonTargets.All);
	}

    [PunRPC]
    void InitializeLoader() {

        loadingSlider = loadingParent.GetComponentInChildren<Slider>();
        loadingParent.gameObject.SetActive(true);
        loadingSlider.value = 0;
        loadingSlider.maxValue = Mathf.Pow(worldSize, 2);
    }

    [PunRPC]
    void LoadedPart() {
        loadingSlider.value++;
    }

    [PunRPC]
    void FinishedLoading() {

        loadingParent.gameObject.SetActive(false);
    }

    [PunRPC]
    void SpawnNewObject(string obj, Vector2 pos, int size) {
        Transform parent = size == 0 ? smallAssetsT : size == 1 ? mediumAssetsT : size == 2 ? bigAssetsT : terrainT;
        Instantiate(Resources.Load(obj), new Vector3(pos.x, 0f, pos.y), Quaternion.identity, parent);
    }
}
