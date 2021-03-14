using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using CustomNotes;
using CustomNotes.Data;
using CustomNotes.Managers;
using CustomNotes.Overrides;

namespace TwitchFX.Colors {
	
	public class CustomNoteColorizer {
		
		private static ConditionalWeakTable<GameObject, ActiveNoteColorizer> activeNoteColorizerMap = new ConditionalWeakTable<GameObject, ActiveNoteColorizer>();
		
		private readonly CustomNoteController customNote;
		
		private readonly CustomNoteColorNoteVisuals customNoteVisuals;
		private readonly float colorStrength;
		
		public CustomNoteColorizer(GameNoteController note) {
			
			Logger.log.Debug("CustomNoteColorizer created");
			
			customNote = note.gameObject.GetComponent<CustomNoteController>();
			
			customNoteVisuals = Helper.GetValue<CustomNoteColorNoteVisuals>(customNote, "_customNoteColorNoteVisuals");
			colorStrength = Helper.GetValue<CustomNote>(customNote, "_customNote").Descriptor.NoteColorStrength;
			
		}
		
		public void SetColor(Color color) {
			
			customNoteVisuals.SetColor(color, true);
			
			GameObject activeNote = Helper.GetValue<GameObject>(customNote, "activeNote");
			
			if (!activeNoteColorizerMap.TryGetValue(activeNote, out ActiveNoteColorizer activeNoteColorizer))
				activeNoteColorizerMap.Add(activeNote, activeNoteColorizer = new ActiveNoteColorizer(activeNote));
			
			activeNoteColorizer.SetColor(color, colorStrength);
			
		}
		
		private class ActiveNoteColorizer {
			
			private readonly Renderer[] renderers;
			private readonly MaterialPropertyBlockController mpbc;
			
			public ActiveNoteColorizer(GameObject activeNote) {
				
				Logger.log.Debug("ActiveNoteColorizer created");
				
				List<Renderer> rendererList = new List<Renderer>();
				
				foreach (Transform transform in activeNote.GetComponentsInChildren<Transform>()) {
					
					if (transform.GetComponent<DisableNoteColorOnGameobject>())
						continue;
					
					Renderer renderer = transform.GetComponent<Renderer>();
					
					if (renderer)
						rendererList.Add(renderer);
					
				}
				
				renderers = rendererList.ToArray();
				
				mpbc = activeNote.GetComponent<MaterialPropertyBlockController>();
				
			}
			
			public void SetColor(Color color, float colorStrength) {
				
				foreach (Renderer renderer in renderers)
					renderer.material.SetColor("_Color", color * colorStrength);
				
				if (mpbc != null) {
					
					mpbc.materialPropertyBlock.SetColor("_Color", color * colorStrength);
					mpbc.ApplyChanges();
					
				}
				
			}
			
		}
		
	}
	
}