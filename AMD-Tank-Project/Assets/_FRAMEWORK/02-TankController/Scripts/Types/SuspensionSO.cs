using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SuspensionData", menuName = "DataObject/Tank/SuspensionData")]
public class SuspensionSO : TankBASESO
{
	public float WheelDiameter;
	public float SuspensionDamper;
	public float SuspensionStrength;
	public LayerMask SuspensionLayermask;
	public float MaximumSlope;
	public float HullTraverseDegrees;
	public float FrictionCoefficient;
	public float WheelRotationSpeed;
}
