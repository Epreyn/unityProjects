// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
    [ActionTarget(typeof(GameObject), "gameObject", true)]
	[Tooltip("Creates a Game Object, usually using a Prefab.")]
	public class InstancePrefab : FsmStateAction
	{
		[RequiredField]
		[Tooltip("GameObject to create. Usually a Prefab.")]
		public FsmGameObject gameObject;

		[Tooltip("Optional Spawn Point.")]
		public FsmGameObject parentObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the created object.")]
		public FsmGameObject storeObject;

		#if PLAYMAKER_LEGACY_NETWORK
		[Tooltip("Use Network.Instantiate to create a Game Object on all clients in a networked game.")]
		public FsmBool networkInstantiate;

		[Tooltip("Usually 0. The group number allows you to group together network messages which allows you to filter them if so desired.")]
		public FsmInt networkGroup;

		#endif
		public override void Reset()
		{
			gameObject = null;
			parentObject = null;
			storeObject = null;
			#if PLAYMAKER_LEGACY_NETWORK
			networkInstantiate = false;
			networkGroup = 0;
			#endif			
		}

		public override void OnEnter()
		{
			var go = gameObject.Value;
			
			if (go != null)
			{

#if PLAYMAKER_LEGACY_NETWORK && !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)
                GameObject newObject;

				if (!networkInstantiate.Value)
				{
					newObject = (GameObject)Object.Instantiate(go, spawnPosition, Quaternion.Euler(spawnRotation));
				}
				else
				{
					newObject = (GameObject)Network.Instantiate(go, spawnPosition, Quaternion.Euler(spawnRotation), networkGroup.Value);
				}
#else
                //var newObject = (GameObject)Object.Instantiate(go, spawnPosition, Quaternion.Euler(spawnRotation));
                var newObject = (GameObject) Object.Instantiate(go, parentObject.Value.transform);
#endif
                storeObject.Value = newObject;
				
				//newObject.transform.position = spawnPosition;
				//newObject.transform.eulerAngles = spawnRotation;
			}
			
			Finish();
		}

	}
}