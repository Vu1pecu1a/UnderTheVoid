using ExcelDataReader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExcelParsingSystem : MonoBehaviour
{
    

    public List<string> strings= new List<string>();
    public Sprite[] sprites = null;

    void Awake()
    {
        //SetData();
        strings.Add("����");
        strings.Add("�ü�");
        strings.Add("������");

        sprites =new Sprite[3];
        //sprites = Resources.LoadAll<Sprite>("CLASS");
        sprites[0] = Resources.LoadAll<Sprite>("CLASS")[79];
        sprites[1] = Resources.LoadAll<Sprite>("CLASS")[46];
        sprites[2] = Resources.LoadAll<Sprite>("CLASS")[55];

       // Debug.Log(sprites.Length);
    }

    void SetData()
    {
        //string filePath = @"C:\test.xlsx";
        string filePath = Application.dataPath + "/Resource/Data.xlsx";
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                //�ش� ��Ʈ�� �൥����(���پ�)�� �ݺ�
               
                //��Ʈ ������ŭ �ݺ�
                for (int i = 0; i < result.Tables.Count; i++)
                {
                    for (int j = 0; j < result.Tables[1].Columns.Count; j++)
                    {
                        ////�ش����� 0,1,2 ���� ������ �Ľ�
                        //strings.Add(result.Tables[1].Rows[1][j].ToString());
                    }

                }
            }
        }
    }
}
