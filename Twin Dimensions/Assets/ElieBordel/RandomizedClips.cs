using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Randomized clips", menuName="Audio/Randomized Set")]
public class RandomizedClips : SoundSetting
{
    public AudioClip[] clips;
    [Header("General Parameters")]
    [Range(0,1)]public float volume;
    [Range(0,5)]public float pitch;
    [Range(0,1)]public float pitchRange;
    public bool loop;

    public override void Play(AudioSource _source)
    {
        _source.volume = volume;
        _source.pitch = RandomPitch();
        _source.loop = loop;
        _source.clip = RandomClip();

        _source.Play();
    }

    AudioClip RandomClip()
    {
        return clips[Random.Range(0,clips.Length)];
    }

    float RandomPitch()
    {
        return pitch + Random.Range(pitch-pitchRange, pitch+pitchRange);
    }
}