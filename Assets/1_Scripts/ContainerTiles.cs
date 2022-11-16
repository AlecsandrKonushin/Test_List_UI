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
            int indexTile = tiles[0].transform.GetSiblingIndex();

            if (tile.transform.position.y < tiles[tiles.Count - 1].transform.position.y)
            {
                indexTile = tiles.Count;
            }
            else
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    float distance = Mathf.Abs((Mathf.Abs)tile.transform.position.y - (Mathf.Abs)tiles[i].transform.position.y);

                    if (distance < minDistance)
                    {
                        Debug.Log($"min = {tiles[i].gameObject.name}");

                        minDistance = distance;
                        indexTile = tiles[i].transform.GetSiblingIndex();
                    }
                }
            }

            Debug.Log($"indexTile = {indexTile}");

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