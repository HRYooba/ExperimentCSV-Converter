using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVConverter : MonoBehaviour
{
    public TextAsset csvFile;
    private List<string[]> csvDatas = new List<string[]>();
    private string fileName;
    private bool canAddListL;
    private bool canAddListR;

    void Start()
    {
        fileName = System.DateTime.Now.ToString("yyyy-MMdd-HHmmss") + ".csv";

        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
        }

        float lMax = 0;
        float rMax = 0;
        List<float> lMaxList = new List<float>();
        List<float> rMaxList = new List<float>();

        for (int i = 0; i < csvDatas.Count; i++)
        {
            if (csvDatas[i][0] == "blendRateL")
            {
                if (lMaxList.Count != 0)
                {
                    lMaxList.Add(lMax);
                    rMaxList.Add(rMax);
                    float sumL = 0;
                    float sumR = 0;
                    for (int j = 0; j < lMaxList.Count; j++)
                    {
                        logSave(fileName, ",Left," + lMaxList[j] + ",Right," + rMaxList[j]);
                        sumL += lMaxList[j];
                        sumR += rMaxList[j];
                    }
                    logSave(fileName, ",LeftAverage," + sumL / lMaxList.Count + ",RightAverage," + sumR / rMaxList.Count);
                }

                lMax = 0;
                rMax = 0;
                canAddListL = false;
                canAddListR = false;
                lMaxList.Clear();
                rMaxList.Clear();
                logSave(fileName, "blendRateL," + csvDatas[i][1] + ",blendRateR," + csvDatas[i][3]);
            }
            else if (csvDatas[i][0] == "")
            {
                // Left
                if (float.Parse(csvDatas[i][3]) > 0.8f)
                {
                    if (float.Parse(csvDatas[i][3]) > lMax)
                    {
                        lMax = float.Parse(csvDatas[i][3]);
                    }
                    canAddListL = true;
                }
                else
                {
                    if (canAddListL)
                    {
                        lMaxList.Add(lMax);
                        canAddListL = false;
                        lMax = 0;
                    }
                }

                // Right
                if (float.Parse(csvDatas[i][7]) > 0.8f)
                {
                    if (float.Parse(csvDatas[i][7]) > rMax)
                    {
                        rMax = float.Parse(csvDatas[i][7]);
                    }
                    canAddListR = true;
                }
                else
                {
                    if (canAddListR)
                    {
                        rMaxList.Add(rMax);
                        canAddListR = false;
                        rMax = 0;
                    }
                }
            }

        }

        lMaxList.Add(lMax);
        rMaxList.Add(rMax);
        float sumLl = 0;
        float sumRl = 0;
        for (int j = 0; j < lMaxList.Count; j++)
        {
            logSave(fileName, ",Left," + lMaxList[j] + ",Right," + rMaxList[j]);
            sumLl += lMaxList[j];
            sumRl += rMaxList[j];
        }
        logSave(fileName, ",LeftAverage," + sumLl / lMaxList.Count + ",RightAverage," + sumRl / rMaxList.Count);
    }


    public void logSave(string file, string txt)
    {
        StreamWriter sw;
        FileInfo fi;
        fi = new FileInfo(Application.dataPath + "/" + file);
        sw = fi.AppendText();
        sw.WriteLine(txt);
        sw.Flush();
        sw.Close();
    }
}
