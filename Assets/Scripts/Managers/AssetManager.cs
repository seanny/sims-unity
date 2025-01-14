using Sims.Far;
using SimsLib.IFF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public enum LangCode : byte
{
    Default = 0,
    EnglishUS = 1,
    EnglishUK = 2,
    French = 3,
    German = 4,
    Italian = 5,
    Spanish = 6,
    Dutch = 7,
    Danish = 8,
    Swedish = 9,
    Norwegian = 10,
    Finish = 11,
    Hebrew = 12,
    Russian = 13,
    Portuguese = 14,
    Japanese = 15,
    Polish = 16,
    SimplifiedChinese = 17,
    TraditionalChinese = 18,
    Thai = 19,
    Korean = 20,
}

[Serializable]
public class LoadedPalt
{
    [SerializeField] private List<Color> m_Colours;
    public List<Color> Colours => m_Colours;

    public void AddColours(Color[] colours)
    {
        m_Colours = colours.ToList();
    }
}

public class AssetManager : MonoBehaviour
{
    public static string[] LanguageSetNames =
    {
        "English (US)",
        "English (UK)",
        "French",
        "German",
        "Italian",
        "Spanish",
        "Dutch",
        "Danish",
        "Swedish",
        "Norwegian",
        "Finish",
        "Hebrew",
        "Russian",
        "Portuguese",
        "Japanese",
        "Polish",
        "Simplified Chinese",
        "Traditional Chinese",
        "Thai",
        "Korean",
        "Slovak"
    };

    public static AssetManager Instance { get; private set; }
    public string BasePath { get; set; }

    public LangCode SelectedLangCode = LangCode.Default;
    private List<Iff> m_LoadedIffFiles = new List<Iff>();
    private List<Far> m_LoadedFarFiles = new List<Far>();

    [SerializeField] private List<string> m_LoadedStrings = new List<string>();
    [SerializeField] private List<string> m_LoadedCatalogStrings = new List<string>();
    [SerializeField] private List<LoadedPalt> m_LoadedPALTs = new List<LoadedPalt>();
    [SerializeField] private List<OBJD> m_LoadedOBJDs = new List<OBJD>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void LoadIffFiles(string basePath)
    {
        string gameData = Path.Combine(basePath, "GameData");
        foreach (string file in Directory.EnumerateFiles(gameData, "*.iff", SearchOption.AllDirectories))
        {
            Debug.Log($"Loading IFF {file}...");
            Iff iff = new Iff(file);
            LoadIff(iff);
            //m_LoadedIffFiles.Add(iff);
        }
    }

    private void LoadIff(Iff iff)
    {
        if (iff == null)
        {
            return;
        }

        if (iff.List<FWAV>() != null)
        {
            foreach (var chunk in iff.List<FWAV>())
            {
                Debug.Log($"-> Adding FWAV: #{chunk.ChunkID} Label: {chunk.ChunkLabel}");
                SpriteManager.Instance.AddDrawingGroup(chunk);
            }
        }
        if (iff.List<BMP>() != null)
        {
            foreach (var chunk in iff.List<BMP>())
            {
                Debug.Log($"-> Adding BMP: #{chunk.ChunkID} Label: {chunk.ChunkLabel}");
                BitmapManager.Instance.AddBitmap(chunk.ChunkLabel, chunk.Data);
            }
        }
        if (iff.List<DGRP>() != null)
        {
            foreach (var chunk in iff.List<DGRP>())
            {
                Debug.Log($"-> Adding DGRP: #{chunk.ChunkID} Label: {chunk.ChunkLabel}");
                SpriteManager.Instance.AddDrawingGroup(chunk);
            }
        }
    }

    public void LoadFarFiles(string basePath)
    {
        string gameData = Path.Combine(basePath, "GameData");
        foreach (string file in Directory.EnumerateFiles(gameData, "*.far", SearchOption.AllDirectories))
        {
            Debug.Log($"Loading FAR {file}...");
            Far far = new Far(file);
            foreach (var entry in far.Manifest.ManifestEntries)
            {
                string ext = Path.GetExtension(entry.Filename).ToLower();
                Debug.Log($"-> [{file}] {entry.Filename} (Size: {entry.FileLength1})");
                if (ext == ".iff")
                {
                    byte[] bytes = far.GetBytes(entry);
                    Iff iff = new Iff(bytes);
                    LoadIff(iff);
                }
                else if (ext == ".bmp")
                {
                    byte[] bytes = far.GetBytes(entry);
                    BitmapManager.Instance.AddBitmap(entry.Filename, bytes);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        m_LoadedIffFiles.Clear();
    }
}