using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csDelaunay;

public class Map : MonoBehaviour
{
    [Header("���γ���(Voronoi) ���� �Ķ����")]
    [SerializeField] Vector2Int _size;
    [SerializeField] int _nodeAmount = 0;
    [SerializeField,Tooltip("���� �õ� Ƚ��")] int _lloydIterateCount = 0;
    [Header("���� Data View")]
    [SerializeField] SpriteRenderer _voronoiViewRenderer = null;

    private void Awake()
    {
        Voronoi vo = GeneratVoronoi(_size, _nodeAmount, _lloydIterateCount);//�õ� ����
        _voronoiViewRenderer.sprite = MapDrawer.DrawVoronoiToSprite(vo);//�� ����
    }

    Voronoi GeneratVoronoi(Vector2Int size,int nodeAmount,int lloydIterateCount) // �� ũ��,�õ��� ����
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
