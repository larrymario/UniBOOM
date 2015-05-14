using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Uniboom.Utility.Map {

	// Global map generator
	public class MapGenerator
	{
		// Sample map (0->null, 1->MapRoom):
		//     (0) (1) (2) (3) (4) (5) (6) (7)
		//(0)  0   0   0   0   0   0   0   0
		//(1)  0   0   1   0   1   0   0   0
		//(2)  1   1   1   1   1   0   0   0
		//(3)  0   0   1   0   0   0   0   0
		//(4)  0   0   0   0   0   0   0   0

		// Number of rooms to be created
		public uint roomNum = 8;
		// Room depth (num of main branch)
		protected uint roomdepth;
		// Current number of rooms
		public uint currRoomNum = 0;
		// Matrix for MapElement, 8*5. The first one comes from (3,1).
		// Index starts from 1 but 0.
		public List<MapRoom> mapMatrix = null;

		// Contructor without params
		public MapGenerator(uint depth, uint count)
		{
			roomdepth = depth;
			roomNum = count;
			mapMatrix = new List<MapRoom>();
			for (uint i = 0; i < 40; ++i)
			{
				mapMatrix.Add(null);
			}
		}
		
		// Get method for mapMatrix
		// row, col starts from 1 but 0.
		public MapRoom GetElementAtIndex(int row, int col)
		{
			if (row<0 || col<0 || row>4 || col>7)
			{
				return null;
			}
			return mapMatrix[row * 8 + col];
		}
		
		// Set method for mapMatrix
		// row, col starts from 1 but 0.
		// If an element exists return false
		protected bool SetElementAtIndex(int row, int col, MapRoom room)
		{
			if (this.GetElementAtIndex(row, col) != null)
			{
				return false;
			}
			else
			{
				mapMatrix[row * 8 + col] = room;
				room.pos_x = (uint)row;
				room.pos_y = (uint)col;
				return true;
			}
		}
		
		// Map Generation method
		public MapRoom GlobalMapGeneration()
		{
			// For main branch of rooms (there will be doors of left and right)
			MapRoom lastRoom = null;// Use this to connect two rooms
			for (int i = 0; i < roomdepth; ++i)
			{
				MapRoom room = new MapRoom();
				room.left = new MapDoor(room);
				room.right = new MapDoor(room);
				
				if (lastRoom!=null)
				{
					this.connectTwoRooms(lastRoom.right, room.left);
				}
				
				this.SetElementAtIndex(2, i, room);
				lastRoom = room;
			}
			this.currRoomNum = roomdepth;
			
			// For rooms left
			while (this.currRoomNum < roomNum)
			{
				// Get a point randomly and check its connectivity
				int row = Random.Range(1, 5);
				int col = Random.Range(1, 8);
				if (this.isRoomConnected(row, col))
				{
					// Create a new room
					MapRoom room = new MapRoom();
					this.SetElementAtIndex(row, col, room);
					// Set doors to join two rooms
					// Up
					lastRoom = (MapRoom)this.GetElementAtIndex(row - 1, col);
					if (lastRoom!=null)
					{
						MapDoor d1 = new MapDoor(lastRoom);
						MapDoor d2 = new MapDoor(room);
						this.connectTwoRooms(d1, d2);
					}
					
					// Down
					lastRoom = (MapRoom)this.GetElementAtIndex(row + 1, col);
					if (lastRoom != null)
					{
						MapDoor d1 = new MapDoor(room);
						MapDoor d2 = new MapDoor(lastRoom);
						this.connectTwoRooms(d1, d2);
					}
					
					// Left
					lastRoom = (MapRoom)this.GetElementAtIndex(row, col - 1);
					if (lastRoom != null)
					{
						MapDoor d1 = new MapDoor(room);
						MapDoor d2 = new MapDoor(lastRoom);
						this.connectTwoRooms(d1, d2);
					}
					
					// Right
					lastRoom = (MapRoom)this.GetElementAtIndex(row, col + 1);
					if (lastRoom != null)
					{
						MapDoor d1 = new MapDoor(lastRoom);
						MapDoor d2 = new MapDoor(room);
						this.connectTwoRooms(d1, d2);
					}
					++this.currRoomNum;
				}
			}
			return this.GetElementAtIndex(2,0);
		}
		
		// Retrieve matrix method, starting from point (3, 1)
		public void Retrieve()
		{
			for (int row = 0; row < 5; row++)
			{
				for (int col = 0; col < 8; col++)
				{
					MapRoom element = this.GetElementAtIndex(row, col);
					if (element != null)
					{
						System.Console.Write("\t1");
					}
					else
					{
						System.Console.Write("\t0");
					}
				}
				System.Console.Write("\n");
			}
		}
		
		// Connects two rooms and generate path
		protected void connectTwoRooms(MapDoor d1, MapDoor d2)
		{
			//  Switch rooms
			MapRoom tpRoom = d1.nextRoom;
			d1.nextRoom = d2.nextRoom;
			d2.nextRoom = tpRoom;
		}
		
		// Check room connectivity
		protected bool isRoomConnected(int row, int col)
		{
			if (this.GetElementAtIndex(row, col)!=null)
			{
				// A room already existed
				return false;
			}
			
			if (this.GetElementAtIndex(row-1, col)==null &&
			    this.GetElementAtIndex(row+1, col)==null &&
			    this.GetElementAtIndex(row, col-1)==null &&
			    this.GetElementAtIndex(row, col+1)==null)
			{
				// No neighbor room existed
				return false;
			}
			
			return true;
		}
	}
}

