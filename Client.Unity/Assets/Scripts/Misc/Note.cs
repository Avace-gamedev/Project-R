using UnityEngine;

namespace Misc
{
    public class Note : CustomMonoBehaviour
    {
        [Header("This component doesn't do anything, it is used to provide comments.")] [TextArea(2, 20)]
        public string note;
    }
}