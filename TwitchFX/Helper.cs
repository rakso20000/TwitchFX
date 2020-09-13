using System.Reflection;

namespace TwitchFX {
	
	public class Helper {
		
		public static T GetValue<T>(object obj, string name) {
			
			FieldInfo field = obj.GetType().GetField(
				name,
				BindingFlags.NonPublic |
				BindingFlags.Public |
				BindingFlags.Instance
			);
			
			return (T) field.GetValue(obj);
			
		}
		
		public static void SetValue<T>(object obj, string name, T value) {
			
			FieldInfo field = obj.GetType().GetField(
				name,
				BindingFlags.NonPublic |
				BindingFlags.Public |
				BindingFlags.Instance
			);
			
			field.SetValue(obj, value);
			
		}
		
	}
	
}