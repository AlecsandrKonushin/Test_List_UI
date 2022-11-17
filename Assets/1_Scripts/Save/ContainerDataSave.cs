using System;
using System.Collections.Generic;

namespace Test.Save
{
    [Serializable]
    public class ContainerDataSave
    {
        public List<TileDataSave> Tiles;
    }

    [Serializable]
    public class TileDataSave
    {
        public string Description;
        public string Number;
    }
}