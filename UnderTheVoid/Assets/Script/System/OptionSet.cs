using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSet : MonoBehaviour
{
    [SerializeField, Tooltip("마스터 사운드")]
    Slider _MasterSound;
    [SerializeField, Tooltip("배경음")]
    Slider _BackGroundMusic;
    [SerializeField, Tooltip("효과음")]
    Slider _EffectSound;
    [SerializeField, Tooltip("해상도")]
    TMP_Dropdown _ScreenSize;
    [SerializeField, Tooltip("창 모드")]
    TMP_Dropdown _FullScreenMode;

    [SerializeField, Tooltip("옵션 창")]
    OptionManager om;
    void Start()
    {
        Set();
    }

    void Set()
    {
        if (om == null)
            om = OptionManager.i;
        _MasterSound.value = om._MasterSound;
        _BackGroundMusic.value = om._BackGroundMusic;
        _EffectSound.value = om._EffectSound;
        _ScreenSize.value = om.rtValue(om._ScreenSize);
        _FullScreenMode.value = om.rtfullScreen();
    }
}
