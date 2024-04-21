using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource BGM;
    public float EFC { get =>EF();}

    private void Start()
    {
        VolumeSet();
    }
    public void VolumeSet()
    {
        OptionManager Alfa = OptionManager.i;
        BGM.volume = Alfa._BackGroundMusic * Alfa._MasterSound;
    }

    float EF()
    {
        float v = OptionManager.i._EffectSound * OptionManager.i._MasterSound;
        return v;
    }
}

public static class AudioPlayer
{
	
}