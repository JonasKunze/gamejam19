using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePhysics : MonoBehaviour
{
    private double twoSquareHalf;
    private double springConstant;
    private double friction;

    private double[] nextAmplitudes;
    private double[] currentAmplitudes;
    private double[] velocities;

    private uint xSize;
    private uint ySize;

    private Mesh mesh;
    private MeshFilter meshFilter;

    private void Start()
    {
        twoSquareHalf = Mathf.Sqrt(2.0f) / 2.0f;
        xSize = 0;
        ySize = 0;

        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
    }

    public void Init(uint xSize, uint ySize, double springConstant, double friction)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        this.springConstant = springConstant;
        this.friction = friction;
        var nextAmplitudes = new double[xSize * ySize];
        var currentAmplitudes = new double[xSize * ySize];
        var velocities = new double[xSize * ySize];
        for (int i = 0; i < xSize * ySize; ++i)
        {
            currentAmplitudes[i] = 0.0;
            nextAmplitudes[i] = 0.0;
            velocities[i] = 0.0;
        }
    }

    private double getAmplitude(uint x, uint y)
    {
        return nextAmplitudes[x * ySize + y];
    }

    private void setAmplitude(uint x, uint y, double value)
    {
        currentAmplitudes[x * ySize + y] = value;
    }

    private void update(double deltaT)
    {
        for (int x = 1; x < xSize - 1; ++x)
        {
            for (int y = 1; y < ySize - 1; ++y)
            {
                double force = 0;
                for (int a = -1; a < 2; ++a)
                {
                    for (int b = -1; b < 2; ++b)
                    {
                        double difference = currentAmplitudes[(x + a) * ySize + y + b] -
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

                double velocity = velocities[x * ySize + y];
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
    }

/*private void setNodes(Vector<Variant> voxels, int index) {
	for (int x = 1; x < xSize - 1; ++x) {
		for (int y = 1; y < ySize -1; ++y) {
			Spatial* node = (Spatial*)((Node*) voxels[x * ySize + y]);
			Vector3 translation = node->get_translation();
			translation[index] = currentAmplitudes[x * ySize + y];
			node->set_translation(translation);
		}
	}
}*/

    private void setMesh() {
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
}