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

    public Trait(List<Traits> traits)
    {
        Traits t = RandomlyPickTraitAvoidingConflicts(traits);
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
    private Traits RandomlyPickTraitAvoidingConflicts(List<Traits> traits)
    {
        bool ok = false;
        Traits t = 0;
        bool hasConflict = false;
        while (!ok)
        {
            t = (Traits)Random.Range(0, Enum.GetValues(typeof(Trait.Traits)).Length);
            hasConflict = GetConflictOfTrait(t, out Traits conflictTrait);
            if (!traits.Contains(t))
            {
                if(!hasConflict || (hasConflict && !traits.Contains(conflictTrait)))
                {
                    ok = true;
                }
            }
        }
        return t;
    }

    private bool GetConflictOfTrait(Traits t, out Traits conflictTrait)
    {
        bool hasConflict = true;
        conflictTrait = 0;
        switch (t)
        {
            case Traits.butterfingers:
                hasConflict = false;
                break;
            case Traits.conflictive:
                conflictTrait = Traits.teamPlayer;
                break;
            case Traits.cosmopolitan:
                conflictTrait = Traits.shy;
                break;
            case Traits.ecoFriendly:
                conflictTrait = Traits.waster;
                break;
            case Traits.hardWorker:
                conflictTrait = Traits.lazy;
                break;
            case Traits.lazy:
                conflictTrait = Traits.hardWorker;
                break;
            case Traits.optimist:
                conflictTrait = Traits.pessimist;
                break;
            case Traits.pessimist:
                conflictTrait = Traits.optimist;
                break;
            case Traits.scary:
                hasConflict = false;
                break;
            case Traits.shy:
                conflictTrait = Traits.cosmopolitan;
                break;
            case Traits.teamPlayer:
                conflictTrait = Traits.conflictive;
                break;
            case Traits.waster:
                conflictTrait = Traits.ecoFriendly;
                break;
        }

        return hasConflict;
    }

    private void FillParamsFromTrait(Traits t)
    {
        this.trait = t;
        switch (t)
        {
            case Traits.butterfingers:
                this.name = "Manazas";
                this.description = "+20% probabilidad evento accidente laboral";
                this.quality = Quality.bad;
                break;
            case Traits.conflictive:
                this.name = "Conflictivo";
                this.description = "+25% probabilidad evento de conflicto";
                this.quality = Quality.bad;
                break;
            case Traits.cosmopolitan:
                this.name = "Cosmopolita";
                this.description = "+10% evento publicitario";
                this.quality = Quality.normal;
                break;
            case Traits.ecoFriendly:
                this.name = "Ecológico";
                this.description = "+5 eco cada turno";
                this.quality = Quality.good;
                break;
            case Traits.hardWorker:
                this.name = "Trabajador";
                this.description = "-10 coste de actividades, +5 eco al podar/quitar hierbas";
                this.quality = Quality.good;
                break;
            case Traits.lazy:
                this.name = "Vago";
                this.description = "+10 coste de actividades, -5 eco";
                this.quality = Quality.bad;
                break;
            case Traits.optimist:
                this.name = "Optimista";
                this.description = "5 árboles reciben +1 punto";
                this.quality = Quality.good;
                break;
            case Traits.pessimist:
                this.name = "Pesimista";
                this.description = "3 árboles pierden -1 punto";
                this.quality = Quality.normal;
                break;
            case Traits.scary:
                this.name = "Asustadizo";
                this.description = "Con ecología <30, +10 coste y -3 ecología en actividades";
                this.quality = Quality.normal;
                break;
            case Traits.shy:
                this.name = "Tímido";
                this.description = "-15% evento publicitario, +3 ecología en actividades";
                this.quality = Quality.normal;
                break;
            case Traits.teamPlayer:
                this.name = "Buen compañero";
                this.description = "Evita el evento de conflicto";
                this.quality = Quality.good;
                break;
            case Traits.waster:
                this.name = "Derrochador";
                this.description = "-5 eco cada turno";
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
