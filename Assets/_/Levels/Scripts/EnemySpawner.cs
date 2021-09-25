using DB.Knight.Horse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Knight.Levels
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _knightPrefabs;
        [SerializeField] private Transform[] _wps;
        [SerializeField] private Transform _spawnPoint;

        public void SpawnOne()
        {
            GameObject go = Instantiate(_knightPrefabs[Random.Range(0, _knightPrefabs.Length)]);
            go.transform.position = _spawnPoint.position;
            WaypointWalker ww = go.GetComponent<WaypointWalker>();
            ww._waypoints = _wps;
            ww.GotoNext(true);
        }
    }
}