using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityHelper
{
    public enum ResourceType
    {
        Stone = 0,
        Copper,
        Iron,
        Gold,
        Diamond,

        Soil,
        Grass,
        Snow
    }

    public struct SeedInfoDEC
    {
        public Vector2Int _centerPos;
        public Color _resourceColor;

        public SeedInfoDEC(Vector2Int pos,Color res)
        {
            _centerPos = pos;
            _resourceColor = res;
        }
    }
}