using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCube : MonoBehaviour {

	public LeapUVC _leapUVC;
	public CalibController calib;

	public int markerId = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(_leapUVC.objList.Count > 0){
			var pos = _leapUVC.objList[markerId].position;
			var rot = _leapUVC.objList[markerId].rotation;
			//this.transform.position = new Vector3(pos.x, pos.y,pos.z)*calib.scale;
			this.transform.localRotation = _leapUVC.objList[markerId].rotation;
			this.transform.position = new Vector3(pos.x, pos.y,pos.z);

			this.transform.Rotate(calib.roll,calib.yaw,calib.pitch);
		}
		*/
	}
}
