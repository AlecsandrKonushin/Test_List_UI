using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Test.Save
{
    public class LoadSaveController
    {
        private const string PATH_SAVE = "SaveData.json";

        private static SaveData SaveData;

        public static void Save()
        {
            SaveData = new SaveData();
            List<ContainerTiles> containers = ListWindow.Instance.GetContainers;
            SaveData.ContainersSave = new ContainerDataSave[containers.Count];

            for (int i = 0; i < containers.Count; i++)
            {
                ContainerDataSave containerDataSave = new ContainerDataSave();
                List<TileDataSave> tilesDataSave = new List<TileDataSave>();
                containerDataSave.TogglesIsActive = containers[i].ToggleIsActive;

                foreach (var tile in containers[i].GetTiles)
                {
                    TileDataSave tileSave = new TileDataSave();
                    tileSave.Description = tile.DescriptionText;
                    tileSave.Number = tile.NumberText;

                    tilesDataSave.Add(tileSave);
                }

                containerDataSave.Tiles = tilesDataSave;
                SaveData.ContainersSave[i] = containerDataSave;
            }
            
            string jsonString = JsonUtility.ToJson(SaveData);

            try
            {
                File.WriteAllText(Application.persistentDataPath + PATH_SAVE, jsonString);
            }
            catch (Exception ex)
            {
                Debug.Log($"Error save Deck data - {ex}");
            }
        }

        public static SaveData LoadData()
        {
            SaveData = null;

            try
            {
                if (File.Exists(Application.persistentDataPath + PATH_SAVE))
                {
                    string strLoadJson = File.ReadAllText(Application.persistentDataPath + PATH_SAVE);
                    SaveData = JsonUtility.FromJson<SaveData>(strLoadJson);                    
                }
                else
                {
                    Debug.Log($"Not have file save");
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"<color=red>Error load file save</color> - {ex}");
            }

            return SaveData;
        }
    }
}