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
            int indexTile = 0;

            if (tiles.Count > 0)
            {
                indexTile = tiles[0].transform.GetSiblingIndex();

                float minDistance = Mathf.Abs(tile.transform.position.y - tiles[0].transform.position.y);

                for (int i = 0; i < tiles.Count; i++)
                {
                    float distance = Mathf.Abs(tile.transform.position.y - tiles[i].transform.position.y);

                    if (distance < minDistance)
                    {

                        minDistance = distance;
                        indexTile = tiles[i].transform.GetSiblingIndex();
                    }
                }
            }

            tile.transform.SetParent(transform);
            tile.transform.SetSiblingIndex(indexTile);
            tile.SetContainer = this;
            tiles.Add(tile);
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