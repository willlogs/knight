using DB.Knight.Horse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.Knight.Levels
{
    public class EnemySpawner : MonoBehaviour
    {
        public UnityEvent OnActivation;

        [SerializeField] private GameObject[] _knightPrefabs;
        [SerializeField] private Transform[] _wps;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private int _counterActivator = 2;

        [SerializeField] private int _dispatchCount = 1;
        [SerializeField] private float _betweenSpawns = 1f;

        int count = 0;
        public void SpawnOne()
        {
            count++;
            if (count == _counterActivator)
            {
                OnActivation?.Invoke();
            }
            StartCoroutine(SpawnThem());
        }

        private void SpawnSingle()
        {
            GameObject go = Instantiate(_knightPrefabs[Random.Range(0, _knightPrefabs.Length)]);
            go.transform.position = _spawnPoint.position;
            WaypointWalker ww = go.GetComponent<WaypointWalker>();
            ww._waypoints = _wps;
            ww.GotoNext(true);
        }

        private IEnumerator SpawnThem()
        {
            for(int i = 0; i < _dispatchCount; i++)
            {
                yield return new WaitForSeconds(_betweenSpawns);
                SpawnSingle();
            }
        }
    }
}