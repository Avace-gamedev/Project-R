using System;
using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static Transform CreateChildWithComponents(this Transform parent, string name, params Type[] components)
        {
            return parent.gameObject.CreateChildWithComponents(name, components).transform;
        }

        public static Transform CreateChildWithComponents(this Transform parent, params Type[] components) =>
            parent.CreateChildWithComponents(null, components);

        public static Transform CreateChild(this Transform parent, string name) =>
            parent.CreateChildWithComponents(name);
    }
}