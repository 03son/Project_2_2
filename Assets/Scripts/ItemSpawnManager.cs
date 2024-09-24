using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    /* *** ������ ���� ��� ***
     �ڽ� ������Ʈ�� ��ġ���� �����ͼ� �ߺ����� �ʴ� ��ġ�� ��ȯ */

    public Transform spawnArea; // ��ġ�� �׷�ȭ�� �θ� ������Ʈ
    public List<ItemData> itemsToSpawn; // ���� ������ �����۰� �� ��ȯ ������ ����
    private HashSet<int> usedIndexes = new HashSet<int>(); // HashSet = ��� �����ۿ� ���� �ߺ��� ��ġ ����

    void Start()
    {
        // ������ ����
        SpawnAllItems();
    }

    void SpawnAllItems()
    {
        // spawnArea�� �ڽ� ������Ʈ��κ��� ������ ��ġ ����
        Transform[] spawnPoints = spawnArea.GetComponentsInChildren<Transform>();
        // ������ ����
        foreach (ItemData itemData in itemsToSpawn)
        {
            SpawnItems(itemData, spawnPoints);
        }
    }

    void SpawnItems(ItemData itemData, Transform[] spawnPoints)
    {
        int spawnCount = itemData.spawnCount; // �����տ� �ִ� spwanCount���� ������

        for (int i = 0; i < spawnCount; i++)
        {
            if (usedIndexes.Count >= spawnPoints.Length - 1) // ��� ��ġ�� ���Ǿ��� ��
            {
                Debug.Log("��� ��ġ�� ��ȯ�Ǿ����ϴ�.");
                break; // �� �̻� ��ȯ���� ����
            }

            int randomIndex;
            do
            {
                randomIndex = Random.Range(1, spawnPoints.Length); // 0�� �θ� ������Ʈ�̹Ƿ� ����
            } while (usedIndexes.Contains(randomIndex)); // �̹� ���� ��ġ���� Ȯ��

            usedIndexes.Add(randomIndex); // ���� ��ġ �߰�
            Vector3 spawnPosition = spawnPoints[randomIndex].position; // ���� ����� ���ͷ� ��ȯ
            Instantiate(itemData.itemPrefab, spawnPosition, Quaternion.identity); // �ش� ��ġ�� ������ ��ȯ
        }
    }
}
