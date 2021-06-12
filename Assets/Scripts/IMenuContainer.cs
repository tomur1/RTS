using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuContainer
{
    public Dictionary<int ,ButtonSpec> GetButtonLayout();
}
