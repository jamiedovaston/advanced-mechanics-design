using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TurretData", menuName = "DataObject/Tank/TurretData")]
public class TurretSO : TankBASESO
{
	public float TurretTraverseSpeed;
	public float BarrelTraverseSpeed;
	public float DepressionLimit;
	public float ElevationLimit;
}
