using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
<<<<<<< HEAD
using System.Collections.Generic;

public class LoadPhoto : MonoBehaviour
{
    public List<Image> displayImages; // List of Image components on canvases
    public Transform player; // Reference to the player object
=======

public class LoadPhoto : MonoBehaviour
{
    public Image displayImage;
>>>>>>> parent of c4c9ccf (Merge branch 'master' of https://github.com/ASlocum64/AIBAttle)

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenFileExplorer();
        }
<<<<<<< HEAD

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveImage();
        }
=======
>>>>>>> parent of c4c9ccf (Merge branch 'master' of https://github.com/ASlocum64/AIBAttle)
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
<<<<<<< HEAD
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

            // Find the closest canvas to the player and update its image
            Image closestImage = FindClosestImage();
            if (closestImage != null)
            {
                closestImage.sprite = Sprite.Create(readableTexture, new Rect(0, 0, readableTexture.width, readableTexture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                Debug.LogWarning("No canvas found near the player.");
            }
        }
    }

    private Image FindClosestImage()
    {
        float closestDistance = Mathf.Infinity;
        Image closestImage = null;

        foreach (Image image in displayImages)
        {
            if (image == null || image.canvas.renderMode != RenderMode.WorldSpace) continue;

            float distance = Vector3.Distance(player.position, image.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestImage = image;
            }
        }

        return closestImage;
    }

    private void SaveImage()
    {
        if (displayImages.Count > 0 && displayImages[0]?.sprite?.texture != null)
        {
            // Save the first sprite as an example (modify to save a specific one if needed)
            Texture2D texture = displayImages[0].sprite.texture;

            byte[] bytes = texture.EncodeToPNG();
            string savePath = EditorUtility.SaveFilePanelInProject("Save Image", "saved_image", "png", "Please enter a file name to save the image to");

            if (!string.IsNullOrEmpty(savePath))
            {
                File.WriteAllBytes(savePath, bytes);
                Debug.Log("Image saved to: " + savePath);
                AssetDatabase.Refresh();
            }
        }
        else
        {
            Debug.LogWarning("No image loaded to save.");
=======
        var texture = new Texture2D(2, 2);
        if (texture.LoadImage(bytes)) // Load image from bytes
        {
            // Apply texture to the canvas Image
            displayImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
>>>>>>> parent of c4c9ccf (Merge branch 'master' of https://github.com/ASlocum64/AIBAttle)
        }
    }
}
