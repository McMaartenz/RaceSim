using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Grafische
{
    using static Visualisation.Orientation;
    using static SectionType;

    public static class Visualisation
    {
        public static object drawingLock = new();

        public enum Orientation
        {
            NORTH,
            EAST,
            SOUTH,
            WEST
        }

        #region graphics
        readonly static string imgdir = Directory.GetCurrentDirectory() + "/img";//"pack://application:,,,/img";

        public readonly static string broken      = $"{imgdir}/broken.png";
        public readonly static string skater      = $"{imgdir}/skater_%s.png"; // string format %s => teamcolor.tostring

        public readonly static string straight_h  = $"{imgdir}/horizontal.png";
        public readonly static string straight_v  = $"{imgdir}/vertical.png";
        public readonly static string corner_SW   = $"{imgdir}/southwest.png";
        public readonly static string corner_SE   = $"{imgdir}/southeast.png";
        public readonly static string corner_NE   = $"{imgdir}/northeast.png";
        public readonly static string corner_NW   = $"{imgdir}/northwest.png";
        public readonly static string startgrid_h = $"{imgdir}/starthorizontal.png";
        public readonly static string startgrid_v = $"{imgdir}/startvertical.png";
        public readonly static string finish_h    = $"{imgdir}/finishhorizontal.png";
        public readonly static string finish_v    = $"{imgdir}/finishvertical.png";
        #endregion

        public static bool IsHorizontal(Orientation dir)
        {
            return dir == WEST || dir == EAST;
        }

        public static Bitmap DrawTrack(Track track, Race race)
        {

            (int w, int h) = ImageManager.CalculateTrackPixelDimensions(Controller.Data.CurrentRace.Track);

            ImageManager.trackWidth = w;
            ImageManager.trackHeight = h;

            Bitmap bitmap = (Bitmap)ImageManager.GetBitmapData("empty").Clone();
            Graphics g = Graphics.FromImage(bitmap);
            
            Point point = new(0, 0);
            Orientation dir = EAST;
            bool shouldMirror;

            lock (drawingLock)
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.White;
                foreach (Section section in track.Sections)
                {
                    SectionType type = section.SectionType;
                    SectionData data = race.GetSectionData(section);
                    Bitmap? imageData = null;
                    shouldMirror = false;

                    int x = dir == EAST ? 1 : (dir == WEST ? -1 : 0);
                    int y = dir == SOUTH ? 1 : (dir == NORTH ? -1 : 0);

                    point.Offset(x, y);

                    switch (type)
                    {
                        case Finish:
                        {
                            imageData = ImageManager.GetBitmapData(IsHorizontal(dir) ? finish_h : finish_v);
                            break;
                        }

                        case StartGrid:
                        {
                            imageData = ImageManager.GetBitmapData(IsHorizontal(dir) ? startgrid_h : startgrid_v);
                            break;
                        }

                        case Straight:
                        {
                            imageData = ImageManager.GetBitmapData(IsHorizontal(dir) ? straight_h : straight_v);
                            if (dir == WEST || dir == NORTH)
                            {
                                shouldMirror = true;
                            }
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
                            imageData = ImageManager.GetBitmapData(result.Item2);
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
                            imageData = ImageManager.GetBitmapData(result.Item2);
                            break;
                        }

                        default:
                        {
                            break;
                        }
                    }

                    Point finalPos = new(point.X * ImageManager.IMAGE_SIZE, point.Y * ImageManager.IMAGE_SIZE);

                    g.DrawImage(imageData, finalPos);
                    Console.WriteLine(finalPos);
                    //FillParticipants(ref drawingData, data, shouldMirror);
                    //PrintAt(finalPos, drawingData);
                }
            }

            return bitmap;
        }
    }
}
