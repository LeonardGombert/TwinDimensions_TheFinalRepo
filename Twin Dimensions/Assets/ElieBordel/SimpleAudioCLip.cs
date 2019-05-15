using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Sound Set", menuName="Audio/Sound Set")]
public class SimpleAudioCLip : SoundSetting
{
    public AudioClip clip;
    [Header("General Parameters")]
    [Range(0,1)]public float volume;
    [Range(0,5)]public float pitch;
    public bool loop;

    public override void Play(AudioSource _source)
    {
        _source.volume = volume;
        _source.pitch = pitch;
        _source.loop = loop;
        _source.clip = clip;

        _source.Play();
    }
}
