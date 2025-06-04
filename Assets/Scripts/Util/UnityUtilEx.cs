using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Util
{
    public static class UnityUtilEx
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (comp == null)
                comp = go.AddComponent<T>();
            return comp;
        }

        public static GameObject FindOrCreateGameObject(string name) 
        {
            GameObject go = GameObject.Find(name);
            if (go == null)
                go = new GameObject(name);
            return go;
        }
    }
}
