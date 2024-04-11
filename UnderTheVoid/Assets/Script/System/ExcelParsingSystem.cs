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
        strings.Add("전사");
        strings.Add("궁수");
        strings.Add("성직자");

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
                //해당 시트의 행데이터(한줄씩)로 반복
               
                //시트 개수만큼 반복
                for (int i = 0; i < result.Tables.Count; i++)
                {
                    for (int j = 0; j < result.Tables[1].Columns.Count; j++)
                    {
                        ////해당행의 0,1,2 셀의 데이터 파싱
                        //strings.Add(result.Tables[1].Rows[1][j].ToString());
                    }

                }
            }
        }
    }
}
