using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBoundaries : MonoBehaviour {
    
	void Update () {
        if (transform.position.x > TerrainManager.boundaries) {
            transform.position = new Vector3(-0.9f * TerrainManager.boundaries, transform.position.y, transform.position.z);
        } else if (transform.position.x < -TerrainManager.boundaries) {
            transform.position = new Vector3(0.9f * TerrainManager.boundaries, transform.position.y, transform.position.z);
        }
        if (transform.position.z > TerrainManager.boundaries) {
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.9f * TerrainManager.boundaries);
        }else if (transform.position.z < -TerrainManager.boundaries) {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.9f * TerrainManager.boundaries);
        }

    }
}
