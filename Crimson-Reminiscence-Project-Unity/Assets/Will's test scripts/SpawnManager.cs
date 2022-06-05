using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> spawnPoints;
    public int currentIndex;

    public PlayerController playerController;

    public void SetCurrentSpawnPoint(GameObject zone)
    {
        int index = spawnPoints.IndexOf(zone);
        if (index >= 0 && index < spawnPoints.Count)
        {
            currentIndex = index;
        }
    }

    public IEnumerator SpawnPlayer()
    {
        playerController.disable = true;
        yield return new WaitForSeconds(1);
        print(spawnPoints[currentIndex].transform.position);
        playerController.transform.position = spawnPoints[currentIndex].transform.position;
        yield return new WaitForSeconds(1);
        playerController.disable = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            StartCoroutine(SpawnPlayer());
    }
}
