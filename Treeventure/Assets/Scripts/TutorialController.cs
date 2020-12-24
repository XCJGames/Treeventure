using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] List<Image> tutorials;

    private int step = 0;
    public void NextTutorial()
    {
        gameObject.SetActive(true);
        tutorials[step].gameObject.SetActive(false);
        step++;
        if(step < tutorials.Count)
        {
            tutorials[step].gameObject.SetActive(true);
            //if (step == 4) gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void SkipTutorial()
    {
        FindObjectOfType<GameSystem>().SkipTutorial();
        gameObject.SetActive(false);
    }
}
