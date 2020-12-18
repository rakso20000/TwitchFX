using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IPA.Loader;
using TwitchFX.Lights;
using UnityEngine;
using Zenject;
using SiraUtil.Interfaces;

namespace TwitchFX.Colors {
	
	public class ColorController : LazyController<ColorController> {
		
		private static readonly int colorID = Shader.PropertyToID("_Color");
		
		public bool useCustomSaberColors { get; private set; } = false;
		public Color saberColorLeft { get; private set; }
		public Color saberColorRight { get; private set; }
		
		public bool useCustomNoteColors { get; private set; } = false;
		public Color noteColorLeft { get; private set; }
		public Color noteColorRight { get; private set; }
		
		public bool useCustomWallColor { get; private set; } = false;
		public Color wallColor { get; private set; }
		
		private ColorManager colorManager;
		private ColorScheme colorScheme;
		
		private SaberManager saberManager;
		private SaberModelController leftSaberModel;
		private SaberModelController rightSaberModel;
		
		private BasicBeatmapObjectManager beatmapObjectManager;
		
		private ConditionalWeakTable<GameNoteController, ColorNoteVisuals> noteVisualsMap = new ConditionalWeakTable<GameNoteController, ColorNoteVisuals>();
		
		private bool siraSabersActive = false;
		
		private float disableSaberColorsOn = -1f;
		private float disableNoteColorsOn = -1f;
		private float disableWallColorOn = -1f;
		
		private CustomLightshowController interceptingLightshow = null;
		
		[Inject]
		public void Inject(
			ColorManager colorManager,
			ColorScheme colorScheme,
			SaberManager saberManager,
			BasicBeatmapObjectManager beatmapObjectManager
		) {
			
			this.colorManager = colorManager;
			this.colorScheme = colorScheme;
			this.saberManager = saberManager;
			this.beatmapObjectManager = beatmapObjectManager;
			
		}
		
		protected override void Init() {
			
			leftSaberModel = saberManager.leftSaber.GetComponentInChildren<SaberModelController>(true);
			rightSaberModel = saberManager.rightSaber.GetComponentInChildren<SaberModelController>(true);
			
			if (PluginManager.GetPluginFromId("SiraUtil") != null)
				CheckCustomSabers();
			
			enabled = false;
			
		}
		
		private void CheckCustomSabers() {
			
			siraSabersActive = leftSaberModel is IColorable || rightSaberModel is IColorable;
			
		}
		
		public void SetSaberColors(Color leftColor, Color rightColor, float? duration = null) {
			
			if (interceptingLightshow != null) {
				
				interceptingLightshow.OnInterceptedSaberColors(leftColor, rightColor, Time.time + duration ?? -1f);
				
				return;
				
			}
			
			saberColorLeft = leftColor;
			saberColorRight = rightColor;
			
			useCustomSaberColors = true;
			
			UpdateSaberColors(
				Helper.IsRainbow(leftColor) ? RainbowController.instance.GetLeftColor() : leftColor,
				Helper.IsRainbow(rightColor) ? RainbowController.instance.GetRightColor() : rightColor
			);
			
			disableSaberColorsOn = duration.HasValue ? Time.time + duration.Value : -1f;
			
			SetEnabled();
			
		}
		
		public void DisableSaberColors() {
			
			if (interceptingLightshow != null) {
				
				interceptingLightshow.OnInterceptedSaberColors(null, null, -1f);
				
				return;
				
			}
			
			if (enabled) {
				
				disableSaberColorsOn = -1f;
				
				SetEnabled();
				
			}
			
			useCustomSaberColors = false;
			
			UpdateSaberColors(colorManager.ColorForSaberType(SaberType.SaberA), colorManager.ColorForSaberType(SaberType.SaberB));
			
		}
		
		private void UpdateSaberColors(Color leftColor, Color rightColor) {
			
			if (siraSabersActive) {
				
				UpdateSiraSaberColors(leftColor, rightColor);
				
			}
			
			UpdateSaberColor(leftSaberModel, leftColor);
			UpdateSaberColor(rightSaberModel, rightColor);
			
		}
		
		private void UpdateSaberColor(SaberModelController saberModel, Color color) {
			
			SetSaberGlowColor[] glowColors = Helper.GetValue<SetSaberGlowColor[]>(saberModel, "_setSaberGlowColors");
			SetSaberFakeGlowColor[] fakeGlowColors = Helper.GetValue<SetSaberFakeGlowColor[]>(saberModel, "_setSaberFakeGlowColors");
			
			Color trailTintColor = Helper.GetValue<SaberModelController.InitData>(saberModel, "_initData").trailTintColor;
			SaberTrail trail = Helper.GetValue<SaberTrail>(saberModel, "_saberTrail");
			Helper.SetValue<Color>(trail, "_color", (color * trailTintColor).linear);
			
			TubeBloomPrePassLight light = Helper.GetValue<TubeBloomPrePassLight>(saberModel, "_saberLight");
			
			if (light != null)
				light.color = color;
			
			foreach (SetSaberGlowColor glowColor in glowColors)
				glowColor.SetColors();
			
			foreach (SetSaberFakeGlowColor fakeGlowColor in fakeGlowColors)
				fakeGlowColor.SetColors();
			
		}
		
		private void UpdateSiraSaberColors(Color leftColor, Color rightColor) {
			
			(leftSaberModel as IColorable)?.SetColor(leftColor);
			(rightSaberModel as IColorable)?.SetColor(rightColor);
			
		}
		
		public void SetNoteColors(Color leftColor, Color rightColor, float? duration = null) {
			
			if (interceptingLightshow != null) {
				
				interceptingLightshow.OnInterceptedNoteColors(leftColor, rightColor, Time.time + duration ?? -1f);
				
				return;
				
			}
			
			noteColorLeft = leftColor;
			noteColorRight = rightColor;
			
			useCustomNoteColors = true;
			
			UpdateNoteColors(
				Helper.IsRainbow(leftColor)? RainbowController.instance.GetLeftColor() : leftColor,
				Helper.IsRainbow(rightColor) ? RainbowController.instance.GetRightColor() : rightColor
			);
			
			disableNoteColorsOn = duration.HasValue ? Time.time + duration.Value : -1f;
			
			SetEnabled();
			
		}
		
		public void DisableNoteColors() {
			
			if (interceptingLightshow != null) {
				
				interceptingLightshow.OnInterceptedNoteColors(null, null, -1f);
				
				return;
				
			}
			
			if (enabled) {
				
				disableNoteColorsOn = -1f;
				
				SetEnabled();
				
			}
			
			useCustomNoteColors = false;
			
			UpdateNoteColors(colorManager.ColorForType(ColorType.ColorA), colorManager.ColorForType(ColorType.ColorB));
			
		}
		
		private void UpdateNoteColors(Color leftColor, Color rightColor) {
			
			HashSet<GameNoteController> notes = Helper.GetValue<MonoMemoryPoolContainer<GameNoteController>>(beatmapObjectManager, "_gameNotePoolContainer").activeItems;
			
			foreach (GameNoteController note in notes) {
				
				if (!noteVisualsMap.TryGetValue(note, out ColorNoteVisuals noteVisuals))
					noteVisualsMap.Add(note, noteVisuals = note.GetComponent<ColorNoteVisuals>());
				
				Color color;
				
				switch (note.noteData.colorType) {
				case ColorType.ColorA:
					color = leftColor;
					break;
				case ColorType.ColorB:
					color = rightColor;
					break;
				default:
					continue;
				}
				
				float arrowGlowIntensity = Helper.GetValue<float>(noteVisuals, "_arrowGlowIntensity");
				
				Helper.SetValue<Color>(noteVisuals, "_noteColor", color);
				Helper.GetValue<SpriteRenderer>(noteVisuals, "_circleGlowSpriteRenderer").color = color;
				Helper.GetValue<SpriteRenderer>(noteVisuals, "_arrowGlowSpriteRenderer").color = color.ColorWithAlpha(arrowGlowIntensity);
				
				MaterialPropertyBlockController[] propertyBlockControllers = Helper.GetValue<MaterialPropertyBlockController[]>(noteVisuals, "_materialPropertyBlockControllers");
				
				foreach (MaterialPropertyBlockController propertyBlockController in propertyBlockControllers) {
					
					propertyBlockController.materialPropertyBlock.SetColor(colorID, color);
					propertyBlockController.ApplyChanges();
					
				}
				
			}
			
		}
		
		public void SetWallColor(Color color, float? duration = null) {
			
			if (interceptingLightshow != null) {
				
				interceptingLightshow.OnInterceptedWallColor(color, Time.time + duration ?? -1f);
				
				return;
				
			}
			
			wallColor = color;
			
			useCustomWallColor = true;
			
			UpdateWallColor(Helper.IsRainbow(color) ? RainbowController.instance.GetWallColor() : color);
			
			disableWallColorOn = duration.HasValue ? Time.time + duration.Value : -1f;
			
			SetEnabled();
			
		}
		
		public void DisableWallColor() {
			
			if (interceptingLightshow != null) {
				
				interceptingLightshow.OnInterceptedWallColor(null, -1f);
				
				return;
				
			}
			
			if (enabled) {
				
				disableWallColorOn = -1f;
				
				SetEnabled();
				
			}
			
			useCustomWallColor = false;
			
			UpdateWallColor(colorScheme.obstaclesColor);
			
		}
		
		private void UpdateWallColor(Color color) {
			
			foreach (ObstacleController wall in beatmapObjectManager.activeObstacleControllers) {
				
				StretchableObstacle stretchable = Helper.GetValue<StretchableObstacle>(wall, "_stretchableObstacle");
				
				ParametricBoxFrameController frame = Helper.GetValue<ParametricBoxFrameController>(stretchable, "_obstacleFrame");
				
				stretchable.SetSizeAndColor(frame.width, frame.height, frame.length, color);
				
			}
			
		}
		
		public void SetRestoreValues(CustomLightshowController lightshow) {
			
			if (useCustomSaberColors)
				lightshow.OnInterceptedSaberColors(saberColorLeft, saberColorRight, disableSaberColorsOn);
			
			if (useCustomNoteColors)
				lightshow.OnInterceptedNoteColors(noteColorLeft, noteColorRight, disableNoteColorsOn);
			
			if (useCustomWallColor)
				lightshow.OnInterceptedWallColor(wallColor, disableWallColorOn);
			
		}
		
		public void StartIntercept(CustomLightshowController lightshow) {
			
			interceptingLightshow = lightshow;
			
		}
		
		public void StopIntercept() {
			
			interceptingLightshow = null;
			
		}
		
		public void Update() {
			
			if (disableSaberColorsOn != -1f && Time.time > disableSaberColorsOn) {
				
				useCustomSaberColors = false;
				
				UpdateSaberColors(colorManager.ColorForSaberType(SaberType.SaberA), colorManager.ColorForSaberType(SaberType.SaberB));
				
				disableSaberColorsOn = -1f;
				
			}
			
			if (disableNoteColorsOn != -1f && Time.time > disableNoteColorsOn) {
				
				useCustomNoteColors = false;
				
				UpdateNoteColors(colorManager.ColorForType(ColorType.ColorA), colorManager.ColorForType(ColorType.ColorB));
				
				disableNoteColorsOn = -1f;
				
			}
			
			if (disableWallColorOn != -1f && Time.time > disableWallColorOn) {
				
				useCustomWallColor = false;
				
				UpdateWallColor(colorScheme.obstaclesColor);
				
				disableWallColorOn = -1f;
				
			}
			
			if (useCustomSaberColors && (Helper.IsRainbow(saberColorLeft) || Helper.IsRainbow(saberColorRight)))
				UpdateSaberColors(
					Helper.IsRainbow(saberColorLeft) ? RainbowController.instance.GetLeftColor() : saberColorLeft,
					Helper.IsRainbow(saberColorRight) ? RainbowController.instance.GetRightColor() : saberColorRight
				);
			
			if (useCustomNoteColors && (Helper.IsRainbow(noteColorLeft) || Helper.IsRainbow(noteColorRight)))
				UpdateNoteColors(
					Helper.IsRainbow(noteColorLeft) ? RainbowController.instance.GetLeftColor() : noteColorLeft,
					Helper.IsRainbow(noteColorRight) ? RainbowController.instance.GetRightColor() : noteColorRight
				);
			
			if (useCustomWallColor && Helper.IsRainbow(wallColor))
				UpdateWallColor(RainbowController.instance.GetWallColor());
			
			SetEnabled();
			
		}
		
		private void SetEnabled() {
			
			enabled = disableSaberColorsOn != -1f ||
				disableNoteColorsOn != -1f ||
				disableWallColorOn != -1f ||
				Helper.IsRainbow(saberColorLeft) ||
				Helper.IsRainbow(saberColorRight) ||
				Helper.IsRainbow(noteColorLeft) ||
				Helper.IsRainbow(noteColorRight) ||
				Helper.IsRainbow(wallColor);
			
		}
		
	}
	
}