using BzKovSoft.ObjectSlicer;
using DB.Knight.AI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Knight.Slice
{
    public class SliceableCharacter : MonoBehaviour
    {
        [FoldoutGroup("Mesh Input")]
        [SerializeField] private SkinnedMeshRenderer _skinnedMesh;
        [FoldoutGroup("Mesh Input")]
        [SerializeField] private Material _meatMat;
        [SerializeField] private Collider _myCollider;
        [SerializeField] private SimpleEnemy _enemy;

        [Button("Bake Mesh")]
        public BzSliceableCollider BakeMesh()
        {
            if(_enemy != null)
            {
                _enemy.Die();
            }

            _myCollider.enabled = false;

            GameObject go = new GameObject();

            go.transform.position = transform.position;
            go.transform.rotation = _skinnedMesh.transform.rotation;
            go.transform.localScale = transform.localScale;
            go.transform.parent = null;
            go.layer = gameObject.layer;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            Mesh mesh = new Mesh();
            _skinnedMesh.BakeMesh(mesh);
            mf.mesh = mesh;
            mr.materials = _skinnedMesh.materials;
            _skinnedMesh.gameObject.SetActive(false);

            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.drag = 1;
            go.AddComponent<MeshCollider>().convex = true;
            BzSliceableCollider bsc = go.AddComponent<BzSliceableCollider>();
            bsc.SectionViewMaterial = _meatMat;
            Destroy(gameObject, 0.01f);
            return bsc;
        }
    }
}