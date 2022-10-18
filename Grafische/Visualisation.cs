using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;

namespace Grafische
{
    using static Visualisation.Orientation;
    using static SectionType;

    public static class Visualisation
    {
        private record PlayerRenderer(Point sectionPos, Section section, SectionData data, Orientation dir, bool mirror) { }

        public static object drawingLock = new();

        public enum Orientation
        {
            NORTH = 0,
            EAST  = 90,
            SOUTH = 180,
            WEST  = 270
        }

        #region graphics
        readonly static string imgdir = Directory.GetCurrentDirectory() + "/img";//"pack://application:,,,/img";

        public readonly static string broken      = $"{imgdir}/broken.png";
        public readonly static string skater      = $"{imgdir}/skater_{{0:s}}.png"; // string format {0:s} => string.Format(..)

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

            Queue<PlayerRenderer> renderQueue = new();

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
                    if (data.Left is not null || data.Right is not null)
                    {
                        renderQueue.Enqueue(new PlayerRenderer(finalPos, section, data, dir, shouldMirror));
                    }
                }

                while (renderQueue.Count > 0)
                {
                    PlayerRenderer renderInfo = renderQueue.Dequeue();

                    int x = renderInfo.sectionPos.X;
                    int y = renderInfo.sectionPos.Y;
                    bitmap.SetPixel(x, y, Color.Red);

                    double ToRadians(double degrees)
                    {
                        return degrees * Math.PI / 180;
                    }

                    (int, int) Compute(int distance, bool left)
                    {
                        int offset = (int)(distance / (float)Section.sectionLength * ImageManager.IMAGE_SIZE);

                        if (IsTurn(renderInfo.section.SectionType))
                        {
                            double angle =  ToRadians(offset / (double)ImageManager.IMAGE_SIZE * 90.0 + (double)renderInfo.dir - 180);
                            
                            double radius = left ? 100 : 20;

                            int x_offset = (int)(Math.Sin(angle) * radius);
                            int y_offset = (int)(Math.Cos(angle) * radius) * -1;

                            x += x_offset - ImageManager.SKATER_SIZE / 2;
                            y += y_offset - ImageManager.SKATER_SIZE / 2;

                            Orientation dir = renderInfo.dir;
                            if (renderInfo.section.SectionType == RightCorner)
                            {
                                switch (renderInfo.dir)
                                {
                                    case SOUTH:
                                    {
                                        y += ImageManager.IMAGE_SIZE;
                                        break;
                                    }

                                    case NORTH:
                                    {
                                        x += ImageManager.IMAGE_SIZE;
                                        break;
                                    }

                                    case EAST:
                                    {
                                        x += ImageManager.IMAGE_SIZE;
                                        y += ImageManager.IMAGE_SIZE;
                                        break;
                                    }

                                    default:
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                switch (renderInfo.dir)
                                {
                                    case WEST:
                                    {
                                        y += ImageManager.IMAGE_SIZE;
                                        break;
                                    }

                                    case EAST:
                                    {
                                        x += ImageManager.IMAGE_SIZE;
                                        break;
                                    }

                                    case SOUTH:
                                    {
                                        x += ImageManager.IMAGE_SIZE;
                                        y += ImageManager.IMAGE_SIZE;
                                        break;
                                    }

                                    default:
                                    {
                                        break;
                                    }
                                }
                            }                            
                        }
                        else if (IsHorizontal(renderInfo.dir))
                        {
                            if (left)
                            {
                                y += ImageManager.HALF_IMAGE * (renderInfo.mirror ? 1 : 0);
                            }
                            else
                            {
                                y += ImageManager.HALF_IMAGE * (renderInfo.mirror ? 0 : 1);
                            }

                            if (renderInfo.mirror)
                            {
                                x += ImageManager.IMAGE_SIZE - distance;
                            }
                            else
                            {
                                x += offset;
                            }
                        }
                        else
                        {
                            if (left)
                            {
                                x += (int)(ImageManager.HALF_IMAGE * 1.33334) * (renderInfo.mirror ? 0 : 1);
                            }
                            else
                            {
                                x += (int)(ImageManager.HALF_IMAGE * 1.33334) * (renderInfo.mirror ? 1 : 0);
                            }

                            if (renderInfo.mirror)
                            {
                                y += ImageManager.IMAGE_SIZE - distance;
                            }
                            else
                            {
                                y += offset;
                            }
                        }

                        return (x, y);
                    }

                    if (renderInfo.data.Left is not null)
                    {
                        Bitmap skaterBitmap = ImageManager.GetBitmapData(string.Format(skater, renderInfo.data.Left.TeamColor.ToString().ToLower()));

                        (x, y) = Compute(renderInfo.data.DistanceLeft, true);
                    
                        g.DrawImage(skaterBitmap, x, y);
                    }

                    x = renderInfo.sectionPos.X;
                    y = renderInfo.sectionPos.Y;

                    if (renderInfo.data.Right is not null)
                    {
                        Bitmap skaterBitmap = ImageManager.GetBitmapData(string.Format(skater, renderInfo.data.Right.TeamColor.ToString().ToLower()));

                        (x, y) = Compute(renderInfo.data.DistanceRight, false);

                        g.DrawImage(skaterBitmap, x, y);
                    }
                }
            }

            return bitmap;
        }

        private static bool IsTurn(SectionType sectionType)
        {
            return sectionType == LeftCorner || sectionType == RightCorner;
        }
    }
}
