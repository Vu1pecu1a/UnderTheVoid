using System;

namespace DefineEnum
{
    /// <summary>
    /// UI е╦ют
    /// </summary>
    public enum UIType
    {
        CharacterInfoWnd,
        InventoryWnd
    }

    public enum SceneType
    {
        Title
    }

    public struct CharInfo
    {
        public string _charName;
        public int _avatarID;
        public int _atk;
        public int _def;

        public CharInfo(string name,int avatar,int att,int def)
        {
            _charName = name;
            _avatarID = avatar;
            _atk = att;
            _def = def;
        }
    }
}