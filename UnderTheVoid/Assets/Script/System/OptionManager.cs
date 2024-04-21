using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    Bgm,
    Effect,
    MaxCount
}

public class OptionManager : MonoBehaviour
{
    public static OptionManager i;

    [SerializeField,Tooltip("마스터 사운드")]
    public float _MasterSound =0;
    [SerializeField, Tooltip("배경음")]
    public float _BackGroundMusic = 0;
    [SerializeField, Tooltip("효과음")]
    public float _EffectSound = 0;

    float _mastersound;
    float _background;
    float _effect;

    [SerializeField, Tooltip("해상도")]
    public Vector2Int _ScreenSize;
    [SerializeField]
    public bool _FullScreen = false;
    private void Awake()
    {
        if (i == null)
            i = this;

         OptionData data = SaveData.OptionLoad(Application.streamingAssetsPath + "/Option.bin");
         if(data != null )
         Setvalue(data);

        Screen.SetResolution(_ScreenSize.x, _ScreenSize.y, _FullScreen);
    }

    public void ScreenSizeSet()
    {
        Screen.SetResolution(_ScreenSize.x, _ScreenSize.y, _FullScreen);
        _MasterSound = _mastersound;
        _BackGroundMusic = _background;
        _EffectSound = _effect;
        SaveData.Save(new OptionData(_MasterSound,_BackGroundMusic,_EffectSound,_ScreenSize,_FullScreen), Application.streamingAssetsPath + "/Option.bin");
    }

    void Setvalue(OptionData data)
    {
        _MasterSound = data._MasterSound;
        _BackGroundMusic= data._BackGroundMusic;
        _EffectSound = data._EffectSound;
        _ScreenSize = new Vector2Int(data._ScreenSizeX, data._ScreenSizeY);  
        _FullScreen= data._FullScreen;
    }

    public void Cancle()
    {
       _ScreenSize =new Vector2Int(Screen.width, Screen.height);
       _FullScreen = Screen.fullScreen;
    }

    public void MasterSoundSet(float a)
    {
       _mastersound = a;
    }
    public void BackGroundMusic(float a)
    {
        _background = a;
    }
    public void EffectSound(float a)
    {
        _effect = a;
    }
    public void ScreenSizeSet(int a)
    {
        switch (a)
        {
            case 0:
                _ScreenSize = new Vector2Int(960, 540);
                break;
            case 1:
                _ScreenSize = new Vector2Int(1280, 720);
                break;
            case 2:
                _ScreenSize = new Vector2Int(1600, 900);
                break;
            case 3:
                _ScreenSize = new Vector2Int(1920, 1080);
                break;
            case 4:
                _ScreenSize =new Vector2Int(2560, 1440);
                break;
            case 5:
                _ScreenSize =new Vector2Int(3840, 2160);
                break;
            default:
                _ScreenSize =new Vector2Int(1920, 1080);
                break;
        }
    }
    public int rtValue(Vector2Int v2)
    {
        switch (v2.x)
        {
            case 960:
                return 0;
            case 1280:
                return 1;
            case 1600:
                return 2;
            case 1920:
                return 3;
            case 2560:
                return 4;
            case 3840:
                return 5;
            default:
                return 3;
        }

    }


    public void FullSCreen(bool FullScreen)
    {
        _FullScreen= FullScreen;    
    }
    public void FullSCreen(int a)
    {
        switch (a)
        {
            case 0:
                _FullScreen = false;
                break;
            case 1:
                _FullScreen = true;
                break;
            default:
                _FullScreen = false;
                break;
        }
    }
    public int rtfullScreen()
    {
        if (_FullScreen)
            return 1;
        else
            return 0;
    }
}
