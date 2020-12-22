using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeStats : MonoBehaviour
{
    [Header("Tree")]
    [SerializeField] Sprite[] treeSprites;
    [SerializeField] Image treeImage;
    /// <summary>
    /// Score of the tree, to add when the game ends. Also, it sets the
    /// sprite of the tree, based on these values:
    /// score < -10: red treeSprites[0]
    /// -10 <= score > 0: yellow treeSprites[1]
    /// 0 <= score < 10: light green treeSprites[2]
    /// score >= 10: dark green treeSprites[3]
    /// </summary>
    [SerializeField] private int score;

    [Header("Seed")]
    [SerializeField] private bool seedEco;
    [SerializeField] private GameObject seedEcoText;
    [SerializeField] private ShowTooltip seed;
    [SerializeField] private const int seedCost = 90;
    [SerializeField] private const int seedEcoCost = 200;

    [Header("Water")]
    [SerializeField] private bool waterEco;
    [SerializeField] private GameObject waterEcoText;
    [SerializeField] private ShowTooltip water;
    [SerializeField] private const int waterCost = 100;
    [SerializeField] private const int waterEcoCost = 250;

    [Header("Fertilizer")]
    [SerializeField] private bool fertilizerEco;
    [SerializeField] private GameObject fertilizerEcoText;
    [SerializeField] private ShowTooltip fertilizer;
    [SerializeField] private const int fertilizerCost = 100;
    [SerializeField] private const int fertilizerEcoCost = 250;


    void Start()
    {
        SeedEco = true;
        WaterEco = true;
        FertilizerEco = true;
        treeImage.sprite = treeSprites[2];
        treeImage.rectTransform.sizeDelta = Vector2.zero;
        Score = 0;
        SetVisibilitySeedEcoText();
        SetVisibilityWaterEcoText();
        SetVisibilityFertilizerEcoText();
    }
    public bool SeedEco { get => seedEco; set => seedEco = value; }
    public bool WaterEco { get => waterEco; set => waterEco = value; }
    public bool FertilizerEco { get => fertilizerEco; set => fertilizerEco = value; }
    public int Score { get => score; set => score = value; }

    public void SetVisibilitySeedEcoText()
    {
        bool visibility = !seedEcoText.activeSelf;
        seedEcoText.SetActive(visibility);
        SeedEco = visibility;
        seed.Text = visibility ? "Semilla ecológica: " + seedEcoCost : "Semilla básica: " + seedCost;
    }

    public void SetVisibilityWaterEcoText()
    {
        bool visibility = !waterEcoText.activeSelf;
        waterEcoText.SetActive(visibility);
        WaterEco = visibility;
        water.Text = visibility ? "Agua ecológica: " + waterEcoCost : "Agua básica: " + waterCost;
    }
    public void SetVisibilityFertilizerEcoText()
    {
        bool visibility = !fertilizerEcoText.activeSelf;
        fertilizerEcoText.SetActive(visibility);
        FertilizerEco = visibility;
        fertilizer.Text = visibility ? "Fertilizante ecológico: " + fertilizerEcoCost : "Fertilizante básico: " + fertilizerCost;
    }

    public void DisableSeedButton()
    {
        Button seedButton = seed.GetComponent<Button>();
        seedButton.interactable = false;
    }

    public void GrowTree()
    {
        treeImage.rectTransform.sizeDelta += new Vector2(10,10);
        if (score < -10)
        {
            treeImage.sprite = treeSprites[0];
        }
        else if(score < 0)
        {
            treeImage.sprite = treeSprites[1];
        }
        else if(score < 10)
        {
            treeImage.sprite = treeSprites[2];
        }
        else
        {
            treeImage.sprite = treeSprites[3];
        }
    }
}
