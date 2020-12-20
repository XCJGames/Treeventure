using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class EventController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] List<Button> buttons;

    enum GeneralEvents
    {
        drought = 0,
        racoonAttack = 1,
        taylorNeedsBraces = 2,
        bankCyberattacked = 3,
        houseRepairs = 4,
        deluge = 5,
        fires = 6,
        townParty = 7,
        tornado = 8
    }
    enum PlagueEvents
    {
        insectPlague = 9,
        mantisPlague = 10,
        scarabPlague = 11,
    }
    enum PlayerProvokedEvents
    {
        ads = 12
    }

    enum EmployeeEvents
    {
        employeesClaimPayrise = 13,
        babyIncoming = 14,
        sickEmployee = 15,
        brokenTools = 16,
        employeesConflict = 17
    }

    enum LowResourceEvents
    {
        lowMoneyLoan = 18,
        lowMoneyLottery = 19,
        lowEcoONG = 20,
        lowEcoDog = 21
    }

    private bool lowMoneyEvent = false;
    private bool lowEcoEvent = false;

    private int currentEvent;

    public void NewEvent(int plagueRerolls, int adsEventPercentage, int numEmployees,
        int employeeConflictRisk, int brokenToolsEventRisk, int money, int eco)
    {
        int e = GetRandomEvent(plagueRerolls, adsEventPercentage, numEmployees,
            employeeConflictRisk, brokenToolsEventRisk, money, eco);
        SetEvent(e);
    }
    private void SetEvent(int e)
    {
        currentEvent = e;
        if (e <= 8)
        {
            switch ((GeneralEvents)e)
            {
                case GeneralEvents.drought:
                    SetEventData("¡Oh, no!", 
                        "La ciudad está sufriendo una terrible sequía este mes, ¿qué piensas hacer?",
                        new string[] { "Reutilizar el agua la casa", "Ahorrar agua", "Comprar agua embotellada" });
                    break;
                case GeneralEvents.racoonAttack:
                    SetEventData("¡Recorcholis!",
                        "Unos mapaches han entrado en la finca y están dañando los abetos, ¿qué vas a hacer?",
                        new string[] { "Poner trampas para mapaches", "Llamar a control de plagas", "Capturarlos y soltarlos en libertad" });
                    break;
                case GeneralEvents.taylorNeedsBraces:
                    SetEventData("¡Repampanos!",
                        "Tu hija Taylor necesita aparato y no es barato",
                        new string[] { "Contratar seguro dental, 1.000 €" });
                    break;
                case GeneralEvents.bankCyberattacked:
                    SetEventData("¡Maldicion!",
                        "Tu banco ha sufrido un ataque cibernético y has perdido dinero",
                        new string[] { "Has perdido 1.500 €" });
                    break;
                case GeneralEvents.houseRepairs:
                    SetEventData("¡Que mala suerte!",
                        "Te han salido unas humedades en casa y necesitas arreglarlas",
                        new string[] { "El albañil te cobra 500 €" });
                    break;
                case GeneralEvents.deluge:
                    SetEventData("¡Rayos y centellas!",
                        "Ha caído un gran diluvio y el terreno está empantanado, ¿cómo actúas?",
                        new string[] { "Achicar el agua con máquinas", "Achicar el agua con herramientas manuales", "Reciclar el agua" });
                    break;
                case GeneralEvents.fires:
                    SetEventData("¡La cosa esta que arde!",
                        "Ha sucedido un incendio en tu terreno, ¿cómo vas a actuar?",
                        new string[] { "Usar cubos de agua", "Utilizar el agua de la boca de incendios ", "Reutilizar agua del regadío" });
                    break;
                case GeneralEvents.townParty:
                    SetEventData("¡Malditas fiestas del pueblo!",
                        "Algunos lugareños han entrado en tu finca y han destrozado cosas, ¿qué haces?",
                        new string[] { "Comprar todo de nuevo", "Comprar objetos de segunda mano", "Reciclar los materiales y arreglar los desperfectos" });
                    break;
                case GeneralEvents.tornado:
                    SetEventData("¡Cuanto viento!",
                        "Ha sucedido un tornado y ha dejado unos desperfectos, ¿qué quieres hacer?",
                        new string[] { "Comprar todo de nuevo", "Comprar objetos de segunda mano", "Reciclar los materiales y arreglar los desperfectos" });
                    break;
            }
        }
        else if(e <= 11)
        {
            switch ((PlagueEvents)e)
            {
                case PlagueEvents.insectPlague:
                    SetEventData("¡Que desastre!",
                        "Tus abetos tienen pulgones y están estropeando sus cortezas",
                        new string[] { "Comprar pesticidas químicos", "Comprar pesticidas naturales", "Hacer repelente de insectos" });
                    break;
                case PlagueEvents.mantisPlague:
                    SetEventData("¡Vaya!",
                        "Hay una plaga de mantis que están poniendo huevos en tus abetos, ¿qué vas a hacer?",
                        new string[] { "Comprar pesticidas químicos", "Comprar pesticidas naturales", "Hacer repelente de insectos" });
                    break;
                case PlagueEvents.scarabPlague:
                    SetEventData("¡No puede ser!",
                        "Unos escarabajos han puesto larvas en tus abetos, ¿cómo actúas?",
                        new string[] { "Comprar pesticidas químicos", "Comprar pesticidas naturales", "Hacer repelente de insectos" });
                    break;
            }
        }
        else if(e == 12)
        {
            SetEventData("¡Tu pericia ha surtido efecto!",
                "Otro comercio ha visto tu publicidad y va a invertir en tu negocio.",
                new string[] { "¡Genial! Consigues 2.500 €" });
        }
        else if(e <= 17)
        {
            switch ((EmployeeEvents)e)
            {
                case EmployeeEvents.employeesClaimPayrise:
                    SetEventData("¡Oh, vaya!",
                        "Tus empleados no están de acuerdo con su sueldo y han iniciado una huelga. Vas a tener que subirles el sueldo.",
                        new string[] { "El salario base de tus empleados sube un 10%" });
                    break;
                case EmployeeEvents.babyIncoming:
                    SetEventData("¡Bebe en camino!",
                        "Un empleado se ha dado de baja por paternidad/maternidad y no contarás con sus servicios este mes",
                        new string[] { "¡La cigüeña va a tener trabajo!" });
                    break;
                case EmployeeEvents.sickEmployee:
                    SetEventData("¡Que malito estoy!",
                        "Uno de tus empleados ha cogido la gripe y tardará un mes en recuperarse",
                        new string[] { "Espero que se recupere pronto" });
                    break;
                case EmployeeEvents.brokenTools:
                    SetEventData("¡Que desastre!",
                        "Tu empleado manazas ha roto algunas herramientas que debes reponer",
                        new string[] { "Pierdes 500 € y 5 puntos de Eco" });
                    break;
                case EmployeeEvents.employeesConflict:
                    SetEventData("¡Maldita sea!",
                        "Uno de tus empleados está siendo conflictivo, ¿cómo actúas?",
                        new string[] { "Les das una paga extra para que se lleven mejor", "Despides al empleado conflictivo", "Despides al otro empleado" });
                    break;
            }
        }
        else
        {
            switch ((LowResourceEvents)e)
            {
                case LowResourceEvents.lowMoneyLoan:
                    lowMoneyEvent = true;
                    SetEventData("¡Vaya!",
                        "Estás muy ajustado de dinero, puedes pedir dinero prestado",
                        new string[] { "Consigues 10.000 €" });
                    break;
                case LowResourceEvents.lowMoneyLottery:
                    lowMoneyEvent = true;
                    SetEventData("¡Que suerte!",
                        "Acabas de ganar la lotería",
                        new string[] { "Consigues 10.000 €" });
                    break;
                case LowResourceEvents.lowEcoONG:
                    lowEcoEvent = true;
                    SetEventData("¡No, por favor!",
                        "El fantasma está muy enfadado, puedes donar a una ONG ecológica para saciar su sed de darte sustos",
                        new string[] { "Por solo 500 €, recuperas 20 puntos de Eco" });
                    break;
                case LowResourceEvents.lowEcoDog:
                    lowEcoEvent = true;
                    SetEventData("¡Que miedo!",
                        "El fantasma está muy enfadado, puedes adoptar un perro de la perrera para saciar su sed de darte sustos",
                        new string[] { "¡Buen chico! Recuperas 20 puntos de Eco" });
                    break;
            }
        }
    }

    private void SetEventData(string title, string description, string[] buttons)
    {
        SetTitle(title);
        SetDescription(description);
        List<string> texts = new List<string>(buttons.Length);
        foreach(string text in buttons)
        {
            texts.Add(text);
        }
        SetButtonsTexts(texts);
    }

    private int GetRandomEvent(int plagueRerolls, int adsEventPercentage, int numEmployees,
        int employeeConflictRisk, int brokenToolsEventRisk, int money, int eco)
    {
        Debug.Log("rerolls: " + plagueRerolls + " ads: " + adsEventPercentage + " numEmp: " + numEmployees + " conflict: " +
            employeeConflictRisk + " broken: " + brokenToolsEventRisk + " money: " + money + " eco: " + eco);
        List<int> events = new List<int>();
        if(money <= 6000 && !lowMoneyEvent)
        {
            return Random.Range((int)LowResourceEvents.lowMoneyLoan, (int)LowResourceEvents.lowMoneyLottery + 1);
        }
        else if(eco <= 10 && !lowEcoEvent)
        {
            return Random.Range((int)LowResourceEvents.lowEcoONG, (int)LowResourceEvents.lowEcoDog + 1);
        }
        else if(adsEventPercentage > 0 && Random.Range(1, 101) <= adsEventPercentage)
        {
            return (int)PlayerProvokedEvents.ads;
        }
        else if(numEmployees > 1)
        {
            if(brokenToolsEventRisk > 0 && Random.Range(1, 101) <= brokenToolsEventRisk)
            {
                return (int)EmployeeEvents.brokenTools;
            }
            else if(numEmployees > 2 && employeeConflictRisk > 0 &&
                Random.Range(1, 101) <= employeeConflictRisk)
            {
                return (int)EmployeeEvents.employeesConflict;
            }
            else
            {
                events.Add((int)EmployeeEvents.babyIncoming);
                events.Add((int)EmployeeEvents.sickEmployee);
                events.Add((int)EmployeeEvents.employeesClaimPayrise);
            }
        }
        foreach(int e in Enum.GetValues(typeof(GeneralEvents)))
        {
            events.Add(e);
        }

        foreach (int e in Enum.GetValues(typeof(PlagueEvents)))
        {
            events.Add(e);
        }

        int i = Random.Range(0, events.Count);
        while(plagueRerolls > 0 && events[i] >= (int)PlagueEvents.insectPlague &&
            events[i] <= (int)PlagueEvents.scarabPlague)
        {
            plagueRerolls--;
            i = Random.Range(0, events.Count);
        }

        return events[i];
    }

    public void SetDescription(string description)
    {
        this.description.text = description;
    }
    public void SetTitle(string title)
    {
        this.title.text = title;
    }
    public void SetButtonsTexts(List<string> texts)
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            if(i < texts.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = texts[i];
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowWindow()
    {
        gameObject.SetActive(true);
    }

    public void ChooseOption(int option)
    {
        Debug.Log("option: " + option);
        GameSystem gameSystem = FindObjectOfType<GameSystem>();
        List<KeyValuePair<string, int>> effects = new List<KeyValuePair<string, int>>();
        if (currentEvent <= 8)
        {
            switch ((GeneralEvents)currentEvent)
            {
                case GeneralEvents.drought:
                    switch (option)
                    {
                        case 0:
                            effects.Add(new KeyValuePair<string, int>("eco", 5));
                            break;
                        case 1:
                            effects.Add(new KeyValuePair<string, int>("eco", 2));
                            effects.Add(new KeyValuePair<string, int>("treeScore", -2));
                            break;
                        case 2:
                            effects.Add(new KeyValuePair<string, int>("eco", -5));
                            effects.Add(new KeyValuePair<string, int>("money", -500));
                            break;
                    }
                    break;
                case GeneralEvents.racoonAttack:
                    switch (option)
                    {
                        case 0:
                            effects.Add(new KeyValuePair<string, int>("eco", -10));
                            effects.Add(new KeyValuePair<string, int>("money", -500));
                            break;
                        case 1:
                            effects.Add(new KeyValuePair<string, int>("eco", -5));
                            effects.Add(new KeyValuePair<string, int>("money", -1000));
                            break;
                        case 2:
                            effects.Add(new KeyValuePair<string, int>("eco", 10));
                            effects.Add(new KeyValuePair<string, int>("money", -2000));
                            break;
                    }
                    break;
                case GeneralEvents.taylorNeedsBraces:
                    effects.Add(new KeyValuePair<string, int>("money", -1000));
                    break;
                case GeneralEvents.bankCyberattacked:
                    effects.Add(new KeyValuePair<string, int>("money", -1500));
                    break;
                case GeneralEvents.houseRepairs:
                    effects.Add(new KeyValuePair<string, int>("money", -500));
                    break;
                case GeneralEvents.deluge:
                    switch (option)
                    {
                        case 0:
                            effects.Add(new KeyValuePair<string, int>("eco", -10));
                            effects.Add(new KeyValuePair<string, int>("money", -500));
                            break;
                        case 1:
                            effects.Add(new KeyValuePair<string, int>("eco", -5));
                            effects.Add(new KeyValuePair<string, int>("money", -1000));
                            break;
                        case 2:
                            effects.Add(new KeyValuePair<string, int>("eco", 5));
                            effects.Add(new KeyValuePair<string, int>("money", -1500));
                            break;
                    }
                    break;
                case GeneralEvents.fires:
                    switch (option)
                    {
                        case 0:
                            effects.Add(new KeyValuePair<string, int>("eco", -5));
                            effects.Add(new KeyValuePair<string, int>("money", -1000));
                            break;
                        case 1:
                            effects.Add(new KeyValuePair<string, int>("eco", -10));
                            effects.Add(new KeyValuePair<string, int>("money", -500));
                            break;
                        case 2:
                            effects.Add(new KeyValuePair<string, int>("money", -1500));
                            break;
                    }
                    break;
                case GeneralEvents.townParty:
                case GeneralEvents.tornado:
                    switch (option)
                    {
                        case 0:
                            effects.Add(new KeyValuePair<string, int>("eco", -10));
                            effects.Add(new KeyValuePair<string, int>("money", -1500));
                            break;
                        case 1:
                            effects.Add(new KeyValuePair<string, int>("eco", -5));
                            effects.Add(new KeyValuePair<string, int>("money", -1000));
                            break;
                        case 2:
                            effects.Add(new KeyValuePair<string, int>("eco", 5));
                            effects.Add(new KeyValuePair<string, int>("money", -500));
                            break;
                    }
                    break;
            }
        }
        else if (currentEvent <= 11)
        {
            switch ((PlagueEvents)currentEvent)
            {
                case PlagueEvents.insectPlague:
                case PlagueEvents.mantisPlague:
                case PlagueEvents.scarabPlague:
                    switch (option)
                    {
                        case 0:
                            effects.Add(new KeyValuePair<string, int>("eco", -10));
                            effects.Add(new KeyValuePair<string, int>("money", -500));
                            break;
                        case 1:
                            effects.Add(new KeyValuePair<string, int>("eco", -5));
                            effects.Add(new KeyValuePair<string, int>("money", -1000));
                            break;
                        case 2:
                            effects.Add(new KeyValuePair<string, int>("eco", 5));
                            effects.Add(new KeyValuePair<string, int>("money", -1500));
                            break;
                    }
                    break;
            }
        }
        else if (currentEvent == 12)
        {
            effects.Add(new KeyValuePair<string, int>("money", 2500));
        }
        else if (currentEvent <= 17)
        {
            switch ((EmployeeEvents)currentEvent)
            {
                case EmployeeEvents.employeesClaimPayrise:
                    effects.Add(new KeyValuePair<string, int>("employeesExtraPayment", 10));
                    break;
                case EmployeeEvents.babyIncoming:
                case EmployeeEvents.sickEmployee:
                    effects.Add(new KeyValuePair<string, int>("absentEmployee", 0));
                    break;
                case EmployeeEvents.brokenTools:
                    effects.Add(new KeyValuePair<string, int>("eco", -5));
                    effects.Add(new KeyValuePair<string, int>("money", -500));
                    break;
                case EmployeeEvents.employeesConflict:
                    switch (option)
                    {
                        case 0:
                            effects.Add(new KeyValuePair<string, int>("employeesExtraPayment", 0));
                            break;
                        case 1:
                            effects.Add(new KeyValuePair<string, int>("firedEmployee", 1));
                            break;
                        case 2:
                            effects.Add(new KeyValuePair<string, int>("firedEmployee", 0));
                            break;
                    }
                    break;
            }
        }
        else
        {
            switch ((LowResourceEvents)currentEvent)
            {
                case LowResourceEvents.lowMoneyLoan:
                    lowMoneyEvent = true;
                    effects.Add(new KeyValuePair<string, int>("money", 10000));
                    break;
                case LowResourceEvents.lowMoneyLottery:
                    lowMoneyEvent = true;
                    effects.Add(new KeyValuePair<string, int>("money", 10000));
                    break;
                case LowResourceEvents.lowEcoONG:
                    lowEcoEvent = true;
                    effects.Add(new KeyValuePair<string, int>("money", -500));
                    effects.Add(new KeyValuePair<string, int>("eco", 20));
                    break;
                case LowResourceEvents.lowEcoDog:
                    effects.Add(new KeyValuePair<string, int>("eco", 20));
                    break;
            }
        }
        gameSystem.EventCalculations(effects);
        gameObject.SetActive(false);
    }
}
