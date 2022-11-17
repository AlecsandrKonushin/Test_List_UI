using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class ContainerTiles : MonoBehaviour
    {
        private const int MAX_TILES_IN_LIST = 7;

        [SerializeField] private Text nameText;
        [SerializeField] private Text countElementsText;
        [SerializeField] private GameObject parentTiles;

        private List<Tile> tiles = new List<Tile>();

        private string nameContainer;

        public string SetNameList { set => nameContainer = value; }
        public bool CanAddTile { get => tiles.Count < MAX_TILES_IN_LIST; }
        public List<Tile> GetTiles { get => tiles; }

        public void AddTileInEnd(Tile tile)
        {
            tiles.Add(tile);
            tile.transform.SetParent(parentTiles.transform);
            tile.transform.localScale = new Vector3(1, 1, 1);
            tile.transform.position = new Vector3(0, 0, 0);
            tile.Container = this;

            UpdateViewData();
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

            tile.transform.SetParent(parentTiles.transform);
            tile.transform.SetSiblingIndex(indexTile);
            tile.Container = this;
            tiles.Add(tile);

            UpdateViewData();
        }

        public void RemoveTile(Tile tile)
        {
            if (tiles.Contains(tile))
            {
                tiles.Remove(tile);
            }

            UpdateViewData();
        }

        public void SortByString(bool up)
        {
            if (up)
            {
                tiles = tiles.OrderBy(tile => tile.DescriptionText).ToList();
            }
            else
            {
                tiles = tiles.OrderByDescending(tile => tile.DescriptionText).ToList();
            }

            SiblingTiles();
        }

        public void SortByNumber(bool up)
        {
            if (up)
            {
                tiles = tiles.OrderBy(tile => tile.NumberText).ToList();
            }
            else
            {
                tiles = tiles.OrderByDescending(tile => tile.NumberText).ToList();
            }

            SiblingTiles();
        }

        private void SiblingTiles()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].transform.SetSiblingIndex(i);
            }
        }

        private void UpdateViewData()
        {
            nameText.text = nameContainer;
            countElementsText.text = tiles.Count.ToString();
        }
    }
}