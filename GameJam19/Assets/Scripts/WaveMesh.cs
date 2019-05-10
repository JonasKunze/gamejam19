using UnityEngine;

public class WaveMesh : MonoBehaviour
{
    [SerializeField] private uint xSize = 20;
    [SerializeField] private uint ySize = 20;

    private float scaleX = 0.5f;
    private float scaleY = 0.5f;

    private float twoSquareHalf;
    [SerializeField] private float springConstant;
    [SerializeField] private float friction;

    private float[] nextAmplitudes;
    private float[] currentAmplitudes;
    private float[] velocities;

    [SerializeField] private float stomplitude = 10;

    [SerializeField] private int numberOfWaves;

    private Mesh mesh;
    private MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        CreateMesh();
        for (int i = 0; i < numberOfWaves; i++)
        {
            int randX = Random.Range(-(int) ((xSize - 5) / 2), (int) ((xSize - 5) / 2));
            int randY = Random.Range(-(int) ((ySize - 5) / 2), (int) ((ySize - 5) / 2));
            float randAmplitude = Random.Range(0.1f, 0.5f);
            stomp(new Vector2(randX, randY), randAmplitude);
        }
    }

    public void CreateMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
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

    // Update is called once per frame
    void Update()
    {
        updateWaves(Time.deltaTime);

// wall

        /*for stonePos in
        stones:
        waves.setAmplitude(stonePos.x, stonePos.y, 0)*/
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

    private void initMesh()
    {
        Mesh mesh = new Mesh();
        var vertices = new Vector3[xSize * ySize];
        var triangles = new int[6 * (xSize - 1) * (ySize - 1)];
        for (uint x = 0; x < xSize; x++)
        {
            for (uint y = 0; y < ySize; y++)
            {
                vertices[x * ySize + y] = new Vector3((x - xSize / 2.0f) * scaleX, currentAmplitudes[x * ySize + y],
                    (y - ySize / 2.0f) * scaleY);
            }
        }

        int ctr = 0;
        for (uint x = 0; x < xSize - 1; x++)
        {
            for (uint y = 0; y < ySize - 1; y++)
            {
                uint triX = x * ySize + y;
                uint triY = (x + 1) * ySize + (y + 1);
                uint triZ = (x + 1) * ySize + y;
                triangles[ctr] = (int) triX;
                triangles[ctr + 1] = (int) triY;
                triangles[ctr + 2] = (int) triZ;

                triX = x * ySize + y;
                triY = x * ySize + (y + 1);
                triZ = (x + 1) * ySize + (y + 1);
                triangles[ctr + 3] = (int) triX;
                triangles[ctr + 4] = (int) triY;
                triangles[ctr + 5] = (int) triZ;

                ctr += 6;
            }
        }

        mesh.Clear();
        meshFilter.sharedMesh.vertices = vertices;
        meshFilter.sharedMesh.triangles = triangles;
        meshFilter.sharedMesh.RecalculateBounds();
        meshFilter.sharedMesh.RecalculateNormals();
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

    Vector3 stomp(Vector2 position, float amplitudeFactor = 1)
    {
        uint indexX = (uint) (position.x + xSize / 2.0f);
        uint indexZ = (uint) (position.y + ySize / 2.0f);

        for (int x = -4; x < 5; x++)
        {
            for (int y = -4; y < 5; y++)
            {
                if ((indexX + x < 1) || (indexZ + y < 1) || (indexX + x > xSize - 2) || (indexZ + y > ySize - 2))
                    continue;
                var r = Mathf.Sqrt(x * x + y * y);
                if (Mathf.Abs(r - 4) < 0.5)
                    setAmplitude((uint) (indexX + x), (uint) (indexZ + y), stomplitude * amplitudeFactor);
            }
        }

        //lastStompCenterIndices = new Vector2(indexX, indexZ);
        return new Vector3(-xSize / 2 + indexX, position.y, -ySize / 2 + indexZ);
    }

/*
var frame = 0
func _process(deltaT):
	frame = frame + 1
	if frame %50 == 0:
		print(1/deltaT)
	waves.update(deltaT)
	
	# I will build a wall to make games great again
	#for x in range(0, sizeX/2):
	#	waves.setAmplitude(x, 10, 0)
	#	waves.setAmplitude(x, 11, 0)
	#	waves.setAmplitude(x, 13, 0)
	
	for stonePos in stones:
		waves.setAmplitude(stonePos.x, stonePos.y, 0)
	
	if OS.get_unix_time()-lastStompTime < 5:
		for i in range(-1, 2):
			waves.setAmplitude(lastStompCenterIndices.x+i, lastStompCenterIndices.y, 0)
			waves.setAmplitude(lastStompCenterIndices.x, lastStompCenterIndices.y+i, 0)
	
	waves.setNodes(boxes, 1)

func stomp(position, amplitudeFactor = 1):
	get_node("/root/Spatial/SamplePlayer").play("smash")
	var indexX = int(position.x)+sizeX/2
	var indexZ = int(position.z)+sizeZ/2
	
	for x in range(-4, 5):
		for z in range(-4, 5):
			if ((indexX + x < 1) or (indexZ + z < 1) or (indexX + x > sizeX-2) or (indexZ + z > sizeZ-2)):
				continue
			var r = sqrt(x*x+z*z)
			if abs(r-4) < 0.5:
				waves.setAmplitude(indexX+x, indexZ+z, stomplitude*amplitudeFactor)
	lastStompTime = OS.get_unix_time()
	lastStompCenterIndices = Vector2(indexX, indexZ)
	return Vector3(-sizeX/2+indexX, position.y, -sizeZ/2+indexZ)

func getHeightAt(position):
	var indexX = int(position.x)+sizeX/2
	var indexZ = int(position.z)+sizeZ/2
	
	return waves.getAmplitude(indexX, indexZ)

func putStone(position):
	var indexX = int(position.x)+sizeX/2
	var indexZ = int(position.z)+sizeZ/2
	
	stones.append(Vector2(indexX, indexZ))*/
}