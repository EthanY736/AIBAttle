using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;
using System;  // Add this for IntPtr

public class ImageLoader : MonoBehaviour
{
    public Image displayImage;

#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
#endif

    public void OpenFileExplorer()
    {
#if UNITY_STANDALONE_WIN
        var extensions = "Image files\0*.png;*.jpg;*.jpeg;*.bmp;*.gif\0\0";
        var title = "Select an Image";

        var filePath = OpenFileDialog("Select Image", extensions, title);
        if (!string.IsNullOrEmpty(filePath))
        {
            LoadImage(filePath);
        }
#else
        Debug.Log("File explorer is not supported on this platform.");
#endif
    }

#if UNITY_STANDALONE_WIN
    private string OpenFileDialog(string title, string filter, string defaultFileName)
    {
        var openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = filter.Replace('|', '\0') + '\0';
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.dataPath;
        openFileName.title = title;
        openFileName.defExt = "png";
        openFileName.flags = 0x00000008 | 0x00000004;
        openFileName.owner = GetActiveWindow();

        if (DllTest.GetOpenFileName(openFileName))
        {
            return openFileName.file;
        }

        return string.Empty;
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    private class OpenFileName
    {
        public int structSize;
        public IntPtr owner;
        public IntPtr instance;
        public string filter;
        public string customFilter;
        public int maxCustomFilter;
        public int filterIndex;
        public string file;
        public int maxFile;
        public string fileTitle;
        public int maxFileTitle;
        public string initialDir;
        public string title;
        public int flags;
        public ushort fileOffset;
        public ushort fileExtension;
        public string defExt;
        public IntPtr custData;
        public IntPtr hook;
        public string templateName;
        public IntPtr reserved0;
        public IntPtr reserved1;
        public int flagsEx;
    }

    private static class DllTest
    {
        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    }
#endif

    private void LoadImage(string filePath)
    {
        var bytes = File.ReadAllBytes(filePath);
        var texture = new Texture2D(2, 2);
        if (texture.LoadImage(bytes))
        {
            displayImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
