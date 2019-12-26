using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour {

    public NavMeshSurface surface;

	public Map[] maps;
	public int mapIndex;
	
	public Transform tilePrefab;
	public Transform obstaclePrefab;
    public Transform torchPrefab;
	public Transform mapFloor;
	public Transform navmeshFloor;
	public Transform navmeshMaskPrefab;
	public Vector2 maxMapSize;
	
	[Range(0,1)]
	public float outlinePercent;
	
	public float tileSize;
	List<Coord> allTileCoords;
	Queue<Coord> shuffledTileCoords;
	Queue<Coord> shuffledOpenTileCoords;
	Transform[,] tileMap;
	
	Map currentMap;

    bool[,] obstacleMap;

    //[Header ("-던전 시작, 보스 방 입구")]
    //public GameObject mapStart;
 //   public GameObject bossDoor;
 //   [SerializeField]
 //   List<int> startListNum = new List<int>();
 //   List<Vector3> tileVecList = new List<Vector3>();
 //   List<Vector3> obstacleVectorList = new List<Vector3>();
 //   List<Vector3> nonObsVecList = new List<Vector3>();
 //   List<Vector3> leftNonObsVecList = new List<Vector3>();
 //   List<Vector3> rightNonObsVecList = new List<Vector3>();
 //   List<Vector3> topNonObsVecList = new List<Vector3>();
 //   List<Vector3> bottomNonObsVecList = new List<Vector3>();


 //   private void Start()
 //   {
 //       ChangeToNonObstaclePos();
 //       int randomSide = Random.Range(0, 3);
 //       CreateStartPos(randomSide);
 //   }

 //   void Awake() {
 //       //FindObjectOfType<Spawner> ().OnNewWave += OnNewWave;
 //       surface.BuildNavMesh();
        
 //   }

	//void OnNewWave(int waveNumber) {
	//	mapIndex = waveNumber - 1;
	//	GenerateMap ();
 //   }

 //   void GenerateEntrance()
 //   {
 //       startListNum.Clear();
 //       int randomSide = Random.Range(0, 3);
 //       //각 사이드에 따른 벽확인
 //       switch(randomSide)
 //       {
 //           case 0:
 //               for (int i = 0; i < maxMapSize.y; i++) // 왼쪽벽 확인
 //               {
 //                   if (obstacleMap[0, i] == false)
 //                   {
 //                       startListNum.Add(i);
 //                       CreateStartPos(randomSide);
 //                   }
 //               }
 //               break;
 //           case 1:
 //               for (int i = 0; i < maxMapSize.y; i++) // 오른쪽벽 확인
 //               {
 //                   if (obstacleMap[19, i] == false)
 //                   {
 //                       startListNum.Add(i);
 //                       CreateStartPos(randomSide);
 //                   }
 //               }
 //               break;
 //           case 2:
 //               for (int i = 0; i < maxMapSize.x; i++) // 위쪽벽 확인
 //               {
 //                   if (obstacleMap[i, 0] == false)
 //                   {
 //                       startListNum.Add(i);
 //                       CreateStartPos(randomSide);
 //                   }
 //               }
 //               break;
 //           case 3:
 //               for (int i = 0; i < maxMapSize.x; i++) // 아래쪽벽 확인
 //               {
 //                   if (obstacleMap[i, 19] == false)
 //                   {
 //                       startListNum.Add(i);
 //                       CreateStartPos(randomSide);
 //                   }
 //               }
 //               break;
 //       }
 //       //안막힌 벽에 랜덤으로 시작생성
        
 //   }

 //   void CreateStartPos(int randomSide)
 //   {
 //       switch(randomSide)
 //       {
 //           case 0: //왼쪽
 //               int rand = Random.Range(0, leftNonObsVecList.Count-1);
 //               mapStart.transform.position = leftNonObsVecList[rand];
 //               break;
 //           case 1: //오른쪽
 //               int rand1 = Random.Range(0, rightNonObsVecList.Count - 1);
 //               mapStart.transform.position = rightNonObsVecList[rand1];
 //               break;
 //           case 2: //위쪽
 //               int rand2 = Random.Range(0, topNonObsVecList.Count - 1);
 //               mapStart.transform.position = topNonObsVecList[rand2];
 //               break;
 //           case 3: //아래쪽
 //               int rand3 = Random.Range(0, bottomNonObsVecList.Count - 1);
 //               mapStart.transform.position = bottomNonObsVecList[rand3];
 //               break;
 //       }
 //   }

 //   void GetObstaclePos(Vector3 obstacleV) //랜덤 장애물 포지션 받아옴
 //   {
 //       obstacleVectorList.Add(obstacleV);
        
 //   }

 //   void ChangeToNonObstaclePos() //랜덤장애물포지션을 제외한 나머지 포지션(여유공간 포지션)을 얻어야함
 //   {
 //       for (int x = 0; x < maxMapSize.x; x++)
 //       {
 //           for(int z = 0; z < maxMapSize.y; z++)
 //           {
 //               Vector3 tilePos = new Vector3(-19 + (float)x*2, 19 - (float)z*2);
 //               tileVecList.Add(tilePos);
                
 //           }
 //       }
 //       for (int i = 0; i < tileVecList.Count; i++)
 //       {
 //           for(int r = 0; r < obstacleVectorList.Count; r++)
 //           {
 //               if (obstacleVectorList[r] != tileVecList[i])
 //                   nonObsVecList.Add(tileVecList[i]);
 //           }
            
 //       }
 //   }

 //   void GetSideObstaclePos()
 //   {
 //       for (int i = 0; i < nonObsVecList.Count; i++)   //각 사이드리스트 생성
 //       {
 //           if (nonObsVecList[i].x == -19)
 //           {
 //               leftNonObsVecList.Add(nonObsVecList[i]);
 //           }
 //           if (nonObsVecList[i].x == 19)
 //           {
 //               rightNonObsVecList.Add(nonObsVecList[i]);
 //           }
 //           if (nonObsVecList[i].y == 19)
 //           {
 //               topNonObsVecList.Add(nonObsVecList[i]);
 //           }
 //           if (nonObsVecList[i].y == -19)
 //           {
 //               bottomNonObsVecList.Add(nonObsVecList[i]);
 //           }
 //       }
 //   }

    public void GenerateMap() {
		currentMap = maps[mapIndex];
		tileMap = new Transform[currentMap.mapSize.x,currentMap.mapSize.y];
		System.Random prng = new System.Random (currentMap.seed);

		// Generating coords
		allTileCoords = new List<Coord> ();
		for (int x = 0; x < currentMap.mapSize.x; x ++) {
			for (int y = 0; y < currentMap.mapSize.y; y ++) {
				allTileCoords.Add(new Coord(x,y));
			}
		}
		shuffledTileCoords = new Queue<Coord> (Utility.ShuffleArray (allTileCoords.ToArray (), currentMap.seed));

		// Create map holder object
		string holderName = "Generated Map";
		if (transform.Find (holderName)) {
			DestroyImmediate (transform.Find (holderName).gameObject);
		}
		
		Transform mapHolder = new GameObject (holderName).transform;
		mapHolder.parent = transform;

		// Spawning tiles
		for (int x = 0; x < currentMap.mapSize.x; x ++) {
			for (int y = 0; y < currentMap.mapSize.y; y ++) {
				Vector3 tilePosition = CoordToPosition(x,y);
				Transform newTile = Instantiate (tilePrefab, tilePosition, Quaternion.Euler (Vector3.right * 90)) as Transform;
				newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
				newTile.parent = mapHolder;
				tileMap[x,y] = newTile;
			}
		}

		// Spawning obstacles
		obstacleMap = new bool[(int)currentMap.mapSize.x,(int)currentMap.mapSize.y];
		
		int obstacleCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent);
		int currentObstacleCount = 0;
		List<Coord> allOpenCoords = new List<Coord> (allTileCoords);
		
		for (int i =0; i < obstacleCount; i ++) {
			Coord randomCoord = GetRandomCoord();
			obstacleMap[randomCoord.x,randomCoord.y] = true;
			currentObstacleCount ++;

			if (randomCoord != currentMap.mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount)) {
				float obstacleHeight = Mathf.Lerp(currentMap.minObstacleHeight,currentMap.maxObstacleHeight,(float)prng.NextDouble());
				Vector3 obstaclePosition = CoordToPosition(randomCoord.x,randomCoord.y);
				
				Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight/2, Quaternion.identity) as Transform;
				newObstacle.parent = mapHolder;
				//newObstacle.localScale = new Vector3((1 - outlinePercent) * tileSize, obstacleHeight, (1 - outlinePercent) * tileSize);
                newObstacle.localScale = new Vector3(tileSize, obstacleHeight, tileSize);
                Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
				Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
				float colourPercent = randomCoord.y / (float)currentMap.mapSize.y;
				obstacleMaterial.color = Color.Lerp(currentMap.foregroundColour,currentMap.backgroundColour,colourPercent);
				obstacleRenderer.sharedMaterial = obstacleMaterial;

                //GetObstaclePos(obstaclePosition);

                allOpenCoords.Remove(randomCoord);
			}
			else {
				obstacleMap[randomCoord.x,randomCoord.y] = false;
				currentObstacleCount --;
			}
		}

		shuffledOpenTileCoords = new Queue<Coord> (Utility.ShuffleArray (allOpenCoords.ToArray (), currentMap.seed));

        
        // Creating navmesh mask
        Transform maskLeft = Instantiate (navmeshMaskPrefab, Vector3.left * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
		maskLeft.parent = mapHolder;
		maskLeft.localScale = new Vector3 ((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;
		
		Transform maskRight = Instantiate (navmeshMaskPrefab, Vector3.right * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
		maskRight.parent = mapHolder;
		maskRight.localScale = new Vector3 ((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;
		
		Transform maskTop = Instantiate (navmeshMaskPrefab, Vector3.forward * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
		maskTop.parent = mapHolder;
		maskTop.localScale = new Vector3 (maxMapSize.x, 1, (maxMapSize.y-currentMap.mapSize.y)/2f) * tileSize;
	
		Transform maskBottom = Instantiate (navmeshMaskPrefab, Vector3.back * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
		maskBottom.parent = mapHolder;
		maskBottom.localScale = new Vector3 (maxMapSize.x, 1, (maxMapSize.y-currentMap.mapSize.y)/2f) * tileSize;
		
		navmeshFloor.localScale = new Vector3 (maxMapSize.x, maxMapSize.y) * tileSize;
		mapFloor.localScale =  new Vector3 (currentMap.mapSize.x * tileSize, currentMap.mapSize.y * tileSize);
	}
	
	bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount) {
		bool[,] mapFlags = new bool[obstacleMap.GetLength(0),obstacleMap.GetLength(1)];
		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (currentMap.mapCentre);
		mapFlags [currentMap.mapCentre.x, currentMap.mapCentre.y] = true;
		
		int accessibleTileCount = 1;
		
		while (queue.Count > 0) {
			Coord tile = queue.Dequeue();
			
			for (int x = -1; x <= 1; x ++) {
				for (int y = -1; y <= 1; y ++) {
					int neighbourX = tile.x + x;
					int neighbourY = tile.y + y;
					if (x == 0 || y == 0) {
						if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1)) {
							if (!mapFlags[neighbourX,neighbourY] && !obstacleMap[neighbourX,neighbourY]) {
								mapFlags[neighbourX,neighbourY] = true;
								queue.Enqueue(new Coord(neighbourX,neighbourY));
								accessibleTileCount ++;
							}
						}
					}
				}
			}
		}
		
		int targetAccessibleTileCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y - currentObstacleCount);
		return targetAccessibleTileCount == accessibleTileCount;
	}
	
	Vector3 CoordToPosition(int x, int y) {
		return new Vector3 (-currentMap.mapSize.x / 2f + 0.5f + x, 0, -currentMap.mapSize.y / 2f + 0.5f + y) * tileSize;
	}

	public Transform GetTileFromPosition(Vector3 position) {
		int x = Mathf.RoundToInt(position.x / tileSize + (currentMap.mapSize.x - 1) / 2f);
		int y = Mathf.RoundToInt(position.z / tileSize + (currentMap.mapSize.y - 1) / 2f);
		x = Mathf.Clamp (x, 0, tileMap.GetLength (0) -1);
		y = Mathf.Clamp (y, 0, tileMap.GetLength (1) -1);
		return tileMap [x, y];
	}
	
	public Coord GetRandomCoord() {
		Coord randomCoord = shuffledTileCoords.Dequeue ();
		shuffledTileCoords.Enqueue (randomCoord);
		return randomCoord;
	}

	public Transform GetRandomOpenTile() {
		Coord randomCoord = shuffledOpenTileCoords.Dequeue ();
		shuffledOpenTileCoords.Enqueue (randomCoord);
		return tileMap[randomCoord.x,randomCoord.y];
	}
	
	[System.Serializable]
	public struct Coord {
		public int x;
		public int y;
		
		public Coord(int _x, int _y) {
			x = _x;
			y = _y;
		}
		
		public static bool operator ==(Coord c1, Coord c2) {
			return c1.x == c2.x && c1.y == c2.y;
		}
		
		public static bool operator !=(Coord c1, Coord c2) {
			return !(c1 == c2);
		}
		
	}
	
	[System.Serializable]
	public class Map {
		
		public Coord mapSize;
		[Range(0,1)]
		public float obstaclePercent;
		public int seed;
		public float minObstacleHeight;
		public float maxObstacleHeight;
		public Color foregroundColour;
		public Color backgroundColour;
		
		public Coord mapCentre {
			get {
				return new Coord(mapSize.x/2,mapSize.y/2);
			}
		}
		
	}
}
