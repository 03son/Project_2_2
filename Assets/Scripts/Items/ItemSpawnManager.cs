using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    /* *** 아이템 스폰 방식 ***
     자식 오브젝트의 위치값을 가져와서 중복되지 않는 위치에 소환 */

    public Transform spawnArea; // 위치를 그룹화한 부모 오브젝트
    public List<Spawn_ItemCount> itemsToSpawn; // 여러 종류의 아이템과 각 소환 개수를 저장
    private HashSet<int> usedIndexes = new HashSet<int>(); // HashSet = 모든 아이템에 걸쳐 중복된 위치 방지

    void Start()
    {
        // 아이템 스폰
        SpawnAllItems();
    }

    void SpawnAllItems()
    {
        // spawnArea의 자식 오브젝트들로부터 랜덤한 위치 선택
        Transform[] spawnPoints = spawnArea.GetComponentsInChildren<Transform>();
        // 아이템 스폰
        foreach (Spawn_ItemCount itemData in itemsToSpawn)
        {
            SpawnItems(itemData, spawnPoints);
        }
    }

    void SpawnItems(Spawn_ItemCount itemData, Transform[] spawnPoints)
    {
        int spawnCount = itemData.spawnCount; // 프리팹에 있는 spwanCount값을 가져옴

        for (int i = 0; i < spawnCount; i++)
        {
            if (usedIndexes.Count >= spawnPoints.Length - 1) // 더 이상 사용할 위치가 없음
            {
                Debug.Log("소환공간 없음");
                break; // 더 이상 소환하지 않음
            }

            int randomIndex;
            do
            {
                randomIndex = Random.Range(1, spawnPoints.Length); // 0은 부모 오브젝트이므로 제외
            } while (usedIndexes.Contains(randomIndex)); // 이미 사용된 위치인지 확인

            usedIndexes.Add(randomIndex); // 사용된 위치 추가
            Vector3 spawnPosition = spawnPoints[randomIndex].position; // 나온 결과를 벡터로 변환
            Instantiate(itemData.itemPrefab, spawnPosition, Quaternion.identity); // 해당 위치에 아이템 소환
        }
    }
}
