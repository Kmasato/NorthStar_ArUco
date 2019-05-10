using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DetectObj;
using OpenCvSharp;


public class animalMaster : MonoBehaviour
{

    public int id = 0;
    private GameObject caliCon;
    private CalibController calib;

    void Start()
    {
        caliCon = GameObject.Find("CalibController");
        calib = caliCon.GetComponent<CalibController>();
    }

    void Update()
    {
    }

    public void setParam(int _id, int index, OutputArray tvec, OutputArray rvec)
    {
        id = _id;
        setName(id);
        setModel(id);
        if (calib != null)
        {
            moveModel(index,tvec, rvec);
        }
    }

    void setName(int _id)
    {
        if (_id == 0)
            this.gameObject.name = "CalibCube";
        else if (_id == 1)
            this.gameObject.name = "Turtle";
        else if (_id == 2)
            this.gameObject.name = "Fish";
        else if (_id == 3)
            this.gameObject.name = "Squ";
        else if (_id == 4)
            this.gameObject.name = "Bird2";
        else
            this.gameObject.name = "NoName";
    }

    void setModel(int _id)
    {
        int count = 0;
        foreach (Transform item in this.transform)
        {
            if (count == _id)
                item.gameObject.SetActive(true);
            else
                item.gameObject.SetActive(false);
            count++;
        }
    }

    public void moveModel(int col,OutputArray tvec, OutputArray rvec)
    {
        if (calib != null)
        {
            Mat m1 = tvec.GetMat();
            Mat m2 = rvec.GetMat();
            Vector3 mytvec = new Vector3();
            Vector3 myrvec = new Vector3();
            
			/* for (int i = 0; i < 3; i++)
            {
                mytvec[i] = (float)(m1.At<Vec3d>(0,index)[i]);
                myrvec[i] = (float)(m2.At<Vec3d>(0,index)[i]);
            }*/

			var m11 = m1.At<Vec3d>(0,col);
			var m22 = m2.At<Vec3d>(0,col);

//			Debug.Log(m1.Size());

			mytvec = new Vector3((float)m11[0], (float)m11[1], (float)m11[2]);
			myrvec = new Vector3((float)m22[0], (float)m22[1], (float)m22[2]);



            this.transform.position = new Vector3(mytvec.x + calib.xAxis, -mytvec.y + calib.yAxis, mytvec.z + calib.zAxis);
            var rod = new Vector3(-myrvec.x, myrvec.y, -myrvec.z);
            this.transform.localRotation = Quaternion.AngleAxis(rod.magnitude * 180 / Mathf.PI, rod);
            this.transform.Rotate(calib.roll, calib.yaw, calib.pitch);
        }
    }

	public void destoryThis(){
		Destroy(this.gameObject);
	}
}
