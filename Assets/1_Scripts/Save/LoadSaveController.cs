using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Test.Save
{
    public class LoadSaveController : MonoBehaviour
    {
        private const string PATH_SAVE = "SaveData.json";

        public SaveData SaveData;

        public void SaveDecksData(List<ContainerTiles> containers)
        {
            SaveData = new SaveData();
            SaveData.ContainersSave = new ContainerDataSave[containers.Count];

            for (int i = 0; i < containers.Count; i++)
            {
                ContainerDataSave containerDataSave = new ContainerDataSave();
                List<TileDataSave> tilesDataSave = new List<TileDataSave>();

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

        private async void LoadDataFromServer()
        {

            // Load data from json

            try
            {
                AllDecksDataJson deckDataJson;

                if (File.Exists(Application.persistentDataPath + PATH_LOCAL_DECK_DATA))
                {
                    string strLoadJson = File.ReadAllText(Application.persistentDataPath + PATH_LOCAL_DECK_DATA);
                    deckDataJson = JsonUtility.FromJson<AllDecksDataJson>(strLoadJson);
                    DeckData deck = null;

                    foreach (var deckJson in deckDataJson.Decks)
                    {
                        deck = new DeckData(deckJson.Id, deckJson.NameDeck, deckJson.IsComplete, deckJson.IsSelected, deckJson.IdPresidentCards, deckJson.IdFightCards);
                        DecksData.Add(deck);
                    }
                }
                else
                {
                    // TODO: Error LogController because init logController after this comit

                    Debug.Log($"Not have file save");
                }

                // Create Fake id presidents 
                for (int i = 1; i < 7; i++)
                {
                    idPresidents.Add(i.ToString());
                }

                // Get data presidents from base
                using (var httpClient = new HttpClient())
                {
                    for (int i = 0; i < idPresidents.Count; i++)
                    {
                        var json = await httpClient.GetStringAsync(PATH_PRESIDENTS + idPresidents[i]);

                        CardPresidentDataSerialize cardData = JsonUtility.FromJson<CardPresidentDataSerialize>(json);
                        CardsPresidentsData.Add(cardData);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error load file save - {ex}");
            }
        }
    }
}