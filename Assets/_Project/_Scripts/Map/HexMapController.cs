using System;
using System.Collections.Generic;
using System.Linq;
using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class HexMapController : Singleton<HexMapController>
    {
        [SerializeField] Transform mapTransform;
        public static float HexSize => 6f;
        public static int MapWidthChunks => 20;
        public static int MapHeightChunks => 15;

        readonly HexTile[,] _map = new HexTile[MapWidthChunks * HexPerChunkX, MapHeightChunks*HexPerChunkY];

        const int HexPerChunkX = 3;
        const int HexPerChunkY = 2;
        const float Sqrt3 = 1.73205080757f;
        
        void Start()
        {
            PopulateMap();
        }

        public HexTile GetHexTileFromWorldPosition(Vector3 worldPosition)
        {
            Vector3 gridPosition = worldPosition - transform.position;
            
            //NOTE: We need to implement this if the map will be ever rotated...
            //gridPosition = transform.rotation.y * gridPosition;
            float xSize = HexSize * 1.5f;
            float ySize = HexSize * Sqrt3;
            
            float x = gridPosition.x + 0.5f * HexSize; //because we are starting from -1.5*hexSize
            float y = (gridPosition.z * -1) + HexSize * Sqrt3; //because we are going towards negative Z axis
            
            int indexY = 0;
            int indexX = Mathf.FloorToInt(x / xSize) + 1;
            if (x < 0) indexX = 0;
            
            if (indexX % 2 == 0)
            {
                indexY = Mathf.FloorToInt(y / ySize);
            }
            else
            {
                float changedY = y - HexSize * Sqrt3 * 0.5f;
                indexY = Mathf.FloorToInt(changedY / ySize);
            }
            
            if (y < 0) indexY = 0;
            
            var neighbours = GetHexTileNeighbours(indexX, indexY);
            neighbours.Add(_map[indexX, indexY]);
            
            HexTile closest = null;
            float minDistance = float.MaxValue;
            
            foreach(HexTile tile in neighbours)
            {
                float distance = Vector3.Distance(tile.transform.position, worldPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = tile;
                }
            }

            return closest;
        }
        
        List<HexTile> GetHexTileNeighbours(int x, int y)
        {
            List<HexTile> neighbours = new List<HexTile>(6);

            if (x < 0 || x >= MapWidthChunks || y < 0 || y >= MapHeightChunks)
            {
                throw new ArgumentException("Wrong tile coordinates");
            }
            
            if(y >= 1)
                neighbours.Add(_map[x, y - 1]);
            if(y < MapHeightChunks - 1)
                neighbours.Add(_map[x, y + 1]);

            if (x % 2 == 0)
            {
                if (x > 0)
                {
                    neighbours.Add(_map[x - 1, y]);
                    if (y >= 1)
                    {
                        neighbours.Add(_map[x - 1, y - 1]);
                    }
                    
                }
                if(x < MapWidthChunks - 1)
                {
                    neighbours.Add(_map[x + 1, y]);
                    if (y >= 1)
                    {
                        neighbours.Add(_map[x + 1, y - 1]);
                    }
                }
            }
            else
            {
                neighbours.Add(_map[x - 1, y]);
                if (y < MapHeightChunks - 1)
                {
                    neighbours.Add(_map[x - 1, y + 1]);
                }
               
                if(x < MapWidthChunks - 1)
                {
                    neighbours.Add(_map[x + 1, y]);
                    if (y < MapHeightChunks - 1)
                    {
                        neighbours.Add(_map[x + 1, y + 1]);
                    }
                }
            }
            
            return neighbours;
        }

        void PopulateMap()
        {
            List<Transform> chunks = mapTransform.GetComponentsInDirectChildren<Transform>();

            int columns = 0;
            bool even = true;
            for(int x = 0; x < MapWidthChunks * HexPerChunkX; x++)
            {
                if (columns == HexPerChunkX)
                {
                    columns = 0;
                    even = !even;
                }             
                columns++;
                
                for(int y = 0; y < MapHeightChunks * HexPerChunkY; y++)
                {
                    int chunkX = x / HexPerChunkX;
                    int chunkY = y / HexPerChunkY;
                    
                    int chunkIndex = chunkX * MapHeightChunks + chunkY;
                    
                    List<HexTile> hexes = chunks[chunkIndex].GetComponentsInChildren<HexTile>().ToList();
                    int hexIndex = -1;
                    if (even)
                    {
                        hexIndex = (x % HexPerChunkX) switch
                        {
                            0 => (y % HexPerChunkY) == 0 ? 1 : 3,
                            1 => (y % HexPerChunkY) == 0 ? 0 : 5,
                            _ => (y % HexPerChunkY) == 0 ? 2 : 4
                        };
                    }
                    else
                    {
                        hexIndex = (x % HexPerChunkX) switch
                        {
                            0 => (y % HexPerChunkY) == 0 ? 4 : 2,
                            1 => (y % HexPerChunkY) == 0 ? 5 : 0,
                            _ => (y % HexPerChunkY) == 0 ? 3 : 1
                        };
                    }
                    _map[x, y] = hexes[hexIndex];
                }
            }
        }
        
    }
}