using UnityEngine;
using System.Collections;

namespace Uniboom.Utility.Map {

	public class MapDoor
	{
		// The door belongs to a room, and the nextRoom points to another room 
		public MapRoom nextRoom = null;

		MapDoor()
		{}

		public MapDoor(MapRoom room)
		{
			this.nextRoom = room;
		}
	}
}

