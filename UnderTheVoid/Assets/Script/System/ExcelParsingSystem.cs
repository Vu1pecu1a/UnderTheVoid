using ExcelDataReader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExcelParsingSystem : MonoBehaviour
{
    void Start()
    {
        //string filePath = @"C:\test.xlsx";
        string filePath = Application.dataPath + "/Resource/Data.xlsx";
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

                //시트 개수만큼 반복
                for (int i = 0; i < result.Tables.Count; i++)
                {
                    //해당 시트의 행데이터(한줄씩)로 반복
                    for (int j = 0; j < result.Tables[i].Rows.Count; j++)
                    {
                        //해당행의 0,1,2 셀의 데이터 파싱
                        string data1 = result.Tables[i].Rows[j][0].ToString();
                        string data2 = result.Tables[i].Rows[j][1].ToString();
                        string data3 = result.Tables[i].Rows[j][2].ToString();
                        Debug.Log(data1+","+data2+ "," + data3);   
                    }
                }
            }
        }
    }
}
