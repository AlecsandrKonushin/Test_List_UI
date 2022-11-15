using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Text descriptionText, numberText;

        public void SetData(string description, int number)
        {
            descriptionText.text = description;
            numberText.text = number.ToString();
        }
    }
}