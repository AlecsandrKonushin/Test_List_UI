using System.Collections.Generic;
using Test.Save;
using UnityEngine;

namespace Test
{
    public class ListWindow : Singleton<ListWindow>
    {
        [SerializeField] private GameObject parentContainers;
        [SerializeField] private GameObject parentDragTile;

        private List<ContainerTiles> containers = new List<ContainerTiles>();
        private ContainerTiles sortContainer;

        private Tile fakeTile;
        private int deafultCountContainers = 2, defaultCountTiles = 5;

        public List<ContainerTiles> GetContainers { get => containers; }

        private void Start()
        {
            SaveData saveData = LoadSaveController.LoadData();

            if (saveData != null)
            {
                for (int i = 0; i < saveData.ContainersSave.Length; i++)
                {
                    CreateContainerTiles(i, saveData.ContainersSave[i].TogglesIsActive);
                }
            }
            else
            {
                for (int i = 0; i < deafultCountContainers; i++)
                {
                    CreateContainerTiles(i, i == 0);
                }
            }

            for (int i = 0; i < containers.Count; i++)
            {
                if (saveData != null)
                {
                    defaultCountTiles = saveData.ContainersSave[i].Tiles.Count;
                }

                for (int t = 0; t < defaultCountTiles; t++)
                {
                    string numberTile = Random.Range(0, 100).ToString();
                    string descriptionTile = "Description_" + numberTile;

                    if (saveData != null)
                    {
                        numberTile = saveData.ContainersSave[i].Tiles[t].Number;
                        descriptionTile = saveData.ContainersSave[i].Tiles[t].Description;
                    }

                    CreateTile(containers[i], numberTile, descriptionTile);
                }
            }

            sortContainer = containers[0];

            CreateFakeTile();

            LoadSaveController.Save();
        }

        private void CreateContainerTiles(int index, bool toggleIsActive)
        {
            ContainerTiles newContainer = Creator.Instance.CreateContainer();
            containers.Add(newContainer);

            newContainer.transform.SetParent(parentContainers.transform);
            newContainer.transform.localScale = new Vector3(1, 1, 1);
            newContainer.transform.position = new Vector3(0, 0, 0);
            newContainer.SetNameList = "Container_" + index;
            newContainer.ToggleIsActive = toggleIsActive;
        }

        private void CreateTile(ContainerTiles container, string numberTile, string descriptionTile)
        {
            Tile newTile = Creator.Instance.CreateTile();
            container.AddTileInStart(newTile);
            newTile.DescriptionText = descriptionTile;
            newTile.NumberText = numberTile.ToString();
        }

        private void CreateFakeTile()
        {
            fakeTile = Creator.Instance.CreateTile();
            fakeTile.transform.SetParent(parentDragTile.transform);
            fakeTile.transform.localScale = new Vector3(1, 1, 1);
            fakeTile.gameObject.SetActive(false);
            fakeTile.gameObject.AddComponent<MoveFakeTile>();
        }

        public void BeginDragTile(Tile tile)
        {
            fakeTile.GetComponent<MoveFakeTile>().Target = tile.gameObject;
            fakeTile.DescriptionText = tile.DescriptionText;
            fakeTile.NumberText = tile.NumberText;
            fakeTile.transform.position = tile.transform.position;
            fakeTile.gameObject.SetActive(true);
        }

        public void EndDragTile(Tile tile)
        {
            fakeTile.gameObject.SetActive(false);

            float minDistance = Mathf.Abs(tile.transform.position.x - containers[0].transform.position.x);
            ContainerTiles needContainer = containers[0];

            foreach (var container in containers)
            {
                float distance = Mathf.Abs(tile.transform.position.x - container.transform.position.x);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    needContainer = container;
                }
            }

            if (needContainer.CanAddTile)
            {
                needContainer.AddTileAfterDrop(tile);
            }
            else
            {
                tile.Container.AddTileAfterDrop(tile);
            }
        }
    }
}