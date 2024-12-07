using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	[SerializeField] private TankSO m_Data;
	[SerializeField] private Shell m_ShellPrefab;
	[SerializeField] private ShellSO[] m_AmmoTypes;
	[SerializeField] private int[] m_AmmoCounts;
	private int m_SelectedShell;

	private float m_CurrentDispersion;

	//Expand this class as you see fit, it is essentially your weapon
	//spawn the base shell and inject the data from m_AmmoTypes after spawning
	
	public void Init(TankSO inData)
	{
		m_Data = inData;
	}

	public void Fire()
	{

	}

}
