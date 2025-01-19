using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders
{
    public class ChunkCreator : MonoBehaviour
    {
        [SerializeField] Transform[] hexPrefabs = new Transform[HexCount];
        [SerializeField] string prefabName;
        const int HexCount = 6;
        const float Sqrt3 = 1.73205080757f;

        List<Vector3> _positions;
        Transform _newChunkTransform;
        
        readonly float _hexSize = HexMapController.HexSize;

        void SetUp()
        {
            _positions = new List<Vector3>
            {
                new Vector3(0, 0, 0),
                new Vector3((float)-1.5 * _hexSize, 0, (float)0.5 * _hexSize * Sqrt3),
                new Vector3((float)1.5 * _hexSize, 0, (float)0.5 * _hexSize * Sqrt3),
                new Vector3((float)-1.5 * _hexSize, 0, (float)-0.5 * _hexSize * Sqrt3),
                new Vector3((float)1.5 * _hexSize, 0, (float)-0.5 * _hexSize * Sqrt3),
                new Vector3(0, 0, -Sqrt3 * _hexSize)
            };
        }

        public void CreateChunk()
        {
            SetUp();
            
            _newChunkTransform = gameObject.transform.GetChild(0);
            for (int i = 0; i < HexCount; i++)
            {
                Transform hex = Instantiate(hexPrefabs[i], transform.position + _positions[i], Quaternion.identity);
                hex.transform.parent = _newChunkTransform.GetChild(0);
            }
            
            string prefabPath = $"Assets/_Project/Prefabs/Chunks/{prefabName}.prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(_newChunkTransform.gameObject, prefabPath);
            if (prefab)
                Debug.Log($"Prefab created at {prefabPath}");
            else
                Debug.LogError("Failed to create prefab.");
            
            DestroyImmediate(_newChunkTransform.gameObject);
            GameObject nextChunk = new("ChunkToBeMadePrefabOf");
            nextChunk.transform.parent = transform;
            GameObject nextVisual = new("Chunk Visual");
            nextVisual.transform.parent = nextChunk.transform;
            prefabName = "ChunkDefaultName";
        }
    }
}