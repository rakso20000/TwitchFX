﻿using IPA.Loader;
using System;
using System.Reflection;
using UnityEngine;
using Xft;

namespace TwitchFX {
	
	public class ColorController : MonoBehaviour {
		
		public static ColorController instance { get; private set; }
		
		private static readonly int colorID = Shader.PropertyToID("_Color");
		
		public bool useCustomSaberColors { get; private set; } = false;
		public Color saberColorLeft { get; private set;}
		public Color saberColorRight { get; private set;}
		
		public bool useCustomNoteColors { get; private set; } = false;
		public Color noteColorLeft { get; private set; }
		public Color noteColorRight { get; private set; }
		
		public bool useCustomWallColor { get; private set; } = false;
		public Color customWallColor { get; private set; }
		
		private ColorManager colorManager;
		private ColorScheme colorScheme;
		private BasicSaberModelController[] sabers;
		
		private MethodInfo customSabersApplyColorsMethod;
		private object customSabersSaberScript;
		
		private bool updateCustomSabers;
		
		private float disableSaberColorsOn = -1f;
		private float disableNoteColorsOn = -1f;
		private float disableWallColorOn = -1f;
		
		public void Awake() {
			
			if (instance != null) {
				
				Logger.log.Warn("Instance of ColorController already exists, destroying.");
				
				Destroy(this);
				
				return;
				
			}
			
			instance = this;
			
		}
		
		public void Start() {
			
			sabers = Resources.FindObjectsOfTypeAll<BasicSaberModelController>();
			
			colorManager = Helper.GetValue<ColorManager>(sabers[0], "_colorManager");
			colorScheme = Helper.GetValue<ColorScheme>(colorManager, "_colorScheme");
			
			updateCustomSabers = PluginManager.GetPluginFromId("Custom Sabers") != null;
			
			if (updateCustomSabers)
				InitCustomSabers();
			
			enabled = false;
			
		}
		
		private void InitCustomSabers() {
			
			try {
				
				Assembly customSabersAssembly = typeof(CustomSaber.Plugin).Assembly;
				Type saberScriptType = customSabersAssembly.GetType("CustomSaber.Utilities.SaberScript", true);
				
				customSabersApplyColorsMethod = saberScriptType.GetMethod("ApplyColorsToSaber");
				customSabersSaberScript = saberScriptType.GetField("instance").GetValue(null);
				
			} catch (Exception e) {
				
				Logger.log.Error("Error whilst trying to access Custom Sabers");
				Logger.log.Error(e.GetType().Name + ": " + e.Message);
				Logger.log.Error(e.StackTrace);
				
				updateCustomSabers = false;
				
			}
			
		}
		
		public void SetSaberColors(Color leftColor, Color rightColor) {
			
			if (enabled) {
				
				disableSaberColorsOn = -1f;
				
				if (disableNoteColorsOn == -1f && disableWallColorOn == -1f)
					enabled = false;
				
			}
			
			saberColorLeft = leftColor;
			saberColorRight = rightColor;
			
			useCustomSaberColors = true;
			
			UpdateSaberColors(leftColor, rightColor);
			
		}
		
		public void DisableSaberColorsIn(float duration) {
			
			disableSaberColorsOn = Time.time + duration;
			
			enabled = true;
			
		}
		
		public void DisableSaberColors() {
			
			if (enabled) {
				
				disableSaberColorsOn = -1f;
				
				if (disableNoteColorsOn == -1f && disableWallColorOn == -1f)
					enabled = false;
				
			}
			
			useCustomSaberColors = false;
			
			UpdateSaberColors(colorManager.ColorForSaberType(SaberType.SaberA), colorManager.ColorForSaberType(SaberType.SaberB));
			
		}
		
		private void UpdateSaberColors(Color leftColor, Color rightColor) {
			
			foreach (BasicSaberModelController saber in sabers) {
				
				if (Helper.GetValue<ColorManager>(saber, "_colorManager") == null)
					continue;
				
				SetSaberGlowColor[] glowColors = Helper.GetValue<SetSaberGlowColor[]>(saber, "_setSaberGlowColors");
				SetSaberFakeGlowColor[] fakeGlowColors = Helper.GetValue<SetSaberFakeGlowColor[]>(saber, "_setSaberFakeGlowColors");
				
				Color color = Helper.GetValue<SaberType>(glowColors[0], "_saberType") == SaberType.SaberA ? leftColor : rightColor;
				
				Color trailTintColor = Helper.GetValue<BasicSaberModelController.InitData>(saber, "_initData").trailTintColor;
				XWeaponTrail trail = Helper.GetValue<XWeaponTrail>(saber, "_saberWeaponTrail");
				trail.color = (color * trailTintColor).linear;
				
				Light light = Helper.GetValue<Light>(saber, "_light");
				light.color = color;
				
				foreach (SetSaberGlowColor glowColor in glowColors)
					glowColor.SetColors();
				
				foreach (SetSaberFakeGlowColor fakeGlowColor in fakeGlowColors)
					fakeGlowColor.SetColors();
				
			}
			
			SetPSSaberGlowColor[] psGlowColors = Resources.FindObjectsOfTypeAll<SetPSSaberGlowColor>();
			
			foreach (SetPSSaberGlowColor psGlowColor in psGlowColors) {
				
				Color color = Helper.GetValue<SaberTypeObject>(psGlowColor, "_saber").saberType == SaberType.SaberA ? leftColor : rightColor;
				
				ParticleSystem ps = Helper.GetValue<ParticleSystem>(psGlowColor, "_particleSystem");
				
				ParticleSystem.MainModule main = ps.main;
				main.startColor = color;
				Helper.SetValue<ParticleSystem.MainModule>(ps, "main", main);
				
			}
			
			SetBlocksBladeSaberGlowColor[] blocksBladeGlowColors = Resources.FindObjectsOfTypeAll<SetBlocksBladeSaberGlowColor>();
			
			foreach (SetBlocksBladeSaberGlowColor blocksBladeGlowColor in blocksBladeGlowColors) {
				
				Color color = Helper.GetValue<SaberTypeObject>(blocksBladeGlowColor, "_saber").saberType == SaberType.SaberA ? leftColor : rightColor;
				
				BlocksBlade blocksBlade = Helper.GetValue<BlocksBlade>(blocksBladeGlowColor, "_blocksBlade");
				
				blocksBlade.color = color;
				
			}
			
			SetSaberBladeParams[] bladeParamsArray = Resources.FindObjectsOfTypeAll<SetSaberBladeParams>();
			
			foreach (SetSaberBladeParams bladeParams in bladeParamsArray) {
				
				Color color = Helper.GetValue<SaberTypeObject>(bladeParams, "_saber").saberType == SaberType.SaberA ? leftColor : rightColor;
				
				MeshRenderer renderer = Helper.GetValue<MeshRenderer>(bladeParams, "_meshRenderer");
				
				MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
				renderer.GetPropertyBlock(propertyBlock);
				
				SetSaberBladeParams.PropertyTintColorPair[] tintPairs = Helper.GetValue< SetSaberBladeParams.PropertyTintColorPair[]>(bladeParams, "_propertyTintColorPairs");
				
				foreach(SetSaberBladeParams.PropertyTintColorPair tintPair in tintPairs) {
					
					propertyBlock.SetColor(tintPair.property, color * tintPair.tintColor);
					
				}
				
				renderer.SetPropertyBlock(propertyBlock);
				
			}
			
			float h;
			float s;
			float v;
			
			Color.RGBToHSV(leftColor, out h, out s, out v);
			Color leftEffectsColor = Color.HSVToRGB(h, s, v);
			
			Color.RGBToHSV(rightColor, out h, out s, out v);
			Color rightEffectsColor = Color.HSVToRGB(h, s, v);
			
			SaberBurnMarkSparkles[] burnMarkSparklesArray = Resources.FindObjectsOfTypeAll<SaberBurnMarkSparkles>();
			
			foreach (SaberBurnMarkSparkles burnMarkSparkles in burnMarkSparklesArray) {
				
				Saber[] sabers = Helper.GetValue<Saber[]>(burnMarkSparkles, "_sabers");
				ParticleSystem[] ps = Helper.GetValue<ParticleSystem[]>(burnMarkSparkles, "_burnMarksPS");
				
				for (int i = 0; i < 2; i++) {
					
					Color color = sabers[i].saberType == SaberType.SaberA ? leftEffectsColor : rightEffectsColor;
					
					ParticleSystem.MainModule main = ps[i].main;
					main.startColor = color;
					
				}
				
			}
			
			SaberBurnMarkArea[] burnMarkAreas = Resources.FindObjectsOfTypeAll<SaberBurnMarkArea>();
			
			foreach (SaberBurnMarkArea burnMarkArea in burnMarkAreas) {
				
				Saber[] sabers = Helper.GetValue<Saber[]>(burnMarkArea, "_sabers");
				LineRenderer[] lineRenderers = Helper.GetValue<LineRenderer[]>(burnMarkArea, "_lineRenderers");
				
				for (int i = 0; i < 2; i++) {
					
					Color color = sabers[i].saberType == SaberType.SaberA ? leftEffectsColor : rightEffectsColor;
					
					lineRenderers[i].startColor = color;
					lineRenderers[i].endColor = color;
					
				}
				
			}
			
			Color clashColor = Color.Lerp(leftEffectsColor, rightEffectsColor, 0.5f);
			
			SaberClashEffect[] clashEffects = Resources.FindObjectsOfTypeAll<SaberClashEffect>();
			
			foreach (SaberClashEffect clashEffect in clashEffects) {
				
				ParticleSystem sparklePS = Helper.GetValue<ParticleSystem>(clashEffect, "_sparkleParticleSystem");
				ParticleSystem glowPS = Helper.GetValue<ParticleSystem>(clashEffect, "_glowParticleSystem");
				
				ParticleSystem.MainModule main = sparklePS.main;
				main.startColor = clashColor;
				
				main = glowPS.main;
				main.startColor = clashColor;
				
			}
			
			if (updateCustomSabers) {
				
				try {
					
					GameObject leftSaber = Helper.GetValue<GameObject>(customSabersSaberScript, "leftSaber");
					GameObject rightSaber = Helper.GetValue<GameObject>(customSabersSaberScript, "rightSaber");
					
					customSabersApplyColorsMethod.Invoke(null, new object[] { leftSaber, leftColor });
					customSabersApplyColorsMethod.Invoke(null, new object[] { rightSaber, rightColor });
					
				} catch (Exception e) {
					
					Logger.log.Error("Error whilst trying to update Custom Sabers");
					Logger.log.Error(e.GetType().Name + ": " + e.Message);
					Logger.log.Error(e.StackTrace);
					
				}
				
			}
			
		}
		
		public void SetNoteColors(Color leftColor, Color rightColor) {
			
			if (enabled) {
				
				disableNoteColorsOn = -1f;
				
				if (disableSaberColorsOn == -1f && disableWallColorOn == -1f)
					enabled = false;
				
			}
			
			noteColorLeft = leftColor;
			noteColorRight = rightColor;
			
			useCustomNoteColors = true;
			
			UpdateNoteColors(leftColor, rightColor);
			
		}
		
		public void DisableNoteColorsIn(float duration) {
			
			disableNoteColorsOn = Time.time + duration;
			
			enabled = true;
			
		}
		
		public void DisableNoteColors() {
			
			if (enabled) {
				
				disableNoteColorsOn = -1f;
				
				if (disableSaberColorsOn == -1f && disableWallColorOn == -1f)
					enabled = false;
				
			}
			
			useCustomNoteColors = false;
			
			UpdateNoteColors(colorManager.ColorForNoteType(NoteType.NoteA), colorManager.ColorForNoteType(NoteType.NoteB));
			
		}
		
		private void UpdateNoteColors(Color leftColor, Color rightColor) {
			
			ColorNoteVisuals[] notes = Resources.FindObjectsOfTypeAll<ColorNoteVisuals>();
			
			foreach (ColorNoteVisuals note in notes) {
				
				NoteData noteData = Helper.GetValue<NoteController>(note, "_noteController").noteData;
				
				if (noteData == null)
					continue;
				
				NoteType noteType = noteData.noteType;
				
				Color color;
				
				switch (noteType) {
				case NoteType.NoteA:
					color = leftColor;
					break;
				case NoteType.NoteB:
					color = rightColor;
					break;
				default:
					continue;
				}
				
				float arrowGlowIntensity = Helper.GetValue<float>(note, "_arrowGlowIntensity");
				
				Helper.SetValue<Color>(note, "_noteColor", color);
				Helper.GetValue<SpriteRenderer>(note, "_circleGlowSpriteRenderer").color = color;
				Helper.GetValue<SpriteRenderer>(note, "_arrowGlowSpriteRenderer").color = color.ColorWithAlpha(arrowGlowIntensity);
				
				MaterialPropertyBlockController[] propertyBlockControllers = Helper.GetValue<MaterialPropertyBlockController[]>(note, "_materialPropertyBlockControllers");
				
				foreach (MaterialPropertyBlockController propertyBlockController in propertyBlockControllers) {
					
					propertyBlockController.materialPropertyBlock.SetColor(colorID, color);
					propertyBlockController.ApplyChanges();
					
				}
				
			}
			
		}
		
		public void SetWallColor(Color color) {
			
			if (enabled) {
				
				disableWallColorOn = -1f;
				
				if (disableSaberColorsOn == -1f && disableNoteColorsOn == -1f)
					enabled = false;
				
			}
			
			customWallColor = color;
			
			useCustomWallColor = true;
			
			UpdateWallColor(color);
			
		}
		
		public void DisableWallColorIn(float duration) {
			
			disableWallColorOn = Time.time + duration;
			
			enabled = true;
			
		}
		
		public void DisableWallColor() {
			
			if (enabled) {
				
				disableWallColorOn = -1f;
				
				if (disableSaberColorsOn == -1f && disableNoteColorsOn == -1f)
					enabled = false;
				
			}
			
			useCustomWallColor = false;
			
			UpdateWallColor(colorScheme.obstaclesColor);
			
		}
		
		private void UpdateWallColor(Color color) {
			
			ObstacleController[] walls = Resources.FindObjectsOfTypeAll<ObstacleController>();
			
			foreach (ObstacleController wall in walls) {
				
				StretchableObstacle stretchable = Helper.GetValue<StretchableObstacle>(wall, "_stretchableObstacle");
				
				ParametricBoxFrameController frame = Helper.GetValue<ParametricBoxFrameController>(stretchable, "_obstacleFrame");
				
				stretchable.SetSizeAndColor(frame.width, frame.height, frame.length, color);
				
			}
			
		}
		
		public void Update() {
			
			if (disableSaberColorsOn != -1f && Time.time > disableSaberColorsOn) {
				
				useCustomSaberColors = false;
				
				UpdateSaberColors(colorManager.ColorForSaberType(SaberType.SaberA), colorManager.ColorForSaberType(SaberType.SaberB));
				
				disableSaberColorsOn = -1f;
				
			}
			
			if (disableNoteColorsOn != -1f && Time.time > disableNoteColorsOn) {
				
				useCustomNoteColors = false;
				
				UpdateNoteColors(colorManager.ColorForNoteType(NoteType.NoteA), colorManager.ColorForNoteType(NoteType.NoteB));
				
				disableNoteColorsOn = -1f;
				
			}
			
			if (disableWallColorOn != -1f && Time.time > disableWallColorOn) {
				
				useCustomWallColor = false;
				
				UpdateWallColor(colorScheme.obstaclesColor);
				
				disableWallColorOn = -1f;
				
			}
			
			if (disableSaberColorsOn == -1f && disableNoteColorsOn == -1f && disableWallColorOn == -1f)
				enabled = false;
			
		}
		
		public void OnDestroy() {
			
			if (instance == this)
				instance = null;
			
		}
		
	}
	
}