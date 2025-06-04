using System;
using UnityEngine;

namespace Scripts.Tool
{
    public static class JsonHelper
    {
        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }

        public static T[] FromJson<T>(string json)
        {
            // 배열을 객체처럼 감싸서 JSONUtility가 읽을 수 있도록 만듦
            string newJson = "{\"Items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }
    }
}
