using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public int countItems;
    public Transform itemParent;
    public int countRoadProps;
    public List<ItemCollect> flagsPool, clocksPool, fuelsPool, carbureatorPool, rearViewMirrorPool, rudderPool;
    public List<ItemCollect> items;
    [System.Serializable]
    public struct ChapterProps
    {
        public bool generateChapter;
        public GameObject[] wallPropsPrefabs;
        public Transform wallPropsParent;
        public Transform roadPropsParent;
        public GameObject[] roadItemsPrefabs;
        public List<GameObject> wallProps, roadProps;
        public Vector3 offsetWallProps;
        public int countWallsProps;
    }
    public ChapterProps[] chapterProps;
    public ItemCollect flagPrefab, clockPrefab, fuelPrefab, rudderPrefab, carburetorPrefab, rearViewMirrorPrefab;
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        InitializePooled();
    }
    public void InitializePooled()
    {
        for(int i = 0; i < chapterProps.Length; i++)
        {
            for(int j = 0; j < chapterProps[i].countWallsProps; j++)
            {
                int _random = Random.Range(0, chapterProps[i].wallPropsPrefabs.Length);
                GameObject _obj = Instantiate(chapterProps[i].wallPropsPrefabs[_random]);
                _obj.SetActive(true);
                _obj.transform.SetParent(chapterProps[i].wallPropsParent);
                chapterProps[i].wallProps.Add(_obj);
                _obj.SetActive(false);
            }

            for(int j = 0; j < countRoadProps; j++)
            {
                int _random = Random.Range(0, chapterProps[i].roadItemsPrefabs.Length);
                GameObject _obj = Instantiate(chapterProps[i].roadItemsPrefabs[_random]);
                _obj.gameObject.SetActive(true);
                _obj.transform.SetParent(chapterProps[i].roadPropsParent);
                chapterProps[i].roadProps.Add(_obj);
                _obj.gameObject.SetActive(false);
            }
        }
        InstanceObject(fuelsPool,fuelPrefab);
        InstanceObject(flagsPool, flagPrefab);
        InstanceObject(clocksPool, clockPrefab);
        InstanceObject(rudderPool, rudderPrefab);
        InstanceObject(carbureatorPool, carburetorPrefab);
        InstanceObject(rearViewMirrorPool, rearViewMirrorPrefab);
    }
    private void InstanceObject(List<ItemCollect> _itemCollects, ItemCollect _prefab)
    {
        for (int j = 0; j < countItems; j++)
        {
            ItemCollect _obj = Instantiate(_prefab);
            _obj.gameObject.SetActive(true);
            _obj.transform.SetParent(itemParent);
            _itemCollects.Add(_obj);
            items.Add(_obj);
            _obj.gameObject.SetActive(false);
        }
    }
}
