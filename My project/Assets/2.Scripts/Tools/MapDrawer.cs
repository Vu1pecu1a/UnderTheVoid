using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csDelaunay;
using System.Linq;
using System.IO;
using UtilityHelper;

public static class MapDrawer
{
    public const int _stageOfDividCount = 12;//c#����const�� static�̶� �Ȱ��� ������� ��� ���� 
    public static Color[] _SpecialResources;

    public static float[] GetRadianGradientMask(Vector2Int size, int maskRadius)
    {
        float[] colorData = new float[size.x * size.y];

        //���� �߽�
        Vector2Int center = size / 2;
        float radius = center.y;
        int index = 0;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                //���� �������κ��� �Ÿ��� ���� ���� �����Ѵ�. ���� �����ص� ����ŷ ������ ����Ѵ�.
                float disFormcenter = Vector2Int.Distance(center, position) + (radius - maskRadius);
                float colorFactor = disFormcenter / radius;
                //�Ÿ��� �� ���� ���� 1�� ����� ������...
                //������ �߽��� �����̴� ���� ����.
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
        Color[] pixelColors = Enumerable.Repeat(Color.white, height * height).ToArray();//�� ������ ��ŭ �ȼ� ä���ֱ�
        List<Vector2> SiteCoods = vo.SiteCoords();//�������� 
        List<SeedInfoDEC> _seeds = new List<SeedInfoDEC>();

        //�õ� �׸���
        foreach(Vector2 coord in SiteCoods)
        {
            int x = Mathf.RoundToInt(coord.x);//�ݿø�
            int y = Mathf.RoundToInt(coord.y);

            int index = x * width + y;
            int resourceIndex = Random.Range(0, (int)ResourceType.Soil);
            SeedInfoDEC info = new SeedInfoDEC(new Vector2Int(x, y),map[resourceIndex]);
            pixelColors[index] = info._resourceColor ; // �������� �� ������ ���������� ǥ���ϰ���. �õ� ��°��� 
            //���� ���� ���� �ٸ� ���� ������
        }

        Vector2Int size = new Vector2Int(width, height);

        //�𼭸� �׸���
        //��� �� ������ ���ͼ� �𼭸��� �׸����� �Ѵ�.
        foreach (Site site in vo.Sites)
        {
            //�ش� ���� �̿� ������ ��� ���ͼ� 
            List<Site> neighbors = site.NeighborSites();//��� �����ΰ�...?
            foreach(Site neighbor in neighbors)
            {
                
                Edge edge = vo.FindEdgeFromAdjacentPolygons(site, neighbor);

                if (edge.ClippedVertices is null)
                    continue;

                //�𼭸��� �̷�� 2���� ������ ����
                Vector2 corner1 = edge.ClippedVertices[LR.LEFT];
                Vector2 corner2 = edge.ClippedVertices[LR.RIGHT];


               // DrawEdgeLine(pixelColors, site.Coord, corner1, size,Color.green);
               // DrawEdgeLine(pixelColors, site.Coord, corner2, size, Color.green);

               // DrawEdgeLine(pixelColors, site.Coord, neighbor.Coord, size, Color.blue);
                //�ؽ��Ŀ� ���ҵ� ���� �׸���.
                DrawEdgeLine(pixelColors, corner1, corner2, size,Color.black);
            }
        }

        //�� ä��� 
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
            //���� ������ ���� corner1�� corner2���̸� lerpRatio��ŭ ������ ���� ���´�.
            targetPoint = Vector2.Lerp(corner1, corner2, lerpRatio);
            lerpRatio += delta;
            //�ؽ����� ��ǥ ������ (0~size.x -1)������ ������ ���γ��� ���̾� �׷��� ��ǥ ������ (0~(float)size.x)�̴�.
            int x = Mathf.Clamp((int)targetPoint.x, 0, size.x - 1);
            int y = Mathf.Clamp((int)targetPoint.y, 0, size.y - 1);

            int index = x * size.x + y;
            pixelColors[index] = color;//�� �׸���
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
        //��ͷ� ä���
    }
}
