using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("item", "number")]
	public class ES3UserType_InventorySlot : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_InventorySlot() : base(typeof(InventorySystem.Inventories.Inventory.InventorySlot)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (InventorySystem.Inventories.Inventory.InventorySlot)obj;
			
			writer.WritePropertyByRef("item", instance.item);
			writer.WriteProperty("number", instance.number, ES3Type_int.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new InventorySystem.Inventories.Inventory.InventorySlot();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "item":
						instance.item = reader.Read<InventorySystem.Inventories.InventoryItem>();
						break;
					case "number":
						instance.number = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_InventorySlotArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_InventorySlotArray() : base(typeof(InventorySystem.Inventories.Inventory.InventorySlot[]), ES3UserType_InventorySlot.Instance)
		{
			Instance = this;
		}
	}
}