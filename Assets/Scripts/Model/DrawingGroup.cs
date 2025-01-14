using System;
using System.Collections.Generic;
using UnityEngine;

public enum EImageDirection : uint
{
    LeftFront = 0x10,
    LeftBack = 0x40,
    RightFront = 0x04,
    RightBack = 0x01
};

[Serializable]
public class DrawingGroup
{
    [Serializable]
    public class DrawingGroupImage
    {
        public uint Direction;
        public EImageDirection ImageDirection;
        public uint Zoom;

        public List<Sprite> Sprites = new List<Sprite>();
    }

    public string Label;
    public List<DrawingGroupImage> Images = new List<DrawingGroupImage>();
}

