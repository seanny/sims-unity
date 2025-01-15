using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbiencePlayer : MonoBehaviour
{
    public static AmbiencePlayer Instance { get; private set; }

    private AudioSource m_AudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlayAmbience(string name, bool loop)
    {
        var pcmFile = AudioManager.Instance.PCMFiles.Where(f => f.Name == name).FirstOrDefault();
        if(pcmFile == null)
        {
            Debug.LogError($"No such loaded PCM with name {name} is loaded!");
            return;
        }
        m_AudioSource.clip = pcmFile.Clip;
        m_AudioSource.loop = loop;
        m_AudioSource.Play();
    }
}