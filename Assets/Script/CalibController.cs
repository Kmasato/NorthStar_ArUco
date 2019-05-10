using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibController : MonoBehaviour {

	private const float PI = 3.14159f;

	[Range(0.00001f,5.0f)]
	public float scale = 1.0f;

	[Range(-10f,10f)]
	public float xAxis = 0.0f;
	[Range(-10f,10f)]
	public float yAxis = 0.0f;
	[Range(-10f,10f)]
	public float zAxis = 0.0f;

	[Range(-180,180)]
	public float  roll= 0.0f;
	[Range(-180,180)]
	public float yaw = 0.0f;
	[Range(-180,180)]
	public float pitch = 0.0f;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
