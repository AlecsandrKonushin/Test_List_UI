using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class ListWindow : MonoBehaviour
    {
        [SerializeField] private GameObject parentContainers;

        private List<ContainerTiles> containers = new List<ContainerTiles>();

        private int countContainers = 2, countTiles = 5;

        private void Start()
        {
            for (int i = 0; i < countContainers; i++)
            {
                ContainerTiles newContainer = Creator.Instance.CreateContainer();
                containers.Add(newContainer);
                newContainer.transform.SetParent(parentContainers.transform);
            }            

            foreach (var container in containers)
            {
                for (int i = 0; i < countTiles; i++)
                {
                    Tile newTile = Creator.Instance.CreateTileList();
                    container.AddTile(newTile);
                }
            }            
        }
    }
}