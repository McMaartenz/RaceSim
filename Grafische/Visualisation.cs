using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Grafische
{
    using static Visualisation.Orientation;
    using static SectionType;

    public static class Visualisation
    {
        public class TrackIterator
        {
            Track track;
            Race race;

            public TrackIterator(Track track, Race race)
            {
                this.track = track;
                this.race = race;
            }

            public IEnumerable<TrackIteration> Iterator()
            {
                Point point = Point.Empty;
                Orientation dir = EAST;
                bool shouldMirror;

                foreach (Section section in track.Sections)
                {
                    SectionType type = section.SectionType;
                    SectionData data = race.GetSectionData(section);
                    shouldMirror = false;

                    int x = dir == EAST  ? 1 : (dir == WEST  ? -1 : 0);
                    int y = dir == SOUTH ? 1 : (dir == NORTH ? -1 : 0);

                    point.Offset(x, y);
                    switch (section.SectionType)
                    {
                        case LeftCorner:
                        {
                            dir = dir switch
                            {
                                NORTH => WEST,
                                WEST  => SOUTH,
                                SOUTH => EAST,
                                EAST  => NORTH
                            };
                            break;
                        }

                        case RightCorner:
                        {
                            dir = dir switch
                            {
                                NORTH => EAST,
                                EAST  => SOUTH,
                                SOUTH => WEST,
                                WEST  => NORTH
                            };
                            break;
                        }

                        case Straight:
                        {
                            if (dir == WEST || dir == NORTH)
                            {
                                shouldMirror = true;
                            }
                            break;
                        }

                        default: break;
                    }

                    yield return new TrackIteration(section, type, data, point, dir, shouldMirror);
                }
            }

            public record TrackIteration(Section section, SectionType type, SectionData data, Point point, Orientation dir, bool shouldMirror) { }
        }

        private record PlayerRenderer(Point sectionPos, Section section, SectionData data, Orientation dir, bool mirror) { }

        public static object updateLock = new();

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

        public static Bitmap DrawEmptyTrack(Track track, Race race)
        {
            Bitmap bitmap = (Bitmap)ImageManager.GetBitmapData("empty").Clone();
            Graphics g = Graphics.FromImage(bitmap);

            TrackIterator iter = new(track, race);
            foreach (var el in iter.Iterator())
            {
                Bitmap? imageData = null;
                string imagePath = "";

                imagePath = el.type switch
                {
                    Finish     => IsHorizontal(el.dir) ? finish_h    : finish_v,
                    StartGrid  => IsHorizontal(el.dir) ? startgrid_h : startgrid_v,
                    Straight   => IsHorizontal(el.dir) ? straight_h  : straight_v,
                    LeftCorner => el.dir switch
                    {
                        NORTH => corner_NW,
                        EAST  => corner_NE,
                        SOUTH => corner_SE,
                        WEST  => corner_SW
                    },
                    RightCorner => el.dir switch
                    {
                        NORTH => corner_NE,
                        EAST  => corner_SE,
                        SOUTH => corner_SW,
                        WEST  => corner_NW
                    }
                };

                imageData = ImageManager.GetBitmapData(imagePath);

                Point calculatedPosition = new(x: el.point.X * ImageManager.IMAGE_SIZE,
                                               y: el.point.Y * ImageManager.IMAGE_SIZE);

                g.DrawImage(imageData, calculatedPosition);
            }

            return bitmap;
        }

        private static (int x, int y) ComputePositionForInfo(PlayerRenderer renderInfo, bool leftSide)
        {
            int x = renderInfo.sectionPos.X * ImageManager.IMAGE_SIZE,
                y = renderInfo.sectionPos.Y * ImageManager.IMAGE_SIZE;

            int dist = leftSide ? renderInfo.data.DistanceLeft : renderInfo.data.DistanceRight;
            dist = (int)(dist / 100.0 * ImageManager.IMAGE_SIZE) - ImageManager.HALF_SKATER;

            switch (renderInfo.section.SectionType)
            {
                case Straight:
                case StartGrid:
                case Finish:
                {
                    if (IsHorizontal(renderInfo.dir))
                    {
                        x += renderInfo.mirror ? ImageManager.IMAGE_SIZE - dist: dist;
                        if (leftSide == renderInfo.mirror)
                        {
                            y += ImageManager.HALF_IMAGE;
                        }
                    }
                    else
                    {
                        y += renderInfo.mirror ? ImageManager.IMAGE_SIZE - dist: dist;
                        if (leftSide != renderInfo.mirror)
                        {
                            x += ImageManager.HALF_IMAGE;
                        }
                    }
                    break;
                }

                case LeftCorner:
                case RightCorner:
                {
                    bool leftCorner = renderInfo.section.SectionType == LeftCorner;

                    double angleDeg = dist / (double)ImageManager.IMAGE_SIZE * 90.0;
                    if (leftCorner)
                    {
                        angleDeg = 90 - angleDeg;
                    }
                    angleDeg = angleDeg + (double)renderInfo.dir - (leftCorner ? 270 : 180);

                    double angleRad = ImageManager.ToRadians(angleDeg);
                    double radius = leftSide != leftCorner ? 100 : 20;

                    var dir = renderInfo.dir;
                    if (dir == EAST || (dir == SOUTH && leftCorner) || (dir == NORTH && !leftCorner))
                    {
                        x += ImageManager.IMAGE_SIZE;
                    }

                    if (dir == SOUTH || (dir == WEST && leftCorner) || (dir == EAST && !leftCorner))
                    {
                        y += ImageManager.IMAGE_SIZE;
                    }

                    x += (int)(Math.Sin(angleRad) * radius)      - ImageManager.HALF_SKATER;
                    y += (int)(Math.Cos(angleRad) * radius) * -1 - ImageManager.HALF_SKATER;

                    break;
                }
            }
            return (x, y);
        }

        public static Bitmap DrawTrack(Track track, Race race)
        {
            Bitmap bitmap = (Bitmap)ImageManager.GetEmptyTrackBitmap(track, race).Clone();
            Graphics g = Graphics.FromImage(bitmap);
            
            Queue<PlayerRenderer> renderQueue = new();

            TrackIterator iter = new(track, race);
            foreach (var el in iter.Iterator())
            {
                if (el.data.Left is not null || el.data.Right is not null)
                {
                    renderQueue.Enqueue(new PlayerRenderer(el.point, el.section, el.data, el.dir, el.shouldMirror));
                }
            }

            while (renderQueue.Count > 0)
            {
                PlayerRenderer renderInfo = renderQueue.Dequeue();

                if (ImageManager.tunnelSections.Contains(renderInfo.section))
                {
                    continue;
                }

                int x = renderInfo.sectionPos.X;
                int y = renderInfo.sectionPos.Y;

                (Bitmap? left, Bitmap? right) = GetSkaterBitmaps(renderInfo);

                lock (Race.updateLock)
                {
                    if (renderInfo.data.Left is not null)
                    {
                        (x, y) = ComputePositionForInfo(renderInfo, leftSide: true);

                        g.DrawImage(left, x, y);
                        if (renderInfo.data.Left.Equipment.IsBroken)
                        {
                            g.DrawImage(ImageManager.GetBitmapData(broken), x, y + ImageManager.HALF_SKATER);
                        }
                    }
                }

                x = renderInfo.sectionPos.X;
                y = renderInfo.sectionPos.Y;

                lock (Race.updateLock)
                {
                    if (renderInfo.data.Right is not null)
                    {
                        (x, y) = ComputePositionForInfo(renderInfo, leftSide: false);

                        g.DrawImage(right, x, y);
                        if (renderInfo.data.Right.Equipment.IsBroken)
                        {
                            g.DrawImage(ImageManager.GetBitmapData(broken), x, y + ImageManager.HALF_SKATER);
                        }
                    }
                }
            }

            return bitmap;
        }

        private static (Bitmap? left, Bitmap? right) GetSkaterBitmaps(PlayerRenderer renderInfo)
        {
            Bitmap? left = null;
            if (renderInfo.data.Left is not null)
            {
                left = ImageManager.GetBitmapData(string.Format(skater, renderInfo.data.Left.TeamColor.ToString().ToLower()));
            }

            Bitmap? right = null;
            if (renderInfo.data.Right is not null)
            {
                right = ImageManager.GetBitmapData(string.Format(skater, renderInfo.data.Right.TeamColor.ToString().ToLower()));
            }

            return (left, right);
        }
    }
}
