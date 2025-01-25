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

        // Add save functionality with the 'S' key
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveImage();
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
        var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (texture.LoadImage(bytes)) // Load image from bytes
        {
            texture.Apply();
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            // Make texture readable
            var readableTexture = new Texture2D(texture.width, texture.height, texture.format, false);
            readableTexture.SetPixels(texture.GetPixels());
            readableTexture.Apply();

            // Apply texture to the canvas Image
            displayImage.sprite = Sprite.Create(readableTexture, new Rect(0, 0, readableTexture.width, readableTexture.height), new Vector2(0.5f, 0.5f));
        }
    }

    private void SaveImage()
    {
        if (displayImage.sprite != null && displayImage.sprite.texture != null)
        {
            // Get the current texture from the Image component
            Texture2D texture = displayImage.sprite.texture;

            // Encode the texture to PNG
            byte[] bytes = texture.EncodeToPNG();

            // Choose where to save the image
            string savePath = EditorUtility.SaveFilePanelInProject("Save Image", "saved_image", "png", "Please enter a file name to save the image to");

            if (!string.IsNullOrEmpty(savePath))
            {
                // Write the bytes to the specified file path
                File.WriteAllBytes(savePath, bytes);
                Debug.Log("Image saved to: " + savePath);
            }
        }
        else
        {
            Debug.LogWarning("No image loaded to save.");
        }
    }
}
