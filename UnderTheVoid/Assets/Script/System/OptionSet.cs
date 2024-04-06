using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSet : MonoBehaviour
{
    [SerializeField, Tooltip("������ ����")]
    Slider _MasterSound;
    [SerializeField, Tooltip("�����")]
    Slider _BackGroundMusic;
    [SerializeField, Tooltip("ȿ����")]
    Slider _EffectSound;
    [SerializeField, Tooltip("�ػ�")]
    TMP_Dropdown _ScreenSize;
    [SerializeField, Tooltip("â ���")]
    TMP_Dropdown _FullScreenMode;

    [SerializeField, Tooltip("�ɼ� â")]
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
