using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{
    enum RoomType  { LABIRINT, NEST } // Nest = Гнездо

    class Room
    {
        public Cell[,] room;
        RoomType type;

        public Room(SpriteBatch spriteBatch)
        {
            room = new Cell[Const.SizeRoom, Const.SizeRoom];

            for (int i = 0; i < Const.SizeRoom; ++i)
                for (int j = 0; j < Const.SizeRoom; ++j)
                    room[i,j] = new Cell(spriteBatch, new Vector2(i * Const.SizeCell, j * Const.SizeCell));
        }

        public static Room GetRoadRoom(SpriteBatch spriteBatch)
        {
            Room room = new Room(spriteBatch);
            for (int i = 0; i < Const.SizeRoom; ++i)
                for (int j = 0; j < Const.SizeRoom; ++j)
                    room.room[i, j].Type = CellType.WALL;
            return room;
        }
    }
}
