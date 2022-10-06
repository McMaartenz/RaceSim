using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;
using static ConsoleView.Orientation;
using static Model.SectionType;

namespace ConsoleView
{
    public enum Orientation
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    public static class Visualisation
    {
        static object drawingLock = new();

        static string straight_h;
        static string straight_v;

        static string corner_SW;
        static string corner_SE;
        static string corner_NE;
        static string corner_NW;

        static string startgrid_h;
        static string startgrid_v;

        static string finish_h;
        static string finish_v;

        public static void Initialize()
        {
            #region Graphics
            Console.OutputEncoding = Encoding.Unicode;

            straight_h =
                "────────\n" +
                "  2     \n" +
                "        \n" +
                "     1  \n" +
                "────────\n";

            straight_v =
                "│      │\n" +
                "│    2 │\n" +
                "│      │\n" +
                "│ 1    │\n" +
                "│      │\n";

            startgrid_h =
                "───┬────\n" +
                "  2┊    \n" +
                "        \n" +
                "     1┊ \n" +
                "──────┴─\n";

            startgrid_v =
                "│    2 │\n" +
                "│    ┈┈┤\n" +
                "│ 1    │\n" +
                "├┈┈    │\n" +
                "│      │\n";

            finish_h =
                "────────\n" +
                "  2  ▚  \n" +
                "     ▚  \n" +
                "     ▚1 \n" +
                "────────\n";

            finish_v =
                "│      │\n" +
                "│    2 │\n" +
                "│▚▚▚▚▚▚│\n" +
                "│ 1    │\n" +
                "│      │\n";

            corner_SW =
                "───────╮\n" +
                "    2  │\n" +
                "       │\n" +
                "  1    │\n" +
                "╮      │\n";

            corner_SE =
                "╭───────\n" +
                "│  2    \n" +
                "│       \n" +
                "│     1 \n" +
                "│      ╭\n";

            corner_NE =
                "│      ╰\n" +
                "│     1 \n" +
                "│2      \n" +
                "│       \n" +
                "╰───────\n";

            corner_NW =
                "╯1     │\n" +
                "       │\n" +
                "       │\n" +
                "     2 │\n" +
                "───────╯\n";

            #endregion

            //TODO: next track redo event subscription
            Data.CurrentRace.DriversChanged += Track_DriversChanged;
        }

        public static bool IsHorizontal(Orientation dir)
        {
            return dir == WEST || dir == EAST;
        }

        public static bool IsVertical(Orientation dir)
        {
            return !IsHorizontal(dir);
        }

        private static void PrintAt(Point pos, string str)
        {
            int x = pos.X;
            int y = pos.Y;

            Console.SetCursorPosition(x, y);
            StringBuilder sb = new();
            foreach (char c in str)
            {
                if (c == '\n')
                {
                    x = pos.X;
                    Console.Write(sb.ToString());
                    Console.SetCursorPosition(x, ++y);
                    sb = new();
                    continue;
                }

                Console.SetCursorPosition(x++, y);
                sb.Append(c);
            }
        }

        private static void FillParticipants(ref string dData, SectionData sData)
        {
            dData = dData.Replace('1', sData.Left?.Name[0] ?? ' ');
            dData = dData.Replace('2', sData.Right?.Name[0] ?? ' ');
        }

        public static void Track_DriversChanged(object sender, DriversChangedEventArgs e)
        {
            DrawTrack(e.track, Data.CurrentRace);
        }

        public static void DrawTrack(Track track, Race race)
        {
            Point point = new(1, 1);
            Orientation dir = EAST;

            lock (drawingLock)
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.White;
                foreach (Section section in track.Sections)
                {
                    SectionType type = section.SectionType;
                    SectionData data = race.GetSectionData(section);
                    string drawingData = "Error";

                    int x = dir == EAST  ? 1 : (dir == WEST  ? -1 : 0);
                    int y = dir == SOUTH ? 1 : (dir == NORTH ? -1 : 0);

                    point.Offset(x, y);

                    switch (type)
                    {
                        case Finish:
                        {
                            drawingData = IsHorizontal(dir) ? finish_h : finish_v;
                            break;
                        }

                        case StartGrid:
                        {
                            drawingData = IsHorizontal(dir) ? startgrid_h : startgrid_v;
                            break;
                        }

                        case Straight:
                        {
                            drawingData = IsHorizontal(dir) ? straight_h : straight_v;
                            break;
                        }

                        case LeftCorner:
                        {
                            Dictionary<Orientation, (Orientation, string)> orientationMappings = new()
                            {
                                { NORTH, (WEST, corner_SW) },
                                { WEST, (SOUTH, corner_SE) },
                                { SOUTH, (EAST, corner_NE) },
                                { EAST, (NORTH, corner_NW) },
                            };

                            var result = orientationMappings[dir];

                            dir = result.Item1;
                            drawingData = result.Item2;
                            break;
                        }

                        case RightCorner:
                        {
                            Dictionary<Orientation, (Orientation, string)> orientationMappings = new()
                            {
                                { NORTH, (EAST, corner_SE) },
                                { EAST, (SOUTH, corner_SW) },
                                { SOUTH, (WEST, corner_NW) },
                                { WEST, (NORTH, corner_NE) },
                            };

                            var result = orientationMappings[dir];

                            dir = result.Item1;
                            drawingData = result.Item2;
                            break;
                        }

                        default:
                        {
                            break;
                        }
                    }

                    Point finalPos = new(point.X * 8, point.Y * 5);
                    FillParticipants(ref drawingData, data);
                    PrintAt(finalPos, drawingData);
                }
            }
        }
    }
}
