using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class HideInClassAttribute : PropertyAttribute
{
    public Type classHidden;
    public HideInClassAttribute(Type classHidden)
    {
        this.classHidden = classHidden;
    }
}
