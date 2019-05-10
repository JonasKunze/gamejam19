using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMesh : MonoBehaviour
{
    private uint xSize = 20;
    private uint ySize = 20;

    private float scaleX = 0.5f;
    private float scaleY = 0.5f;

    private Mesh mesh;
    private MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        initMesh();
    }

    // Update is called once per frame
    void Update()
    {
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
                vertices[x * ySize + y] = new Vector3((x - xSize / 2.0f) * scaleX, 0, (y - ySize/2.0f) * scaleY);
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
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.triangles = triangles;
        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();
    }

/*    var material = FixedMaterial.new()
	material.set_parameter(material.PARAM_DIFFUSE, Color(1,0,0,1))
	var surfTool = SurfaceTool.new()
	surfTool.set_material(material)
	surfTool.begin(VS.PRIMITIVE_TRIANGLES)
	for z in range(sizeZ):
		for x in range(sizeX):
			var xCentered = x - sizeX / 2
			var zCentered = z - sizeZ / 2
			surfTool.add_normal(Vector3(0,1,0))
			surfTool.add_vertex(Vector3(xCentered + 0, 0, zCentered + 0))
			surfTool.add_normal(Vector3(0,1,0))
			surfTool.add_vertex(Vector3(xCentered + 1, 0, zCentered + 0))
			surfTool.add_normal(Vector3(0,1,0))
			surfTool.add_vertex(Vector3(xCentered + 0, 0, zCentered + 1))
			surfTool.add_normal(Vector3(0,1,0))
			surfTool.add_vertex(Vector3(xCentered + 1, 0, zCentered + 1))
			surfTool.add_normal(Vector3(0,1,0))
			surfTool.add_vertex(Vector3(xCentered + 0, 0, zCentered + 1))
			surfTool.add_normal(Vector3(0,1,0))
			surfTool.add_vertex(Vector3(xCentered + 1, 0, zCentered + 0))
	surfTool.commit(mesh)
	meshinstance.set_mesh(mesh)
	meshinstance.create_trimesh_collision()
	add_child(meshinstance)

/*func initNodes():
	for x in range(0, sizeX):
		for z in range(0, sizeZ):
			var pos = Vector3(x - sizeX/2.0, 0, z - sizeZ/2.0)
			var box = mapscene.instance()
			add_child(box)
			box.set_translation(pos)
			boxes.append(box)
	waves.setNodes(boxes, 1)

func _ready():
	initNodes()
	waves.init(sizeX, sizeZ, springConstant, friction)
	self.set_process(true)

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