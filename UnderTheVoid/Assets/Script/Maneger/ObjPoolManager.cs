using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolingObj
    {
        public string name; // 오브젝트 이름?
        public GameObject _prefobj;// 게임오브젝트
        public int _amount; //최대치
        int _curAmount; //현재 갯수

        public int CurAmount
        {
            get { return _curAmount; }
            set { _curAmount = value; }
        }
    }

    public static ObjPoolManager i;

    public PoolingObj[] _poolobj; // 나중에 딕셔너리로 교체할것
    public List<GameObject>[] _poolobjList;

    public int _defPoolAmount = 10;//디폴트 갯수

    public bool _canPoolExpand = true; // 확장 플래그

    // Start is called before the first frame update

    private void Awake()
    {
        i = this;

        _poolobjList = new List<GameObject>[_poolobj.Length];

        for(int i = 0; i<_poolobj.Length; i++)
        {
            _poolobjList[i] = new List<GameObject>();

            if (_poolobj[i]._amount > 0)
                _poolobj[i].CurAmount = _poolobj[i]._amount;
            else
                _poolobj[i].CurAmount = _defPoolAmount;

            int idx = 0;
            for(int j = 0; j < _poolobj[i].CurAmount; j++)
            {
                GameObject newItem = Instantiate(_poolobj[i]._prefobj);

                string suffix = _poolobj[i].name + "_" + idx;

                AddTOPoolObjList(i, newItem, suffix);

                ++idx;
            }
        }
    }

    void AddTOPoolObjList(int idx,GameObject newItem,string suffix)
    {
        newItem.name = suffix;
        newItem.SetActive(false);
        newItem.transform.SetParent(transform);

        _poolobjList[idx].Add(newItem);
    }

    GameObject GetPoolItem(string Code)
    {
        for(int i = 0; i<_poolobjList.Length; i++)
        {
            if (_poolobj[i]._prefobj.name == Code)
            {
                int listIdx = 0;

                for(listIdx = 0; listIdx < _poolobjList[i].Count; listIdx++)
                {
                    if (_poolobjList[i][listIdx] == null)
                        return null;
                    if (_poolobjList[i][listIdx].activeInHierarchy == false)
                        return _poolobjList[i][listIdx];
                }
                if(_canPoolExpand)
                {
                    GameObject tmpobj = Instantiate(_poolobj[i]._prefobj);
                    
                    string sffix = Code + "_" + listIdx.ToString() + "(" + (listIdx - _poolobj[i].CurAmount+1).ToString() + ")";

                    AddTOPoolObjList(i, tmpobj, sffix);
                    return tmpobj;
                }
                break;
            }
        }

        return null;
    }

    void Start()
    {

    }
    public static void DestroyAPS(GameObject obj) { obj.SetActive(false); }
    // Update is called once per frame
    public GameObject InstantiateAPS(int idx, GameObject parent = null)
    {
        string pooledObjName = _poolobj[idx].name;

        GameObject tmp = InstantiateAPS(pooledObjName, Vector3.zero,
                                        _poolobj[idx]._prefobj.transform.rotation,
                                        _poolobj[idx]._prefobj.transform.localScale,
                                        parent);

        return tmp;

    }// public GameObject InstantiateAPS(int idx, GameObject parent = null)
    //-----------------------------
    public GameObject InstantiateAPS(int idx, Vector3 pos, Quaternion rot, Vector3 scale, GameObject parent = null)
    {
        string pooledObjName = _poolobj[idx].name;

        GameObject tmp = InstantiateAPS(pooledObjName, pos, rot, scale, parent);

        return tmp;

    }// public GameObject InstantiateAPS(int idx, Vector3 pos, Quaternion rot, Vector3 scale, GameObject parent = null)
    //-----------------------------
    public GameObject InstantiateAPS(string pooledObjName, GameObject parent = null)
    {
        GameObject tmpObj = GetPoolItem(pooledObjName);

        tmpObj.SetActive(true);

        return tmpObj;

    }
    public GameObject InstantiateAPSTr(string pooledObjName, Transform parent )
    {
        GameObject tmpObj = GetPoolItem(pooledObjName);

        tmpObj.SetActive(true);

        tmpObj.transform.SetParent(parent);  

        return tmpObj;

    }// public GameObject InstantiateAPS(string pooledObjName, GameObject parent = null)
    //-----------------------------
    public GameObject InstantiateAPS(string pooledObjName, Vector3 pos, Quaternion rot, Vector3 scale, GameObject parent = null)
    {
        GameObject tmpObj = GetPoolItem(pooledObjName);

        if (tmpObj != null)
        {
            if (parent != null)
                tmpObj.transform.parent = parent.transform;

            tmpObj.transform.position = pos;
            tmpObj.transform.rotation = rot;
            tmpObj.transform.localScale = scale;
            tmpObj.SetActive(true);

        }//	if(newObject != null)

        return tmpObj;

    }
}
public static class PoolingSystemExtensions
{
    //-----------------------------
    public static void DestroyAPS(this GameObject obj)
    { ObjPoolManager.DestroyAPS(obj); }
    //-----------------------------
    /*
    public static void PlaySoundRepeatedly(this GameObject soundObj, float volume = 1.0f)
    { ObjPoolManager.PlaySoundRepeatedly(soundObj, volume); }
    //-----------------------------
    public static void PlaySound(this GameObject soundObj, float volume = 1.0f)
    { ObjPoolManager.PlaySound(soundObj, volume); }
    //-----------------------------
    public static void StopSound(this GameObject soundObj)
    { ObjPoolManager.StopSound(soundObj); }
    //-----------------------------
    public static void PlayEffect(this GameObject effObj, int emitCount) { ObjPoolManager.PlayEffect(effObj, emitCount); }
    //-----------------------------
    public static void PlayEffect(this GameObject effObj) { ObjPoolManager.PlayEffect(effObj); }
    //-----------------------------
    */
}
