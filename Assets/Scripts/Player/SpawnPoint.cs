using System.Collections;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] Player playerPrefab;
        [Min(0f)]
        [SerializeField] float timeToSpawn;

        private Player activePlayer;


        private void Start()
        {
            InitalizePlayer();
        }


        private void InitalizePlayer()
        {
            activePlayer = Instantiate(playerPrefab);
            activePlayer.transform.position = transform.position;
            activePlayer.OnPlayerDeath += PlayerDeathHandler;
        }

        private void PlayerDeathHandler()
        {
            activePlayer.OnPlayerDeath -= PlayerDeathHandler;
            activePlayer = null;
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            Debug.Log("Spawning...");
            yield return new WaitForSeconds(timeToSpawn);
            InitalizePlayer();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
}

