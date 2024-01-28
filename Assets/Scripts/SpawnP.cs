using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnP : MonoBehaviour
{
    public GameObject[] prefabs; //찍어낼 게임 오브젝트 넣기

    private BoxCollider area;
    public int count = 100;

    private List<GameObject> gameObject = new List<GameObject>();
    void Start()
    {
        area = GetComponent<BoxCollider>();

        for (int i = 0; i < count; ++i)  //count수만큼 생성
        {
            Spawn(); //생성 + 스폰위치 포함하는 함수
        }

        area.enabled = false;
    }

    private void Spawn()
    {
        int selection = Random.Range(0, prefabs.Length);

        GameObject selectedPrefab = prefabs[selection];

        Vector3 spawnPos = GetRandomPosition();//랜덤위치함수

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        gameObject.Add(instance);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;
        Vector3 size = area.size;

        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);

        return spawnPos;
    }

    
}

