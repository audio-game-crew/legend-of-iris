using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Characters : MonoBehaviour {
    public static Characters instance;

    public GameObject Lucy;
    public GameObject Boris;
    public GameObject Iris;
    public GameObject Beorn;

    public Characters()
    {
        instance = this;
    }
}
