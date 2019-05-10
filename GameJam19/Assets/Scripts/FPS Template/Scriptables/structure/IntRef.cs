using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IntRef {

    public bool useconstant;
    public int constant;
    public IntVariable intvar;
    public int Value
    {
        get { return useconstant ? constant : intvar.Value; }
        set { if(!useconstant) intvar.Value = value; }
    }
}
