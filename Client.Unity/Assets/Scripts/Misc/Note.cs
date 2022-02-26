using UnityEngine;

namespace Misc
{
    public class Note : MonoBehaviour
    {
        [Header("This component doesn't do anything, it is used to provide comments.")] [TextArea(2, 20)]
        public string note;
    }
}