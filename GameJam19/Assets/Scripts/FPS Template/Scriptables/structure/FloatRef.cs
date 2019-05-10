using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FloatRef {

    public bool isconstant;
    public float theconstant;
    public FloatVar floatvar;

    public float value
    {
        get { return isconstant ? theconstant : floatvar.value; }
        set { if (!isconstant) floatvar.value = value; }
    }
}
