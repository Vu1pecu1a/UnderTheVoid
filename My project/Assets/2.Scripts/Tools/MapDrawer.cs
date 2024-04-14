using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csDelaunay;
using System.Linq;
using System.IO;
using UtilityHelper;

public static class MapDrawer
{
    public const int _stageOfDividCount = 12;//c#에선const도 static이랑 똑같은 방식으로 사용 가능 
    public static Color[] _SpecialResources;

    public static float[] GetRadianGradientMask(Vector2Int size, int maskRadius)
    {
        float[] colorData = new float[size.x * size.y];

        //맵의 중심
        Vector2Int center = size / 2;
        float radius = center.y;
        int index = 0;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                //맵의 중점으로부터 거리에 따라 색을 결정한다. 물론 설정해둔 마스킹 범위를 고려한다.
                float disFormcenter = Vector2Int.Distance(center, position) + (radius - maskRadius);
                float colorFactor = disFormcenter / radius;
                //거리가 멀 수록 색은 1에 가까워 지도록...
                //하지만 중심이 내륙이니 색을 반전.
                colorData[index++] = 1 - colorFactor;
            }
        }
        return colorData;
    }
    public static Sprite DrawSprite(Vector2Int size,Color[] colorDatas)
    {
        Texture2D texture = new Texture2D(size.x, size.y);
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorDatas);
        texture.Apply();

        Rect rect = new Rect(0, 0, size.x, size.y);
        Sprite sprite = Sprite.Create(texture, rect, Vector2.one * 0.5f);
        return sprite;
    }

    public static Sprite DrawVoronoiToSprite(Voronoi vo,Map map)
    {
        Rect rect = vo.PlotBounds;
        int width = Mathf.RoundToInt(rect.width);
        int height = Mathf.RoundToInt(rect.height);
        Color[] pixelColors = Enumerable.Repeat(Color.white, height * height).ToArray();//맵 사이즈 만큼 픽셀 채워넣기
        List<Vector2> SiteCoods = vo.SiteCoords();//점찍은거 
        List<SeedInfoDEC> _seeds = new List<SeedInfoDEC>();

        //시드 그리기
        foreach(Vector2 coord in SiteCoods)
        {
            int x = Mathf.RoundToInt(coord.x);//반올림
            int y = Mathf.RoundToInt(coord.y);

            int index = x * width + y;
            int resourceIndex = Random.Range(0, (int)ResourceType.Soil);
            SeedInfoDEC info = new SeedInfoDEC(new Vector2Int(x, y),map[resourceIndex]);
            pixelColors[index] = info._resourceColor ; // 랜덤으로 점 찍은걸 빨간색으로 표기하겠음. 시드 찍는것임 
            //이젠 색상에 따라 다른 점이 찍히게
        }

        Vector2Int size = new Vector2Int(width, height);

        //모서리 그리기
        //모든 점 정보를 얻어와서 모서리를 그리도록 한다.
        foreach (Site site in vo.Sites)
        {
            //해당 점의 이웃 점들을 모두 얻어와서 
            List<Site> neighbors = site.NeighborSites();//노드 구조인가...?
            foreach(Site neighbor in neighbors)
            {
                
                Edge edge = vo.FindEdgeFromAdjacentPolygons(site, neighbor);

                if (edge.ClippedVertices is null)
                    continue;

                //모서리를 이루는 2개의 정점을 얻어옴
                Vector2 corner1 = edge.ClippedVertices[LR.LEFT];
                Vector2 corner2 = edge.ClippedVertices[LR.RIGHT];


               // DrawEdgeLine(pixelColors, site.Coord, corner1, size,Color.green);
               // DrawEdgeLine(pixelColors, site.Coord, corner2, size, Color.green);

               // DrawEdgeLine(pixelColors, site.Coord, neighbor.Coord, size, Color.blue);
                //텍스쳐에 분할된 선을 그린다.
                DrawEdgeLine(pixelColors, corner1, corner2, size,Color.black);
            }
        }

        //색 채우기 
        for(int n=0;n<_seeds.Count; n++)
        {
            FillColor_DFS(_seeds[n]._centerPos.x, _seeds[n]._centerPos.y, size.x,pixelColors);
        }

        return DrawSprite(size, pixelColors);
    }

    static void DrawEdgeLine(Color[] pixelColors, Vector2 corner1, Vector2 corner2,Vector2Int size, Color color)
    {
        Vector2 targetPoint = corner1;
        float delta = 1 / (corner2 - corner1).magnitude;
        float lerpRatio = 0f;
        while ((int)targetPoint.x != (int)corner2.x || (int)targetPoint.y != (int)corner2.y)
        {
            //선형 보간을 통해 corner1과 corner2사이를 lerpRatio만큼 나누는 점을 얻어온다.
            targetPoint = Vector2.Lerp(corner1, corner2, lerpRatio);
            lerpRatio += delta;
            //텍스쳐의 좌표 영역은 (0~size.x -1)이지만 생성한 보로노이 다이어 그램의 좌표 영억은 (0~(float)size.x)이다.
            int x = Mathf.Clamp((int)targetPoint.x, 0, size.x - 1);
            int y = Mathf.Clamp((int)targetPoint.y, 0, size.y - 1);

            int index = x * size.x + y;
            pixelColors[index] = color;//선 그리기
        }
    }
    /*
    static void DrawCircle(Color[] pixelColors, Vector2 Coord, Vector2Int size, Color color)
    {
        for (int i = 0; i < 360; i++)
        {
            float radian = (float)i / 360 * Mathf.PI * 2;
            float x = Mathf.Cos(radian) * radius;
            float y = Mathf.Sin(radian) * radius;
        }
    }
    float circumradius(Vector2Int A,Vector2Int B,Vector2Int C)
    {
        float ax = C.x - B.x, ay = C.y - B.y;
        float bx = A.x - C.x, by = A.y - C.y;
        float crossab = ax * by - ay * bx;

        if(crossab != 0)
        {
            float a = Mathf.Sqrt(Mathf.)
        }
    }*/
    public static void CreatImageToFile(string fileName,Sprite image)
    {
        Texture2D img = new Texture2D(image.texture.width, image.texture.height);
        for(int y= 0; y<img.height;y++)
        {
            for(int x =0; x<img.width;x++)
            {
                img.SetPixel(x, y, image.texture.GetPixel(x, y));
            }
        }

        img.Apply();
        byte[] by = img.EncodeToPNG();
        FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        BinaryWriter bw = new BinaryWriter(fs);

        bw.Write(by);

        bw.Close();
        fs.Close();

    }

    static void FillColor_DFS(int PosX,int PosY,int width,Color[] SourceColor)
    {
        int index = PosX * width + PosY;
        if (SourceColor[index] == Color.black)
            return;
        //재귀로 채운다
    }
}
