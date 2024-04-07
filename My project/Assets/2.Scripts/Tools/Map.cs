using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csDelaunay;

public class Map : MonoBehaviour
{
    [Header("보로노이(Voronoi) 생성 파라미터")]
    [SerializeField] Vector2Int _size;
    [SerializeField] int _nodeAmount = 0;
    [SerializeField,Tooltip("보정 시도 횟수")] int _lloydIterateCount = 0;
    [Header("PerlinNoise 생성 파라미터")]
    [SerializeField, Range(0f, 0.4f)] float _noiseFrequency = 0;
    [SerializeField] int _noiseOctave = 0;
    [SerializeField] int _seed = 100;
    [SerializeField, Range(0f, 0.5f)] float _landNoiseThreshold = 0;
    [SerializeField] int _noiseMaskRadius = 0;
    [Header("생성 Data View")]
    [SerializeField] SpriteRenderer _voronoiViewRenderer = null;
    [SerializeField] SpriteRenderer _noiseMapRender = null;

    private void Awake()
    {
        Voronoi vo = GeneratVoronoi(_size, _nodeAmount, _lloydIterateCount);//시드 생성
        _voronoiViewRenderer.sprite = MapDrawer.DrawVoronoiToSprite(vo);//맵 생성
    }

    Voronoi GeneratVoronoi(Vector2Int size,int nodeAmount,int lloydIterateCount) // 맵 크기,시드의 갯수
    {
        List<Vector2> centroids = new List<Vector2>();
        for(int n = 0; n<nodeAmount; n++)
        {
            int rx = Random.Range(0, size.x);
            int ry = Random.Range(0, size.y);

            centroids.Add(new Vector2(rx, ry));
        }

        Rect rect = new Rect(0, 0, size.x, size.y);
        Voronoi vo = new Voronoi(centroids, rect,lloydIterateCount);

        return vo;
    }

    float[] CreatMapShape(Vector2Int size,float frequency,int octave)
    {
        int seed = (_seed == 0) ? Random.Range(1, int.MaxValue) : _seed;

        FastNoiseLite noise = new FastNoiseLite();
        //펄린 노이즈
        noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        // 기본적인 프렉탈 노이즈 타입으로 설정
        noise.SetFractalType(FastNoiseLite.FractalType.FBm);//Fractional Brownian motion
        noise.SetFrequency(frequency);
        noise.SetFractalOctaves(octave);
        noise.SetSeed(seed);

        // 마스크 컬러 생성 
        float[] mask = MapDrawer.GetRadianGradientMask(size, _noiseMaskRadius);
        //색은 0~1 범위이며, 0은 검은색,1은 흰색
        float[] colorDatas = new float[size.x * size.y];
        int index = 0;
        for(int y = 0; y<size.y; y++)
        {
            for(int x = 0; x<size.x; x++)
            {
                float noiseColorFactor = noise.GetNoise(x, y);
                //노이즈 범위가 -1~1이기 떄문에 이 노이즈를 0~1범위로 변환하는 연산.
                noiseColorFactor = (noiseColorFactor + 1) * 0.5f; // 음수값을 받지 않기 위해 이렇게 해준다.
                noiseColorFactor *= mask[index];
                float color = noiseColorFactor > _landNoiseThreshold ? noiseColorFactor : 0f;
                //colorDatas[index++] = noiseColorFactor;
                colorDatas[index++] = color;
            }
        }
        return colorDatas;
    }

    public void GenerateMap()
    {
        float[] noiseColor = CreatMapShape(_size, _noiseFrequency, _noiseOctave);
        Color[] colors = new Color[noiseColor.Length];
        for(int n =0; n<colors.Length;n++)
        {
            if(noiseColor[n]==0)
            {
                colors[n] = Color.blue;
            }
            else
            colors[n] = new Color(noiseColor[n], noiseColor[n], noiseColor[n], 1);
        }
        _noiseMapRender.sprite = MapDrawer.DrawSprite(_size, colors);
    }

    private void Update()
    {
        GenerateMap();
    }


}
