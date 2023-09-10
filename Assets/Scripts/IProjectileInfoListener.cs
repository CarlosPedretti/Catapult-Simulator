using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileInfoListener
{
    void OnProjectileInfo(float height, float speed, float distance);
}
