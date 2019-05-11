using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = System.Random;

public class WaveMesh : MonoBehaviour
{
    [SerializeField] private uint xSize = 20;
    [SerializeField] private uint ySize = 20;

    [SerializeField] private float scaleX = 0.5f;
    [SerializeField] private float scaleY = 0.5f;

    private float twoSquareHalf;
    [SerializeField] private float springConstant;
    [SerializeField] private float friction;

    private float[] nextAmplitudes;
    private float[] currentAmplitudes;
    private float[] velocities;

    [SerializeField] private float stomplitude = 10;
    [SerializeField] private float frequency;

    [SerializeField] private int numberOfWaves;

    private Mesh mesh;
    private MeshFilter meshFilter;

    [Serializable]
    public struct SineSourceInfo
    {
        public Vector2Int position;
        public float amplitude;
        public float frequency;
        [HideInInspector] public float startTime;
        [HideInInspector] public float endTime;
        [Range(0, 600)] public float duration;
        [HideInInspector] public float currentTime;
        public bool isInfinite;

        public void Init(uint sizeX, uint sizeY)
        {
            //position = GetCorrectPosition(position, sizeX, sizeY);
            startTime = 0;
            currentTime = startTime;
            endTime = startTime + duration;
        }
    }

    [SerializeField] private List<SineSourceInfo> sineWaves;

    // Start is called before the first frame update
    void Start()
    {
        CreateMesh();

        foreach (var sineWave in sineWaves)
        {
            sineWave.Init(xSize, ySize);
        }

        /*for (int i = 0; i < numberOfWaves; i++)
        {
            int randX = Random.Range(-(int) ((xSize - 5) / 2), (int) ((xSize - 5) / 2));
            int randY = Random.Range(-(int) ((ySize - 5) / 2), (int) ((ySize - 5) / 2));
            float randAmplitude = Random.Range(0.1f, 0.5f);
            //simpleStomp(new Vector2(randX, randY), randAmplitude);
            StartSineSource(new Vector2(randX, randY), stomplitude, frequency, 60);
        }*/
    }

    public void CreateMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = new Mesh();
        mesh = meshFilter.sharedMesh;
        mesh.Clear();

        nextAmplitudes = new float[xSize * ySize];
        currentAmplitudes = new float[xSize * ySize];
        velocities = new float[xSize * ySize];
        for (int i = 0; i < xSize * ySize; ++i)
        {
            currentAmplitudes[i] = 0.0f;
            nextAmplitudes[i] = 0.0f;
            velocities[i] = 0.0f;
        }

        initMesh();
    }

    private void initMesh()
    {
        var vertices = new Vector3[xSize * ySize];
        var triangles = new int[6 * (xSize - 1) * (ySize - 1)];
        var uvCoords = new Vector2[xSize * ySize];
        for (uint x = 0; x < xSize; x++)
        {
            for (uint y = 0; y < ySize; y++)
            {
                vertices[x * ySize + y] = new Vector3((x - xSize / 2.0f) * scaleX, currentAmplitudes[x * ySize + y],
                    (y - ySize / 2.0f) * scaleY);
                uvCoords[x * ySize + y] = new Vector2(x / (float) xSize, y / (float) ySize);
            }
        }

        int triangleIndex = 0;
        for (uint x = 0; x < xSize - 1; x++)
        {
            for (uint y = 0; y < ySize - 1; y++)
            {
                uint triX = x * ySize + y;
                uint triY = (x + 1) * ySize + (y + 1);
                uint triZ = (x + 1) * ySize + y;
                triangles[triangleIndex] = (int) triX;
                triangles[triangleIndex + 1] = (int) triY;
                triangles[triangleIndex + 2] = (int) triZ;

                triX = x * ySize + y;
                triY = x * ySize + (y + 1);
                triZ = (x + 1) * ySize + (y + 1);
                triangles[triangleIndex + 3] = (int) triX;
                triangles[triangleIndex + 4] = (int) triY;
                triangles[triangleIndex + 5] = (int) triZ;

                triangleIndex += 6;
            }
        }

        meshFilter.sharedMesh.vertices = vertices;
        meshFilter.sharedMesh.triangles = triangles;
        meshFilter.sharedMesh.uv = uvCoords;

        meshFilter.sharedMesh.RecalculateBounds();
        meshFilter.sharedMesh.RecalculateNormals();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateWaves(Time.fixedDeltaTime);
        UpdateSineSources();

// wall

        /*for stonePos in
        stones:
        waves.setAmplitude(stonePos.x, stonePos.y, 0)*/
    }

    private void UpdateSineSources()
    {
        var wavesToBeRemoved = new List<SineSourceInfo>();
        for (int i = 0; i < sineWaves.Count; i++)
        {
            var sineWave = sineWaves[i];
            sineWave.currentTime += Time.fixedDeltaTime;
            if (!sineWave.isInfinite && sineWave.currentTime > sineWave.endTime)
            {
                wavesToBeRemoved.Add(sineWave);
                continue;
            }

            Vector2Int indices = WorldPositionToMeshIndices(new Vector2(sineWave.position.x, sineWave.position.y));
            //uint indexX = (uint) sineWave.position.x;
            //uint indexZ = (uint) sineWave.position.y;
            float amplitude = sineWave.amplitude *
                              Mathf.Sin(sineWave.frequency * (sineWave.currentTime - sineWave.startTime));
            setAmplitude((uint) indices.x, (uint) indices.y, amplitude);
            sineWaves[i] = sineWave;
        }

        foreach (var waveToBeRemoved in wavesToBeRemoved)
        {
            sineWaves.Remove(waveToBeRemoved);
        }
    }

    private void setAmplitude(uint x, uint y, float value)
    {
        currentAmplitudes[x * ySize + y] = value;
    }

    private void updateWaves(float deltaT)
    {
        for (int x = 1; x < xSize - 1; ++x)
        {
            for (int y = 1; y < ySize - 1; ++y)
            {
                float force = 0;
                for (int a = -1; a < 2; ++a)
                {
                    for (int b = -1; b < 2; ++b)
                    {
                        float difference = currentAmplitudes[(x + a) * ySize + y + b] -
                                           currentAmplitudes[x * ySize + y];
                        if (Mathf.Abs(x) + Mathf.Abs(y) == 2)
                        {
                            force += twoSquareHalf * difference;
                        }
                        else
                        {
                            force += difference;
                        }
                    }
                }

                float velocity = velocities[x * ySize + y];
                force = force * springConstant - velocity * friction;
                velocity += deltaT * force;

                nextAmplitudes[x * ySize + y] = currentAmplitudes[x * ySize + y] + velocity * deltaT;
                velocities[x * ySize + y] = velocity;
            }
        }

        for (int i = 0; i < xSize * ySize; ++i)
        {
            currentAmplitudes[i] = nextAmplitudes[i];
        }

        updateMesh();
    }

    private void updateMesh()
    {
        var vertices = new Vector3[xSize * ySize];

        for (uint x = 0; x < xSize; x++)
        {
            for (uint y = 0; y < ySize; y++)
            {
                vertices[x * ySize + y] = new Vector3((x - xSize / 2.0f) * scaleX, currentAmplitudes[x * ySize + y],
                    (y - ySize / 2.0f) * scaleY);
            }
        }

        //meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();
    }

    

    private Vector2Int WorldPositionToMeshIndices(Vector2 worldPosition)
    {
        int x = (int) (worldPosition.x / scaleX + xSize / 2.0f);
        int y = (int) (worldPosition.y / scaleY + ySize / 2.0f);

        x = (int) Mathf.Clamp(x, 0, xSize - 1);
        y = (int) Mathf.Clamp(y, 0, ySize - 1);

        return new Vector2Int(x, y);
    }

    public void Splash(Vector3 worldPosition, float intensity)
    {
        intensity = Mathf.Clamp01(intensity);
        simpleStomp(worldPosition, intensity);
    }

    Vector3 simpleStomp(Vector2 position, float amplitudeFactor = 1)
    {
        Vector2 indices = WorldPositionToMeshIndices(position);
        float betterStomplitude = 10; 

            Debug.LogError(indices);
//        if ((indexX + 0 < 1) || (indexZ + 0 < 1) || (indexX + 0 > xSize - 2) || (indexZ + 0 > ySize - 2))
//            Debug.Assert(false);
        setAmplitude((uint)indices.x, (uint)indices.y, betterStomplitude * amplitudeFactor);


        //lastStompCenterIndices = new Vector2(indexX, indexZ);
        return new Vector3(-xSize / 2f + indices.x, position.y, -ySize / 2f + indices.y);
    }
    
    
    Vector3 stomp(Vector2 position, float amplitudeFactor = 1)
    {
        Vector2Int indices = WorldPositionToMeshIndices(position);

        for (int x = -4; x < 5; x++)
        {
            for (int y = -4; y < 5; y++)
            {
                if ((indices.x + x < 1) || (indices.y + y < 1) || (indices.x + x > xSize - 2) ||
                    (indices.y + y > ySize - 2))
                    continue;
                var r = Mathf.Sqrt(x * x + y * y);
                if (Mathf.Abs(r - 4) < 0.5)
                    setAmplitude((uint) (indices.x + x), (uint) (indices.y + y), stomplitude * amplitudeFactor);
            }
        }

        //lastStompCenterIndices = new Vector2(indexX, indexZ);
        return new Vector3(-xSize / 2 + indices.x, position.y, -ySize / 2 + indices.y);
    }

    /*private void StartSineSource(Vector2 position, float amplitude, float frequency, float timeSeconds, bool isInfinite)
    {
        sineWaves.Add(new SineSourceInfo
        {
            position = GetCorrectPosition(position, xSize, ySize), amplitude = amplitude, frequency = frequency,
            currentTime = Time.fixedTime, startTime = Time.fixedTime, endTime = Time.fixedTime + timeSeconds, isInfinite = isInfinite
        });
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var sineWave in sineWaves)
        {
            //Vector2 correctPos = WorldPositionToIndex(sineWave.position, xSize, ySize, new Vector2(scaleX, scaleY));
            //correctPos.x *= scaleX;
            //correctPos.y *= scaleY;
            float y = gameObject.transform.position.y;
            Vector3 worldPos = new Vector3(sineWave.position.x, y, sineWave.position.y);

            Gizmos.DrawSphere(worldPos, 1.0f);
        }
    }
}