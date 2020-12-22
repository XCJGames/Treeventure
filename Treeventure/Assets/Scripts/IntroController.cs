using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class IntroController : MonoBehaviour
{
    [SerializeField] Button button;
    TextMeshProUGUI buttonText;

    [SerializeField] TextMeshProUGUI exclamation;
    [SerializeField] Image farmer;
    [SerializeField] Sprite altFarmerSprite;
    [SerializeField] Image ghost;

    [SerializeField] AudioClip exclamationSound;
    [SerializeField] AudioClip ghostSound;

    List<string> texts;
    int step;
    // Start is called before the first frame update
    void Start()
    {
        step = 0;
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        texts = GetTexts();
        buttonText.text = texts[0];
    }
    public void NextStep()
    {
        
        switch (step)
        {
            case 0:
                button.gameObject.SetActive(true);
                break;
            case 1:
                AudioSource.PlayClipAtPoint(
                    exclamationSound,
                    Camera.main.transform.position,
                    PlayerPrefsController.GetMasterVolume());
                exclamation.gameObject.SetActive(true);
                ghost.gameObject.SetActive(true);
                button.gameObject.SetActive(false);
                StartCoroutine(Step1SecondPart(1.5f));
                break;
            case 2:
                buttonText.text = texts[step];
                break;
            case 3:
                FindObjectOfType<LevelController>().LoadFirstLevel();
                break;
        }
        Debug.Log(step);
        step++;
    }

    public IEnumerator Step1SecondPart(float x)
    {
        yield return new WaitForSeconds(x);
        AudioSource.PlayClipAtPoint(
                    ghostSound,
                    Camera.main.transform.position,
                    PlayerPrefsController.GetMasterVolume());
        exclamation.gameObject.SetActive(false);
        farmer.sprite = altFarmerSprite;
        buttonText.text = texts[1];
        button.gameObject.SetActive(true);
    }
    private List<string> GetTexts()
    {
        List<string> list = new List<string>();
        list.Add("Este año ha ido muy bien la venta en mi granja de árboles de Navidad, tengo que pensar cómo sacar más beneficio.");
        list.Add("¡¿Quién eres?!");
        list.Add("Soy el fantasma de la ecología, he estado observando cómo utilizas prácticas que hieren al planeta, ¡y eso se va acabar! Voy a estar observándote y si sigues haciendo daño a la naturaleza, ¡te asustaré! Y puede que de alguno de esos sustos no te recuperes.");
        return list;
    }
}
