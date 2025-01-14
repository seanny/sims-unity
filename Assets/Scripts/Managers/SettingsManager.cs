using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public bool Fullscreen;
    public ResolutionOption ScreenSize;
    public LangCode SelectedLanguage;
    public string BaseDir;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SetBaseDir(string baseDir)
    {
        BaseDir = baseDir;
    }

    public void SetScreensize(bool fullscreen, ResolutionOption resolution)
    {
        ScreenSize = resolution;
        Fullscreen = fullscreen;
    }
}
