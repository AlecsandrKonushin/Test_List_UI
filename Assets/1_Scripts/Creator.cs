using UnityEngine;

namespace Test
{
    public class Creator : Singleton<Creator>
    {
        [SerializeField] private ContainerTiles containerPrefab;
        [SerializeField] private Tile tileListPrefab;

        public ContainerTiles CreateContainer()
        {
            return Instantiate(containerPrefab);
        }

        public Tile CreateTileList()
        {
            return Instantiate(tileListPrefab);
        }
    }
}