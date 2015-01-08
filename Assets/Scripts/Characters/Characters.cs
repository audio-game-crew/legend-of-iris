using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Characters {
    private static LucyController lucy;
    public static LucyController Lucy {
        get {
            if (lucy == null) lucy = GameObject.FindGameObjectWithTag("Lucy").GetComponent<LucyController>();
            return lucy;
        }
    }
}
