using UnityEngine;

namespace Test
{
    public class MoveFakeTile : MonoBehaviour
    {
        public GameObject Target;

        private void Update()
        {
            if (Target)
            {
                transform.position = Target.transform.position;
            }
        }
    }
}