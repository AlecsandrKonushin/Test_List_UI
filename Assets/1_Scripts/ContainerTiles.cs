using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class ContainerTiles : MonoBehaviour
    {
        private List<Tile> tiles = new List<Tile>();

        public void AddTile(Tile tile)
        {
            tiles.Add(tile);
            tile.transform.SetParent(transform);
            tile.transform.localScale = new Vector3(1, 1, 1);
            tile.transform.position = new Vector3(0, 0, 0);
        }
    }
}