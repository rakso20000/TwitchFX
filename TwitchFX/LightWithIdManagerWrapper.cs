using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchFX {
	
	public class LightWithIdManagerWrapper : LightWithIdManager {
		
		private readonly LightWithIdManager manager;
		
		public LightWithIdManagerWrapper(LightWithIdManager manager) {
			
			this.manager = manager;
			
			//accessible via colors
			_colors = manager.colors;
			
			//accessed by chroma
			_lights = Helper.GetValue<List<LightWithId>[]>(manager, "_lights");
			
			manager.didSetColorForIdEvent += OnDidSetColorForIdEvent;
			
		}
		
		~LightWithIdManagerWrapper() {
			
			manager.didSetColorForIdEvent -= OnDidSetColorForIdEvent;
			
		}
		
		public void SetCustomColorForId(int id, Color color) {
			
			manager.SetColorForId(id, color);
			
			_lastColorChangeFrameNum = manager.lastColorChangeFrameNum;
			
		}
		
		public override void SetColorForId(int id, Color color) {
			
			if (LightController.instance.overrideLights)
				return;
			
			manager.SetColorForId(id, color);
			
			_lastColorChangeFrameNum = manager.lastColorChangeFrameNum;
			
		}
		
		public override Color GetColorForId(int id) {
			
			return manager.GetColorForId(id);
			
		}
		
		public override void RegisterLight(LightWithId light) {
			
			manager.RegisterLight(light);
			
		}
		
		public override void UnregisterLight(LightWithId light) {
			
			manager.UnregisterLight(light);
			
		}
		
		private void OnDidSetColorForIdEvent(int id, Color color) {
			
			//Abiguity between didSetColorForIdEvent and didSetColorForIdEvent
			Action<int, Color> action = Helper.GetValue<LightWithIdManager, Action<int, Color>>(this, "didSetColorForIdEvent");
			action?.Invoke(id, color);
			
		}
		
	}
	
}