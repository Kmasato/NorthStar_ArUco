using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

namespace DetectObj
{
    public static class DetObj
    {
        ///<summarry>
        ///指定したIDが辞書の中にあるのか判定する関数
        ///</summary>
        public static bool CheckTargetMarker(int search_id, int[] id_dict)
        {
            foreach (int _id in id_dict){
                if (_id == search_id)
                    return true;
            }
            return false;
        }

		///<summarry>
		/// 登録されたマーカが存在しているか調べる関数
		/// True 存在している， False 存在していない
		///</summarry>
		public static bool CheckLiveMarker(int id, GameObject[] animals){
			foreach(GameObject animal in animals){
				animalMaster animal_script = animal.GetComponent<animalMaster>();
				if(id == animal_script.id)
					return true;
				}
			return false;
		}

        ///<summary>
        ///指定したIDを持つオブジェクトが存在しているのか判定する関数
        ///</summary>
        public static bool CheckTargetObject(int search_id, GameObject[] animals)
        {
            foreach (GameObject animal in animals){
                animalMaster animal_script = animal.GetComponent<animalMaster>();
                if (search_id == animal_script.id){
                    return true;
                }
            }
            return false;
        }

		///<summary>
		///すでに存在するオブジェクトと同じIDのマーカが検出されているか判定する関数
		///</summary>
		public static bool CheckSameID(GameObject animal, int[] search_ids){
			foreach(int id in search_ids){
				animalMaster animal_script = animal.GetComponent<animalMaster>();
				if(animal_script.id == id){
					return true;
				}
			}
			return false;
		}

		///<smmary>
		///指定したIDを持つオブジェクトのインデックスを返す
		///</summary>
		public static int GetAnimalIndex(int search_id, GameObject[] animals){
			for(int i=0; i < animals.Length; i++){
				animalMaster animal_script = animals[i].GetComponent<animalMaster>();
				if(search_id == animal_script.id){
					return i;
				}
			}
			return 0;
		}
    }
}