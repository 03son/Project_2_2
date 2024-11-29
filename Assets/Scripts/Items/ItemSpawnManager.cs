using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class FloorSpawnData
    {
        public Transform floorSpawnArea; // 층별 스폰 위치 그룹
        public List<Spawn_ItemCount> itemsToSpawn; // 해당 층의 아이템 리스트
    }

    public List<FloorSpawnData> floors; // 여러 층의 데이터 리스트
    private Dictionary<int, HashSet<int>> usedIndexesPerFloor = new Dictionary<int, HashSet<int>>(); // 층별로 중복 관리

    void Start()
    {
        SpawnItemsPerFloor();
    }

    void SpawnItemsPerFloor()
    {
        for (int floorIndex = 0; floorIndex < floors.Count; floorIndex++)
        {
            FloorSpawnData floorData = floors[floorIndex];
            Transform[] spawnPoints = floorData.floorSpawnArea.GetComponentsInChildren<Transform>();

            // 중복 방지용 HashSet 초기화
            if (!usedIndexesPerFloor.ContainsKey(floorIndex))
            {
                usedIndexesPerFloor[floorIndex] = new HashSet<int>();
            }

            // 해당 층의 아이템 스폰
            foreach (Spawn_ItemCount itemData in floorData.itemsToSpawn)
            {
                SpawnItemsOnFloor(itemData, spawnPoints, usedIndexesPerFloor[floorIndex]);
            }
        }
    }

    void SpawnItemsOnFloor(Spawn_ItemCount itemData, Transform[] spawnPoints, HashSet<int> usedIndexes)
    {
        int spawnCount = itemData.spawnCount;

        for (int i = 0; i < spawnCount; i++)
        {
            if (usedIndexes.Count >= spawnPoints.Length - 1) // 위치가 부족할 경우
            {
                Debug.Log("소환공간 부족");
                break;
            }

            int randomIndex;
            do
            {
                randomIndex = Random.Range(1, spawnPoints.Length); // 0번(부모) 제외
            } while (usedIndexes.Contains(randomIndex)); // 중복 검사

            usedIndexes.Add(randomIndex);
            Vector3 spawnPosition = spawnPoints[randomIndex].position;
            Instantiate(itemData.itemPrefab, spawnPosition, Quaternion.identity);
        }
    }
}