using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtility
{
    public static Dictionary<FitnessTypes, string> fitnessToString = new Dictionary<FitnessTypes, string>()
    {
        {FitnessTypes.Speed, "Velocidade"},
        {FitnessTypes.Distance, "Distância Percorrida"},
        {FitnessTypes.Smarter, "Inteligência"},
        {FitnessTypes.SpeedSmarter, "Inteligência + Velocidade"},
        {FitnessTypes.Defense, "Defesa"},
        {FitnessTypes.DefenseSmarter, "Inteligência + Defesa"},
    };
    public static Dictionary<string, FitnessTypes> stringToFitness = new Dictionary<string, FitnessTypes>()
    {
        {"Velocidade", FitnessTypes.Speed},
        {"Distância Percorrida", FitnessTypes.Distance},
        {"Inteligência", FitnessTypes.Smarter},
        {"Inteligência + Velocidade", FitnessTypes.SpeedSmarter},
        {"Defesa", FitnessTypes.Defense},
        {"Inteligência + Defesa", FitnessTypes.DefenseSmarter},
    };

    public static string FitnessToString(FitnessTypes pType)
    {
        return fitnessToString[pType];
    }
    
    public static FitnessTypes StringToFitness(string stringVal)
    {
        return stringToFitness[stringVal];
    }
}
