using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class LoadPhoto : MonoBehaviour
{
    public Image displayImage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenFileExplorer();
        }
    }

    private void OpenFileExplorer()
    {
        string path = EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");
        if (!string.IsNullOrEmpty(path))
        {
            LoadImage(path);
        }
    }

    private void LoadImage(string filePath)
    {
        var bytes = File.ReadAllBytes(filePath);
        var texture = new Texture2D(2, 2);
        if (texture.LoadImage(bytes)) // Load image from bytes
        {
            // Apply texture to the canvas Image
            displayImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
