using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace EternalDefenders
{
    public class MapBuilder : MonoBehaviour
    {
        [SerializeField] List<GameObject> chunkPrefabs;
        [SerializeField] Transform mapParent;

        List<Vector3> _chunkPositions;
        List<Quaternion> _chunkRotations;
        List<int> _chunkTypes;

        readonly float _hexSize = HexMapController.HexSize;
        readonly int _width = HexMapController.MapWidthChunks;
        readonly int _height = HexMapController.MapHeightChunks;
        readonly string _filePath = Path.Combine(Application.dataPath, "_Project/MapConfigs/Otlupium.json");

        const float Sqrt3 = 1.73205080757f;
        
        
        void SetUp()
        {
            string json = File.ReadAllText(_filePath);
            int[][] mapChunks = JsonConvert.DeserializeObject<int[][]>(json);

            _chunkTypes = new List<int>(_width * _height);
            _chunkPositions = new List<Vector3>(_width * _height);
            _chunkRotations = new List<Quaternion>(_width * _height);

            for (int x = 0; x < _width; x++)
            {
                bool oddColumn = x % 2 == 1;
                for (int y = 0; y < _height; y++)
                {
                    float calcX = 4.5f * x * _hexSize;
                    float calcY = -1 * 2 * Sqrt3 * y * _hexSize;

                    if (oddColumn)
                    {
                        calcY -= 0.5f * _hexSize * Sqrt3;
                        _chunkRotations.Add(Quaternion.Euler(0, 180, 0));
                    }
                    else
                    {
                        _chunkRotations.Add(Quaternion.identity);
                    }

                    _chunkPositions.Add(new Vector3(calcX, 0, calcY));
                    _chunkTypes.Add(mapChunks[y][x]);
                }
            }
        }
        public void GenerateMap()
        {
            SetUp();
            for (int chunkIndex = 0; chunkIndex < _width * _height; chunkIndex++)
            {
                var chunk = Instantiate(chunkPrefabs[_chunkTypes[chunkIndex]], 
                    _chunkPositions[chunkIndex], _chunkRotations[chunkIndex]);
                chunk.transform.parent = mapParent;
            }
        }
    }
}
