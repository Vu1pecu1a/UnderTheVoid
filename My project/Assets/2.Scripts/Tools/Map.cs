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
    [Header("생성 Data View")]
    [SerializeField] SpriteRenderer _voronoiViewRenderer = null;

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
}
