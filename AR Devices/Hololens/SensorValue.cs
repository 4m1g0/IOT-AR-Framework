using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SensorValue
{
    public string Time;
    public string POWER;
    public Energy ENERGY;
}

[System.Serializable]
public class Energy
{
    public float Total;
    public float Yesterday;
    public float Today;
    public float Period;
    public float Power;
    public float Factor;
    public float Voltage;
    public float Current;
}