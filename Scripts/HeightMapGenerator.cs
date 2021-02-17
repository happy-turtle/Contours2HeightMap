using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ContourGenerator))]
public class HeightMapGenerator : MonoBehaviour
{
    public Material[] materials;
    public ComputeShader heightMapShader;
    public enum TextureSize { x512 = 512, x1024 = 1024, x2048 = 2048, x4096 = 4096 }
    public TextureSize textureSize = TextureSize.x1024;
    public float curviness = 10f;

    private RenderTexture heightMap;
    private ComputeBuffer polygonBuffer;
    private ComputeBuffer polygonIndicesBuffer;
    private int kernelHandle;
    private uint threadSizeX, threadSizeY, threadSizeZ;
    private int polygonBufferID;
    private int polygonIndicesBufferID;
    private int curvinessID;

    private readonly List<Vector2> flattened = new List<Vector2>();
    private readonly List<uint> polygonIndices = new List<uint>();

    private const int vector2Stride = sizeof(float) * 2;

    private void Awake()
    {
        heightMap = new RenderTexture((int)textureSize, (int)textureSize, 0, RenderTextureFormat.RFloat)
        {
            enableRandomWrite = true,
            wrapMode = TextureWrapMode.Repeat
        };
        heightMap.Create();

        kernelHandle = heightMapShader.FindKernel("CSMain");
        heightMapShader.GetKernelThreadGroupSizes(kernelHandle, out threadSizeX, out threadSizeY, out threadSizeZ);
        heightMapShader.SetTexture(kernelHandle, "Result", heightMap);

        polygonBufferID = Shader.PropertyToID("polygonOutline");
        polygonIndicesBufferID = Shader.PropertyToID("polygonIndices");
        curvinessID = Shader.PropertyToID("curviness");

        foreach (Material mat in materials)
        {
            //prevent height map from not being loaded in unity hdrp tesselation shader
            mat.EnableKeyword("_HEIGHTMAP");
            mat.SetTexture("_HeightMap", heightMap);
        }
    }

    public void GenerateHeightMap(List<List<Vector2>> polygons)
    {
        if (polygons == null || polygons.Count == 0)
            return;

        //flatten the list
        flattened.Clear();
        //where are the polygons in the flattened list
        polygonIndices.Clear();
        foreach(List<Vector2> polygon in polygons)
        {
            flattened.AddRange(polygon);
            polygonIndices.Add((uint)polygon.Count);
        }

        if(flattened != null && flattened.Count > 0)
        {
            polygonBuffer = new ComputeBuffer(flattened.Count, vector2Stride);
            polygonBuffer.SetData(flattened);
            heightMapShader.SetBuffer(kernelHandle, polygonBufferID, polygonBuffer);

            polygonIndicesBuffer = new ComputeBuffer(polygonIndices.Count, sizeof(int));
            polygonIndicesBuffer.SetData(polygonIndices);
            heightMapShader.SetBuffer(kernelHandle, polygonIndicesBufferID, polygonIndicesBuffer);

            heightMapShader.SetFloat(curvinessID, curviness);

            heightMapShader.Dispatch(kernelHandle, heightMap.width / (int)threadSizeX, heightMap.height / (int)threadSizeY, (int)threadSizeZ);

            polygonBuffer.Release();
            polygonIndicesBuffer.Release();
        }
    }
}
