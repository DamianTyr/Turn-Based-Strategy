using System;
using Colony;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_isReserved")]
	public class ES3UserType_ColonyGridObject : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ColonyGridObject() : base(typeof(ColonyGridObject)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (ColonyGridObject)obj;
			
			writer.WritePrivateField("_isReserved", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (ColonyGridObject)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_isReserved":
					instance = (ColonyGridObject)reader.SetPrivateField("_isReserved", reader.Read<System.Boolean>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new ColonyGridObject();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_ColonyGridObjectArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ColonyGridObjectArray() : base(typeof(ColonyGridObject[]), ES3UserType_ColonyGridObject.Instance)
		{
			Instance = this;
		}
	}
}