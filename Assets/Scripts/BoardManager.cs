using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;
	public GameObject player;
	public MapGenerator mapGenerator;
	public int spawnCorner = 4;

	private Transform boardHolder;
	private int[,] map;

	public void CallGenerateMap ()
	{
		map = mapGenerator.GenerateMap ();
	}

	public void BoardSetup ()
	{
		LayoutTiles ();
		LayoutFood ();
		LayoutExit ();
		SpawnPlayer ();
		SpawnEnemies ();
	}

	void LayoutTiles ()
	{
		boardHolder = new GameObject ("Board").transform;
		for (int x = 0; x < map.GetLength(0); x++) {
			for (int y = 0; y < map.GetLength(1); y++) {
				if (map [x, y] == 1) {
					GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (boardHolder);
				} else if (map [x, y] == 0) {
					GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
					GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (boardHolder);
				}
			}
		}
	}

	void LayoutFood ()
	{
		int foodCount = 5;
		Transform foodHolder = new GameObject ("FoodHolder").transform;
		for (int x = 0; x < foodCount; x++) {
			Coord place = mapGenerator.GetRandomTile (map);
			GameObject toInstantiate = foodTiles [Random.Range (0, foodTiles.Length)];
			GameObject instance = Instantiate (toInstantiate, new Vector3 (place.tileX, place.tileY, 0f), Quaternion.identity) as GameObject;
			instance.transform.SetParent (foodHolder);
		}
	}

	void LayoutExit ()
	{
		Coord spawn = mapGenerator.GetCornerSpawnPoint (map, 5 - spawnCorner);
		GameObject instance = Instantiate (exit, new Vector3 (spawn.tileX, spawn.tileY, 0f), Quaternion.identity) as GameObject;
		Transform exitHolder = new GameObject ("ExitHolder").transform;
		instance.transform.SetParent (exitHolder);
	}

	void SpawnPlayer ()
	{
		Coord spawn = mapGenerator.GetCornerSpawnPoint (map, spawnCorner);
		GameObject playerInstance = Instantiate (player, new Vector3 (spawn.tileX, spawn.tileY, 0f), Quaternion.identity) as GameObject;
		Transform playerHolder = new GameObject ("PlayerHolder").transform;
		playerInstance.transform.SetParent (playerHolder);
	}

	void SpawnEnemies ()
	{
		int enemyCount = 2;
		Transform foodHolder = new GameObject ("EnemyHolder").transform;
		for (int x = 0; x < enemyCount; x++) {
			Coord spawn = mapGenerator.GetRandomTile (map);
			GameObject toInstantiate = enemyTiles [Random.Range (0, enemyTiles.Length)];
			GameObject instance = Instantiate (toInstantiate, new Vector3 (spawn.tileX, spawn.tileY, 0f), Quaternion.identity) as GameObject;
			instance.transform.SetParent (foodHolder);
		}
	}

	public struct Coord
	{
		public int tileX;
		public int tileY;

		public Coord (int x, int y)
		{
			tileX = x;
			tileY = y;
		}
	}
}
