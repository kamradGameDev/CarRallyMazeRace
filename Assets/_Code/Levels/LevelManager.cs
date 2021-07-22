using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum StatusCurrentLevel
{
    menu, start, process, pause, win, nullTime, collsionEmemy, nullFuel
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public LevelComply levelComply;
    public Tile wallBlockTile;
    public int currentLevel;
    public GameObject activeLevel;
    public LevelHelper levelHelper;
    public StatusCurrentLevel statusCurrentLevel;
    public int currentPlayer;

    public Transform player;
    public EnemyMotion[] enemyMotions;
    public List<Transform> wayPoints;
    public Text LivesText, TimeText, FuelProcentText, FlagsCountText, PointsCountText;
    public Image FuelSlider;
    public int maxCountFlagInLevel;
    public int countClockInScene, countFuelInScene, countRudderInScene, countRearViewMirror, countCarburetorInScene;
    public int countCollectFlags;
    public int countPoints;
    [System.Serializable]
    public struct RoadTiles
    {
        public List<Vector3> vector;
        public List<string> name;
    }
    public RoadTiles roadTiles;
    public Dictionary<Vector3, WorldTile> tiles;
    public List<Vector3> wallTiles;
    public List<ItemCollect> itemInScene;
    public List<GameObject> wallPropsInScene, roadPropsInScene;
    public List<Transform> targetInstanceBonus;
    static public Tilemap WallTilemap;
    public float timeCount, fuelCount, maxFuelCount;
    public Tilemap roadMap;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    private void CheckMinAndMaxPosRoadForCameraController()
    {
        CameraController.instance.MinX = roadTiles.vector.Min(maxCount => maxCount.x) + 7;
        CameraController.instance.MaxX = roadTiles.vector.Max(maxCount => maxCount.x) - 7;
        CameraController.instance.MinY = roadTiles.vector.Min(maxCount => maxCount.y) + 1;
        CameraController.instance.MaxY = roadTiles.vector.Max(maxCount => maxCount.y) - 1;
        for (int i = 0; i < levelHelper.roadBlockTiles.Length; i++)
        {
            if(levelHelper.roadBlockTiles[i].x == CameraController.instance.MinX)
            {
                CameraController.instance.MinX += 1;
            }
            else if(levelHelper.roadBlockTiles[i].x == CameraController.instance.MaxX)
            {
                CameraController.instance.MaxX -= 1;
            }
            else if(levelHelper.roadBlockTiles[i].y == CameraController.instance.MinY)
            {
                CameraController.instance.MinY += 2;
            }
            else if(levelHelper.roadBlockTiles[i].y == CameraController.instance.MaxY)
            {
                CameraController.instance.MaxY -= 2;
            }
        }

    }
    private void CheckCountLives(char _char, int _newCount)
    {
        if (!SavedData.instance.unlimitedLives)
        {
            LivesText.text = _newCount.ToString();
            LivesText.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            LivesText.text = "";
            LivesText.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void CurrentPointsLevel(int _newPoints) => PointsCountText.text = countPoints.ToString();
    private void CollectFrags(int _newCollectFlags) => FlagsCountText.text = countCollectFlags + "/" + maxCountFlagInLevel.ToString();
    public void EndLevel()
    {
        EventManager.instance.CheckCountLivesAction -= CheckCountLives;
        EventManager.instance.PointsCurrentLevelAction -= CurrentPointsLevel;
        EventManager.instance.CollectFlagsAction -= CollectFrags;
    }
    public void StartLevel()
    {
        EventManager.instance.StatusLevel(StatusCurrentLevel.start);
        EventManager.instance.PointsCurrentLevelAction += CurrentPointsLevel;
        EventManager.instance.CollectFlagsAction += CollectFrags;
        EventManager.instance.CheckCountLivesAction += CheckCountLives;

        for (int i = 0; i < levelHelper.enemyMotions.Length; i++)
        {
            levelHelper.enemyMotions[i].transform.position = levelHelper.enemyMotions[i].GetComponent<EnemyMotion>().startPos;
            levelHelper.enemyMotions[i].GetComponent<EnemyMotion>().indexTarget = 0;
            levelHelper.enemyMotions[i].GetComponent<EnemyMotion>().transform.position = levelHelper.enemyMotions[i].GetComponent<EnemyMotion>().targets[0].position;

            int _randColorEnemyCar = UnityEngine.Random.Range(0, levelHelper.enemyMotions[i].transform.childCount);

            for (int j = 0; j < levelHelper.enemyMotions[i].transform.childCount; j++)
            {
                //Debug.Log("randomColorEnemyCar: " + _randColorEnemyCar);
                if (j == _randColorEnemyCar)
                {
                    levelHelper.enemyMotions[i].transform.GetChild(j).gameObject.SetActive(true);
                }
                else
                {
                    levelHelper.enemyMotions[i].transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
        WallTilemap = activeLevel.transform.GetChild(1).GetChild(1).GetComponent<Tilemap>();
        roadMap = activeLevel.transform.GetChild(1).GetChild(0).GetComponent<Tilemap>();
        ClearItems();
        ClearWallPropsInScene();
        GetWorldTiles(roadMap, 0);
        GetWorldTiles(WallTilemap, 1);
        //GetWorldTiles(wallMap, 2);
        timeCount = 66;
        fuelCount = DataManager.instance.cars[DataManager.instance.currentCar].fuelCount;
        maxFuelCount = DataManager.instance.cars[DataManager.instance.currentCar].maxCountFuel;
        countPoints = 0;
        countCollectFlags = 0;
        EventManager.instance.NewPointsLevel(0);
        EventManager.instance.CheckCountLives(' ', SavedData.instance.livesCount);
        EventManager.instance.CollectFlags(0);
        JoystickHelper.instance.movementStatus = true;
        ClosedParking();
        levelComply.StartLevel();
    }
    private void GetWorldTiles(Tilemap _tilemap, int _statusGenerate)
    {
        tiles = new Dictionary<Vector3, WorldTile>();
        tiles.Clear();
        foreach (Vector3Int _pos in _tilemap.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(_pos.x, _pos.y, _pos.z);

            if (!_tilemap.HasTile(localPlace)) continue;
            var tile = new WorldTile
            {
                LocalPlace = localPlace,
                WorldLocation = _tilemap.CellToWorld(localPlace),
                TileBase = _tilemap.GetTile(localPlace),
                TilemapMember = _tilemap,
                Name = localPlace.x + "," + localPlace.y,
                Cost = 1 // TODO: Change this with the proper cost from ruletile
            };
            tiles.Add(tile.WorldLocation, tile);
            if (_tilemap.name == "RoadMap")
            {
                roadTiles.vector.Add(tile.LocalPlace);
                roadTiles.name.Add(tile.TileBase.name);
                //roadTiles.Add(tile.LocalPlace, tile.TileBase.name);
            }
            else
            {
                wallTiles.Add(tile.WorldLocation); /*wallTilesInt.Add(tile.LocalPlace);*/
            }
        }
        if(!CameraController.instance.enabled)
        {
            CheckMinAndMaxPosRoadForCameraController();
            CameraController.instance.enabled = true;
            CameraController.instance.currentPlayer = player;
        }
        if (_statusGenerate == 0) { GenerateItems(); }
        else { GenerateProps(); }
    }
    public void OpenParking()
    {
        levelHelper.barrier.SetActive(false);
        int _posX = int.Parse(levelHelper.blockTile.x.ToString());
        int _posY = int.Parse(levelHelper.blockTile.y.ToString());
        int _posZ = int.Parse(levelHelper.blockTile.z.ToString());
        WallTilemap.SetTile(new Vector3Int(_posX, _posY, _posZ), null);
    }
    public void ClosedParking()
    {
        levelHelper.barrier.SetActive(true);
        int _posX = int.Parse(levelHelper.blockTile.x.ToString());
        int _posY = int.Parse(levelHelper.blockTile.y.ToString());
        int _posZ = int.Parse(levelHelper.blockTile.z.ToString());
        WallTilemap.SetTile(new Vector3Int(_posX, _posY, _posZ), wallBlockTile);
    }
    private void GenerateItems()
    {
        for (int i = 0; i < levelHelper.roadBlockTiles.Length; i++)
        {
            for (int k = 0; k < roadTiles.vector.Count; k++)
            {
                if (roadTiles.vector[k] == levelHelper.roadBlockTiles[i])
                {
                    //Debug.Log("Remove tile road: " + roadTiles.vector[k]);
                    roadTiles.vector.Remove(roadTiles.vector[k]);
                    roadTiles.name.Remove(roadTiles.name[k]);
                }
            }
        }
        maxCountFlagInLevel = UnityEngine.Random.Range(8, 12);
        countClockInScene = UnityEngine.Random.Range(7, 12);
        countFuelInScene = UnityEngine.Random.Range(7, 12);
        countCarburetorInScene = UnityEngine.Random.Range(7, 12);
        countRudderInScene = UnityEngine.Random.Range(7, 12);
        countRearViewMirror = UnityEngine.Random.Range(7, 12);
        InstanceInScenePoolItems(maxCountFlagInLevel, PoolManager.instance.flagsPool);
        InstanceInScenePoolItems(countClockInScene, PoolManager.instance.clocksPool);
        InstanceInScenePoolItems(countFuelInScene, PoolManager.instance.fuelsPool);
        InstanceInScenePoolItems(countCarburetorInScene, PoolManager.instance.carbureatorPool);
        InstanceInScenePoolItems(countRudderInScene, PoolManager.instance.rudderPool);
        InstanceInScenePoolItems(countRearViewMirror, PoolManager.instance.rearViewMirrorPool);
    }
    private void InstanceInScenePoolItems(int _count, List<ItemCollect> _items)
    {
        for (int i = 0; i < _count; i++)
        {
            int _random = UnityEngine.Random.Range(0, roadTiles.vector.Count);
            ItemCollect obj = _items[i];
            obj.gameObject.SetActive(true);
            obj.GetComponent<ItemCollect>().enabled = false;
            obj.GetComponent<ItemCollect>().dissolveAmount = 0f;
            obj.transform.position = roadTiles.vector[_random];
            obj.dissolveAmount = 0f;
            obj.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmount", 0f);
            itemInScene.Add(obj);
            roadTiles.vector.Remove(roadTiles.vector[_random]);
            roadTiles.name.Remove(roadTiles.name[_random]);
        }
    }
    private void GenerateProps()
    {
        if(PoolManager.instance.chapterProps[GameManager.instance.currentChapter].generateChapter)
        {
            for (int i = 0; i < wallTiles.Count; i++)
            {
                if (wallTiles[i] == levelHelper.blockTile)
                {
                    wallTiles.Remove(wallTiles[i]);
                }
            }
            for (int i = 0; i < PoolManager.instance.chapterProps[GameManager.instance.currentChapter].countWallsProps; i++)
            {
                int _randomPos = UnityEngine.Random.Range(0, wallTiles.Count);
                GameObject _obj = PoolManager.instance.chapterProps[GameManager.instance.currentChapter].wallProps[i];
                _obj.SetActive(true);
                _obj.transform.position = wallTiles[_randomPos] + PoolManager.instance.chapterProps[GameManager.instance.currentChapter].offsetWallProps;
                wallPropsInScene.Add(_obj);
                wallTiles.Remove(wallTiles[_randomPos]);
            }
        }
        for(int i = 0; i < PoolManager.instance.chapterProps[GameManager.instance.currentChapter].roadProps.Count; i++)
        {
            int _randomPos = UnityEngine.Random.Range(0, roadTiles.vector.Count);
            GameObject _obj = PoolManager.instance.chapterProps[GameManager.instance.currentChapter].roadProps[i];
            _obj.SetActive(true);
            float _randomEuler = UnityEngine.Random.Range(0, 270);
            _obj.transform.eulerAngles = new Vector3(0,0, _randomEuler);
            _obj.transform.position = roadTiles.vector[_randomPos];
            roadPropsInScene.Add(_obj);
        }
    }
    public void ClearRoadProps()
    {
        for(int i = 0; i < roadPropsInScene.Count; i++)
        {
            roadPropsInScene[i].SetActive(false);
        }
        roadPropsInScene.Clear();
    }
    public void ClearItems()
    {
        for (int i = 0; i < itemInScene.Count; i++)
        {
            itemInScene[i].gameObject.SetActive(false);
            itemInScene[i].collisionPlayerCar = false;
            StopCoroutine(itemInScene[i].DissolveSprite(false));
        }
        itemInScene.Clear();
        ClearRoadProps();
    }
    public void ClearWallPropsInScene()
    {
        for (int i = 0; i < wallPropsInScene.Count; i++)
        {
            wallPropsInScene[i].gameObject.SetActive(false);
        }
        wallPropsInScene.Clear();
        wallTiles.Clear();
        roadTiles.name.Clear();
        roadTiles.vector.Clear();
    }
    private void Update()
    {
        if(statusCurrentLevel == StatusCurrentLevel.process)
        {
            timeCount -= Time.deltaTime;

            TimeText.text = Mathf.Round(timeCount).ToString();

            fuelCount -= Time.deltaTime;

            float _percentFuel = (fuelCount / maxFuelCount) * 100;

            FuelProcentText.text = Math.Round(_percentFuel) + "%";

            //FuelSlider.fillAmount -= 0.01f / 1.33f * Time.deltaTime; 
            FuelSlider.fillAmount = _percentFuel / 100;

            if (timeCount <= 0)
            {
                EventManager.instance.StatusLevel(StatusCurrentLevel.nullTime);
                GameManager.instance.gameOverPanel.SetTrigger("Open");
            }
            else if(fuelCount <= 0)
            {
                EventManager.instance.StatusLevel(StatusCurrentLevel.nullFuel);
                GameManager.instance.gameOverPanel.SetTrigger("Open");
            }
        }
    }
}
public class WorldTile
{
    public Vector3Int LocalPlace { get; set; }
    public Vector3 WorldLocation { get; set; }
    public TileBase TileBase { get; set; }
    public Tilemap TilemapMember { get; set; }
    public string Name { get; set; }
    public int Cost { get; set; }
}
