using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spreadsheet
{
    public class DataManager : MonoBehaviour
    {
        public TextAsset data;
        private AllData datas;

        public GameObject block;
        public Transform env;

        private void Awake()
        {
            datas= JsonUtility.FromJson<AllData>(data.text);

            foreach (var v in datas.stage)
            {
                print(v.name);
            }
        }

        private void Start()
        {
            int x = datas.stage[0].horizontal;
            int y = datas.stage[0].vertical;


            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    var obj = Instantiate(block, env);
                    obj.transform.position = new Vector3(i, 0, j);
                }
            }
        }
    }
}

[System.Serializable]
public class AllData
{
    public MapData[] stage;
}

[System.Serializable]
public class MapData
{
    public int stage;
    public string name;
    public int horizontal;
    public int vertical;
    public int x;
    public int y;
}