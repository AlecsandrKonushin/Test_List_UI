using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class ListWindow : MonoBehaviour
    {
        [SerializeField] private GameObject parentContainers;
        [SerializeField] private GameObject parentDragTile;

        private List<ContainerTiles> containers = new List<ContainerTiles>();

        private int countContainers = 2, countTiles = 5;

        private Tile dragTile;

        private void Start()
        {
            for (int i = 0; i < countContainers; i++)
            {
                ContainerTiles newContainer = Creator.Instance.CreateContainer();
                containers.Add(newContainer);

                newContainer.transform.SetParent(parentContainers.transform);
                newContainer.transform.localScale = new Vector3(1, 1, 1);
                newContainer.transform.position = new Vector3(0, 0, 0);
            }

            foreach (var container in containers)
            {
                for (int i = 0; i < countTiles; i++)
                {
                    string descriptionTile = "Description";
                    int numberTile = Random.Range(0, 100);

                    Tile newTile = Creator.Instance.CreateTileList();
                    container.AddTile(newTile);
                    newTile.SetData(this, descriptionTile, numberTile);
                }
            }
        }

        public void BeginDragTile(Tile tile)
        {
            dragTile = tile;
            tile.transform.SetParent(parentDragTile.transform);
        }
    }
}