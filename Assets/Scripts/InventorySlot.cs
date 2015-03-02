using UnityEngine;
using System.Collections;

public class InventorySlot
{

	public Item itemHere;
	public int ammountHere;


	public void set(Item item, int ammount)
	{
		itemHere =  item;
		ammountHere = ammount;
	}

	public void addToHere(int ammount)
	{
		ammountHere += ammount;
	}
}
