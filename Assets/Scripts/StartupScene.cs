using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class ResolutionOption
{
    public ResolutionOption(int w, int h, RefreshRate r)
    {
        width = w;
        height = h;
        refresh = r;
    }

    public int width;
    public int height;
    public RefreshRate refresh;
}

public class StartupScene : MonoBehaviour
{
    //public static string BasePath;

    [SerializeField] private TMP_InputField m_BasePathField;
    [SerializeField] private TMP_Dropdown m_LanguageSelector;
    [SerializeField] private TMP_Dropdown m_ScreenSize;
    [SerializeField] private Toggle m_FullscreenSelector;
    [SerializeField] private List<ResolutionOption> m_ResolutionOptions = new List<ResolutionOption>();

    private void Start()
    {
        m_LanguageSelector.AddOptions(AssetManager.LanguageSetNames.ToList());
        m_LanguageSelector.value = 1;

        m_ScreenSize.ClearOptions();
        List<string> resOptions = new List<string>();
        foreach (var resolution in Screen.resolutions)
        {
            resOptions.Add(resolution.ToString());
            m_ResolutionOptions.Add(new ResolutionOption(resolution.width, resolution.height, resolution.refreshRateRatio));
        }
        m_ScreenSize.AddOptions(resOptions);
    }

    public void OnSetScreenSize()
    {
        var resolution = m_ResolutionOptions[m_ScreenSize.value];
        if(resolution == null)
        {
            Debug.LogError($"Cannot set resolution: m_ResolutionOptions[{m_ScreenSize.value}] was null");
        }
        SettingsManager.Instance.SetScreensize(m_FullscreenSelector.isOn, resolution);

    }

    public void OnStartGame()
    {
        string basePath = m_BasePathField.text;
        if(!Directory.Exists(basePath))
        {
            Debug.LogError($"Cannot find base path!");
            return;
        }

        AssetManager.Instance.SelectedLangCode = (LangCode)(m_LanguageSelector.value + 1);
        SettingsManager.Instance.SetBaseDir(basePath);

        AssetManager.Instance.LoadIffFiles(basePath);
        AssetManager.Instance.LoadFarFiles(basePath);

        Debug.Log($"Loaded {BitmapManager.Instance.Bitmaps.Count} bitmaps!");

        FullScreenMode fullScreenMode = FullScreenMode.Windowed;
        if(SettingsManager.Instance.Fullscreen)
        {
            fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        //Screen.SetResolution(SettingsManager.Instance.ScreenSize.width, SettingsManager.Instance.ScreenSize.height, fullScreenMode, SettingsManager.Instance.ScreenSize.refresh);
        SceneManager.LoadSceneAsync(1);
    }
}
