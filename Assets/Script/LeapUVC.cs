using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using OpenCvSharp.Aruco;
using DetectObj; //検出されたマーカをオブジェクトと紐づけるクラス

public class LeapUVC : MonoBehaviour
{

    /* カメラパラメータ */
    public int WIDTH = 640;
    public int HEIGHT = 480;
    private InputArray cameraMatrix;        //カメラマトリクス
    private InputArray distCoefficients;    //歪み係数
    private int source = 1;                 //カメラのインデックス

    /* カメラの出力データ */
    private VideoCapture cam;
    private Mat cam_frame;
    public Mat[] leftRight_frame;

    /* ARUcoの設定 */
    private const PredefinedDictionaryName dictName = PredefinedDictionaryName.Dict4X4_50;
    private Dictionary ar_dict;
    private DetectorParameters detect_param;

    public GameObject animal;

    /* 検出されたオブジェクト */
    //public List<detectObject> objList = new List<detectObject>();
    private int[] id_dict = { 0, 1, 2, 3 };
    private List<GameObject> animalList = new List<GameObject>();
    //private detectObject Detect

    void Start()
    {
        cam = new VideoCapture(source);
        cam.Set(CaptureProperty.ConvertRgb, 0); //これが重要!!
        cam_frame = new Mat();
        leftRight_frame = new Mat[2];

        setCamParameters(); //カメラパラメータの設定

        ar_dict = CvAruco.GetPredefinedDictionary(dictName);
        detect_param = DetectorParameters.Create();
    }

    void Update()
    {

        //Leapmotionのカメラの映像を取得
        readLeftRightFrame();

        Point2f[][] maker_corners;
        int[] maker_ids;
        Point2f[][] reject_points;

        //ARマーカの検出
        CvAruco.DetectMarkers(leftRight_frame[0], ar_dict, out maker_corners, out maker_ids, detect_param, out reject_points);
        GameObject[] animals = GameObject.FindGameObjectsWithTag("animal");

		//マーカが検出されない場合はすべてのanimalオブジェクトを削除
        if (maker_ids.Length == 0){
            foreach (GameObject animal in animals){
                animalMaster animal_script = animal.GetComponent<animalMaster>();
                animal_script.destoryThis();
                Debug.Log("destroy");
            }
        }
		//マーカが検出されなかったanimalオブジェクトを削除
        else
        {
            foreach (GameObject animal in animals){
                //検出されたIDと一致するゲームオブジェクトが存在しなかったら
                if (DetObj.CheckSameID(animal, maker_ids) == false){
                    animalMaster animal_script = animal.GetComponent<animalMaster>();
                    animal_script.destoryThis();
                }
            }
        }

        if (maker_ids.Length > 0)
        {
            //検出されたマーカ情報の描画
            CvAruco.DrawDetectedMarkers(leftRight_frame[0], maker_corners, maker_ids, new Scalar(0, 255, 0));
            //マーカの姿勢推定
            OutputArray tvec = new Mat(), rvec = new Mat();
            CvAruco.EstimatePoseSingleMarkers(maker_corners, 0.6f, cameraMatrix, distCoefficients, rvec, tvec, null);


            //検出されたIDに対する操作
            for (int i = 0; i < maker_ids.Length; i++){
                //ID辞書と一致するマーカが検出されたら
                if (DetObj.CheckTargetMarker(maker_ids[i], id_dict) == true){
                    
					if (animals != null){
                        //すでにオブジェクトが存在していたら
                        if (DetObj.CheckTargetObject(maker_ids[i], animals) == true){
                            int animal_index = DetObj.GetAnimalIndex(maker_ids[i], animals);
                            animalMaster animal_script = animals[animal_index].GetComponent<animalMaster>();
                            animal_script.moveModel(i, tvec, rvec);
                        }

                        //オブジェクトが存在しない場合
                        else{
                            GameObject newAnimal = Instantiate(animal);
                            animalMaster animal_script = newAnimal.GetComponent<animalMaster>();
                            int animal_index = DetObj.GetAnimalIndex(maker_ids[i], animals);
                            animal_script.setParam(maker_ids[i], i, tvec, rvec);
                        }
                    }

                    else{
                        GameObject newAnimal = Instantiate(animal);
                        animalMaster animal_script = newAnimal.GetComponent<animalMaster>();
                        animal_script.setParam(maker_ids[i], 0, tvec, rvec);
                    }
                }
            }
        }
    }

    void setCamParameters()
    {
        //カメラ行列の設定
        double[] camMatrixData = new double[] { 133.86336, 0.0, 328.58466, 0.0, 133.86336, 242.4675, 0.0, 0.0, 1.0 };
        cameraMatrix = new Mat(3, 3, MatType.CV_64FC1, camMatrixData);

        //歪み係数の設定
        double[] distCoefficientData = new double[] { 9.7910637e-01, 4.4650108e-05, -6.8155047e-03, 1.0700627e-02, 7.8342581e-01 };
        distCoefficients = new Mat(1, 5, MatType.CV_64FC1, distCoefficientData);
    }

    void readLeftRightFrame()
    {
        cam.Read(cam_frame);
        cam_frame = cam_frame.Reshape(1, HEIGHT * WIDTH, 2); // LとRの307200(480x640)x2に分ける
        Mat leftFrame = cam_frame.ColRange(0, 1);   //Lのカメラデータ 1次元行列
        Mat rightFrame = cam_frame.ColRange(1, 2);  //Rのカメラデータ 1次元行列
        leftFrame = leftFrame.T();
        rightFrame = rightFrame.T();
        /*	カメラサイズにreshape 480x640 */
        leftFrame = leftFrame.Reshape(1, HEIGHT, WIDTH);
        rightFrame = rightFrame.Reshape(1, HEIGHT, WIDTH);

        leftRight_frame = new Mat[2] { leftFrame, rightFrame };
    }
}
