using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trait
{
    public enum Quality
    {
        good,
        normal,
        bad
    }

    public enum Traits
    {
        teamPlayer,
        ecoFriendly,
        optimist,
        hardWorker,
        shy,
        scary,
        cosmopolitan,
        pessimist,
        conflictive,
        lazy,
        waster,
        butterfingers
    }

    private string name;
    private string description;
    private Traits trait;
    private Quality quality;

    public Trait()
    {
        Traits t = RandomlyPickTrait();
        FillParamsFromTrait(t);
    }

    public Trait(Traits t)
    {
        trait = t;
        FillParamsFromTrait(t);
    }

    public Trait(string name, string description, Traits trait, Quality quality)
    {
        this.name = name;
        this.description = description;
        this.quality = quality;
        this.trait = trait;
    }
    private Traits RandomlyPickTrait()
    {
        int i = Random.Range(0, Enum.GetNames(typeof(Traits)).Length);
        return (Traits) i;
    }
    private void FillParamsFromTrait(Traits t)
    {
        this.trait = t;
        switch (t)
        {
            case Traits.butterfingers:
                this.name = "Manazas";
                this.description = "Aumenta el riesgo de que se produzcan accidentes en el trabajo";
                this.quality = Quality.bad;
                break;
            case Traits.conflictive:
                this.name = "Conflictivo";
                this.description = "Puede causar problemas a sus compañeros";
                this.quality = Quality.bad;
                break;
            case Traits.cosmopolitan:
                this.name = "Cosmopolita";
                this.description = "No está acostumbrado al campo, pero se le da bien la gente";
                this.quality = Quality.normal;
                break;
            case Traits.ecoFriendly:
                this.name = "Ecológico";
                this.description = "Se preocupa por el medio ambiente";
                this.quality = Quality.good;
                break;
            case Traits.hardWorker:
                this.name = "Trabajador";
                this.description = "Trabaja duro y con eficacia";
                this.quality = Quality.good;
                break;
            case Traits.lazy:
                this.name = "Vago";
                this.description = "Pierde el tiempo con frecuencia y su trabajo no es muy bueno";
                this.quality = Quality.bad;
                break;
            case Traits.optimist:
                this.name = "Optimista";
                this.description = "Siempre ve el lado bueno de todo";
                this.quality = Quality.good;
                break;
            case Traits.pessimist:
                this.name = "Pesimista";
                this.description = "No te lo lleves a una fiesta";
                this.quality = Quality.normal;
                break;
            case Traits.scary:
                this.name = "Asustadizo";
                this.description = "Hasta su sombra le produce horror";
                this.quality = Quality.normal;
                break;
            case Traits.shy:
                this.name = "Tímido";
                this.description = "Prefiere trabajar en el campo a sentarse contigo en el desayuno";
                this.quality = Quality.normal;
                break;
            case Traits.teamPlayer:
                this.name = "Buen compañero";
                this.description = "Comprensivo y conciliador con sus compañeros";
                this.quality = Quality.good;
                break;
            case Traits.waster:
                this.name = "Derrochador";
                this.description = "Le encanta gastar y consumir, y no le importa el medio ambiente";
                this.quality = Quality.bad;
                break;
        }
    }

    public string GetName() => name;

    public string GetDescription() => description;

    public Traits GetTrait() => trait;

    public Quality GetQuality() => quality;

    public override string ToString()
    {
        return name;
    }
}
