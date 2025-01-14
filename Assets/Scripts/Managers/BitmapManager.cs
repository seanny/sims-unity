using B83.Image.BMP;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BitmapManager : MonoBehaviour
{
    [Serializable]
    public class Bitmap
    {
        public string Label { get; private set; }
        public Sprite Sprite { get; private set; }

        public Bitmap(string label, Sprite sprite)
        {
            Label = label;
            Sprite = sprite;
        }
    }

    public static BitmapManager Instance { get; private set; }

    [SerializeField] private List<Bitmap> m_Bitmaps = new List<Bitmap>();

    public List<Bitmap> Bitmaps => m_Bitmaps;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddBitmap(string label, byte[] bytes)
    {
        Debug.Log($"Loading bitmap file with length {bytes.Length} bytes");
        BMPLoader bmpLoader = new BMPLoader();
        var bmpImage = bmpLoader.LoadBMP(bytes);
        Texture2D texture2D = bmpImage.ToTexture2D();

        var sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
        m_Bitmaps.Add(new Bitmap(label, sprite));
    }
}