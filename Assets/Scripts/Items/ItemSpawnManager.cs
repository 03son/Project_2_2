using Photon.Pun; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class FloorSpawnData
    {
        public Transform floorSpawnArea; // ���� ���� ��ġ �׷�
        public List<Spawn_ItemCount> itemsToSpawn; // �ش� ���� ������ ����Ʈ
    }

    public List<FloorSpawnData> floors; // ���� ���� ������ ����Ʈ
    private Dictionary<int, HashSet<int>> usedIndexesPerFloor = new Dictionary<int, HashSet<int>>(); // ������ �ߺ� ����

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnItemsPerFloor(); // MasterClient�� ���� ����
        }
    }

    void SpawnItemsPerFloor()
    {
        for (int floorIndex = 0; floorIndex < floors.Count; floorIndex++)
        {
            FloorSpawnData floorData = floors[floorIndex];
            Transform[] spawnPoints = floorData.floorSpawnArea.GetComponentsInChildren<Transform>();

            // �ߺ� ������ HashSet �ʱ�ȭ
            if (!usedIndexesPerFloor.ContainsKey(floorIndex))
            {
                usedIndexesPerFloor[floorIndex] = new HashSet<int>();
            }

            // �ش� ���� ������ ����
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
            if (usedIndexes.Count >= spawnPoints.Length - 1) // ��ġ�� ������ ���
            {
                Debug.Log("��ȯ���� ����");
                break;
            }

            int randomIndex;
            do
            {
                randomIndex = Random.Range(1, spawnPoints.Length); // 0��(�θ�) ����
            } while (usedIndexes.Contains(randomIndex)); // �ߺ� �˻�

            usedIndexes.Add(randomIndex);
            Vector3 spawnPosition = spawnPoints[randomIndex].position;

            // PhotonNetwork�� ����� ����ȭ�� ���� ����
           // PhotonNetwork.Instantiate($"Prefabs/Items/{itemData.itemPrefab.name}", spawnPosition, Quaternion.identity);
            PhotonNetwork.InstantiateRoomObject($"Prefabs/Items/{itemData.itemPrefab.name}", spawnPosition, Quaternion.identity);
        }
    }
}
