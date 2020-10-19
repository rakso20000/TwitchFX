using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchFX.Lights {
	
	public class LightWithIdManagerWrapper : LightWithIdManager {
		
		private readonly LightWithIdManager manager;
		
		private readonly Color offColor;
		
		public LightWithIdManagerWrapper(LightWithIdManager manager) {
			
			this.manager = manager;
			
			offColor = new Color(0f, 0f, 0f, 0f);
			
			//accessible via colors
			_colors = manager.colors;

			//accessed by chroma and ClearLights
			_lights = Helper.GetValue<List<LightWithId>[]>(manager, "_lights");
			
			manager.didSetColorForIdEvent += OnDidSetColorForIdEvent;
			
		}
		
		~LightWithIdManagerWrapper() {
			
			manager.didSetColorForIdEvent -= OnDidSetColorForIdEvent;
			
		}
		
		public void ClearLights() {
			
			for (int i = 0; i < _lights.Length; i++)
				manager.SetColorForId(i, offColor);
			
		}
		
		public void SetCustomColorForId(int id, Color color) {
			
			if (LightController.instance.boostColors)
				color = color.ColorWithAlpha(color.a > 0.5f ? 1 : color.a * 2f);
			
			manager.SetColorForId(id, color);
			
			_lastColorChangeFrameNum = manager.lastColorChangeFrameNum;
			
		}
		
		public override void SetColorForId(int id, Color color) {
			
			if (LightController.instance.mode != LightMode.Default)
				return;
			
			if (LightController.instance.boostColors)
				color = color.ColorWithAlpha(color.a > 0.5f ? 1 : color.a * 2f);
			
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