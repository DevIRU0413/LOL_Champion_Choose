using System.IO;

using UnityEngine;

namespace Scripts.Tool
{
    public static class ImageSaver
    {
        public static void SaveTextureToPNG(Texture2D texture, string folderPath, string fileName)
        {
            string localFolderPath = Path.Combine(Application.persistentDataPath, folderPath);
            string filePath = Path.Combine(localFolderPath, fileName);

            Directory.CreateDirectory(folderPath);

            if (texture == null)
            {
                Debug.LogWarning("❌ 저장 실패: Texture2D가 null이야");
                return;
            }

            byte[] bytes = texture.EncodeToPNG(); // 또는 EncodeToJPG()
            File.WriteAllBytes(filePath, bytes);

            Debug.Log($"✅ 이미지 저장 완료: {filePath}");
        }
    }
}
