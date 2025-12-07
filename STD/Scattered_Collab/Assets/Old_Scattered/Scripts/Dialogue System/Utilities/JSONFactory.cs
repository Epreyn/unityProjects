using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using LitJson;

namespace JSONFactory{
	class JSONAssembly {
		
		private static Dictionary<int, string> _resourceList = new Dictionary<int, string> {
			{1, "/Resources/EventsJSON/Event1.json"},
			{2, "/Resources/EventsJSON/Event2.json"}
		};

		public static NarrativeEvent RunJSONFactoryForScene(int sceneNumber) {
			string resourcePath = PathForScene(sceneNumber);

			if (IsValidJSON(resourcePath) == true){
				string jsonString = File.ReadAllText (Application.dataPath + resourcePath);
				NarrativeEvent narrativeEvent = JsonMapper.ToObject<NarrativeEvent>(jsonString);

				return narrativeEvent;
			}
			else throw new Exception("JSON not valid");
		}

		private static string PathForScene(int sceneNumber){
			string resourcePathResult;

			if (_resourceList.TryGetValue(sceneNumber, out resourcePathResult)){
				return _resourceList[sceneNumber];
			}
			else throw new Exception("Error JSONFactory");
		}

		private static bool IsValidJSON(string path){
			return (Path.GetExtension(path) == ".json") ? true : false;
		}
	}
}