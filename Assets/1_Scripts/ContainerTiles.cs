using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class ContainerTiles : MonoBehaviour
    {
        private List<Tile> tiles = new List<Tile>();

        public void AddTileInEnd(Tile tile)
        {
            tiles.Add(tile);
            tile.transform.SetParent(transform);
            tile.transform.localScale = new Vector3(1, 1, 1);
            tile.transform.position = new Vector3(0, 0, 0);
            tile.SetContainer = this;
        }

        public void AddTileInCenter(Tile tile)
        {
            float minDistance = Mathf.Abs(tile.transform.position.y - tiles[0].transform.position.y);
            int numberTile = 0;

            for (int i = 0; i < tiles.Count; i++)
            {
                float distance = Mathf.Abs(tile.transform.position.y - tiles[i].transform.position.y);

                if(distance < minDistance)
                {
                    minDistance = distance;
                    numberTile = i;
                }
            }

            tile.transform.SetParent(transform);
            tile.transform.SetSiblingIndex(numberTile);
            tile.SetContainer = this;
        }

        public void RemoveTile(Tile tile)
        {
            if (tiles.Contains(tile))
            {
                tiles.Remove(tile);
            }
        }
    }
}