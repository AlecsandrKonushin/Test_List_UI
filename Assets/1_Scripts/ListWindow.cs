using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class ListWindow : MonoBehaviour
    {
        [SerializeField] private GameObject parentContainers;
        [SerializeField] private GameObject parentDragTile;

        private List<ContainerTiles> containers = new List<ContainerTiles>();

        private Tile fakeTile;
        private int countContainers = 2, countTiles = 5;

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

                    Tile newTile = Creator.Instance.CreateTile();
                    container.AddTileInEnd(newTile);
                    newTile.SetData(this);
                    newTile.DescriptionText = descriptionTile;
                    newTile.NumberText = numberTile.ToString();
                }
            }

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

            needContainer.AddTileInCenter(tile);
        }
    }
}