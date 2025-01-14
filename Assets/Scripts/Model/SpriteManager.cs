using SimsLib.IFF;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ESpriteRenderingMode
{
    DrawingGroup,
    Bitmap
};

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance { get; private set; }

    public ESpriteRenderingMode SpriteRenderingMode = ESpriteRenderingMode.DrawingGroup;
    private List<DrawingGroup> m_DrawingGroups = new List<DrawingGroup>();
    private int m_SelectedGroup = 0;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private TMP_Text m_Text;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void FixedUpdate()
    {
        if(m_SpriteRenderer == null)
        {
            return;
        }    

        if(SpriteRenderingMode == ESpriteRenderingMode.DrawingGroup)
        {
            RenderDrawingGroups();
        }
        else
        {
            RenderBitmaps();
        }
    }

    private void RenderBitmaps()
    {
        if (BitmapManager.Instance.Bitmaps == null || BitmapManager.Instance.Bitmaps.Count <= 0)
        {
            return;
        }

        if (m_SelectedGroup > BitmapManager.Instance.Bitmaps.Count)
        {
            m_SelectedGroup = 0;
        }

        if (m_SelectedGroup < 0)
        {
            m_SelectedGroup = BitmapManager.Instance.Bitmaps.Count;
        }

        if (BitmapManager.Instance.Bitmaps.Count > 0 && BitmapManager.Instance.Bitmaps[m_SelectedGroup] != null)
        {
            m_Text.text = $"Bitmap #{m_SelectedGroup}: {BitmapManager.Instance.Bitmaps[m_SelectedGroup].Label}";
            if (BitmapManager.Instance.Bitmaps[m_SelectedGroup].Sprite != null)
            {
                m_SpriteRenderer.sprite = BitmapManager.Instance.Bitmaps[m_SelectedGroup].Sprite;
            }
        }
        else
        {
            m_Text.text = $"Bitmap list is empty or null!";
        }
    }

    private void RenderDrawingGroups()
    {
        if (m_DrawingGroups == null || m_DrawingGroups.Count <= 0)
        {
            return;
        }

        if (m_DrawingGroups[m_SelectedGroup].Images == null || m_DrawingGroups[m_SelectedGroup].Images.Count <= 0)
        {
            return;
        }

        if (m_DrawingGroups[m_SelectedGroup].Images != null && m_DrawingGroups[m_SelectedGroup].Images.Count > 0)
        {
            if (m_DrawingGroups[m_SelectedGroup].Images[0].Sprites != null && m_DrawingGroups[m_SelectedGroup].Images[0].Sprites.Count > 0)
            {
                for (int i = 0; i < m_DrawingGroups[m_SelectedGroup].Images[0].Sprites.Count; i++)
                {
                    m_Text.text = $"Sprite #{m_SelectedGroup}: {m_DrawingGroups[m_SelectedGroup].Label} (frame {i})";
                    if (m_DrawingGroups[m_SelectedGroup].Images[0].Sprites[i] != null)
                    {
                        m_SpriteRenderer.sprite = m_DrawingGroups[m_SelectedGroup].Images[0].Sprites[i];
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (m_SpriteRenderer == null)
        {
            m_SpriteRenderer = GameObject.FindAnyObjectByType<SpriteRenderer>();
        }
        if (m_Text == null)
        {
            m_Text = GameObject.FindAnyObjectByType<TMP_Text>();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            SpriteRenderingMode = ESpriteRenderingMode.DrawingGroup;
            m_SelectedGroup = 0;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            SpriteRenderingMode = ESpriteRenderingMode.Bitmap;
            m_SelectedGroup = 0;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_SelectedGroup++;
            if(m_SelectedGroup > m_DrawingGroups.Count && SpriteRenderingMode == ESpriteRenderingMode.DrawingGroup)
            {
                m_SelectedGroup = 0;
            }
            if (m_SelectedGroup > BitmapManager.Instance.Bitmaps.Count && SpriteRenderingMode == ESpriteRenderingMode.Bitmap)
            {
                m_SelectedGroup = 0;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            m_SelectedGroup--;
            if (m_SelectedGroup < 0 && SpriteRenderingMode == ESpriteRenderingMode.DrawingGroup)
            {
                m_SelectedGroup = m_DrawingGroups.Count;
            }
            if (m_SelectedGroup < 0 && SpriteRenderingMode == ESpriteRenderingMode.Bitmap)
            {
                m_SelectedGroup = BitmapManager.Instance.Bitmaps.Count;
            }
        }
    }

    public void AddDrawingGroup(DGRP dgrp)
    {
        DrawingGroup drawingGroup = new DrawingGroup();
        drawingGroup.Label = dgrp.ChunkLabel;
        foreach(var image in dgrp.Images)
        {
            DrawingGroup.DrawingGroupImage drawingGroupImage = new DrawingGroup.DrawingGroupImage();
            drawingGroupImage.Direction = image.Direction;
            drawingGroupImage.Sprites = new List<Sprite>();
            foreach(var sprite in image.Sprites)
            {
                Debug.Log($"Adding spriteid #{sprite.SpriteID} to drawing group sprites list");
                drawingGroupImage.Sprites.Add(sprite.GetSprite());
            }
            drawingGroup.Images.Add(drawingGroupImage);
        }
        m_DrawingGroups.Add(drawingGroup);
    }
}
