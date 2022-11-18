using System.Collections.Generic;
using System.Linq;
using Test.Save;
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
        [SerializeField] private GameObject toggles;
        [SerializeField] private Toggle stringToggle, numberToggle;

        private List<Tile> tiles = new List<Tile>();

        private string nameContainer;

        public string SetNameList { set => nameContainer = value; }
        public bool CanAddTile { get => tiles.Count < MAX_TILES_IN_LIST; }
        public List<Tile> GetTiles { get => tiles; }
        public bool ToggleIsActive { get => toggles.activeSelf; set => toggles.SetActive(value); }

        private void Awake()
        {
            stringToggle.onValueChanged.AddListener(delegate { SortByString(stringToggle.isOn); });
            numberToggle.onValueChanged.AddListener(delegate { SortByString(numberToggle.isOn); });
        }

        public void AddTileInStart(Tile tile)
        {
            tiles.Add(tile);
            tile.transform.SetParent(parentTiles.transform);
            tile.transform.localScale = new Vector3(1, 1, 1);
            tile.transform.position = new Vector3(0, 0, 0);
            tile.Container = this;

            SiblingTilesForList();
            UpdateViewData();
        }

        public void AddTileAfterDrop(Tile tile)
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
            EditListBySybling();

            LoadSaveController.Save();
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

            SiblingTilesForList();
            LoadSaveController.Save();
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

            SiblingTilesForList();
            LoadSaveController.Save();
        }

        private void SiblingTilesForList()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].transform.SetSiblingIndex(i);
            }
        }

        private void EditListBySybling()
        {
            Tile[] sortTiles = new Tile[tiles.Count];

            for (int i = 0; i < tiles.Count; i++)
            {
                sortTiles[tiles[i].transform.GetSiblingIndex()] = tiles[i];
            }

            tiles = sortTiles.ToList();
        }

        private void UpdateViewData()
        {
            nameText.text = nameContainer;
            countElementsText.text = tiles.Count.ToString();
        }
    }
}