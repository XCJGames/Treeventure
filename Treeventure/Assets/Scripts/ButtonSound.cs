using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    public void PlayClip()
    {
        AudioSource.PlayClipAtPoint(
            audioClip,
            Camera.main.transform.position,
            PlayerPrefsController.GetMasterVolume());
    }
}
