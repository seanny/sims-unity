using UnityEngine;

public class CityContent : MonoBehaviour
{
    public static CityContent Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        
    }
}