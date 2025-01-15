using System;
using System.Collections.Generic;
using UnityEngine;
using static BitmapManager;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class PCMFile
    {
        public string Name;
        public AudioClip Clip;

        public PCMFile(string name, AudioClip clip)
        {
            Name = name;
            Clip = clip;
        }
    }

    public static AudioManager Instance { get; private set; }

    public List<PCMFile> PCMFiles => m_PCMFiles;

    [SerializeField] private List<PCMFile> m_PCMFiles = new List<PCMFile>();
    private int m_SelectedAudio = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddXA(string name, byte[] data)
    {
        var xaFile = new XAFile(data);
        AudioClip clip = WavUtility.ToAudioClip(xaFile.DecompressedData, name: name);
        m_PCMFiles.Add(new PCMFile(name, clip));
    }
}