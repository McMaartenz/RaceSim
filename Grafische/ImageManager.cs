using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace Grafische
{
    using static Visualisation.Orientation;
    using static Model.SectionType;

    public static class ImageManager
    {
        public const int IMAGE_SIZE = 128;
        public const int HALF_IMAGE = 64;
        public const int SKATER_SIZE = 48;

        public static int trackWidth;
        public static int trackHeight;

        public static Dictionary<string, Bitmap> imageCache = new();

        public static Bitmap GetBitmapData(string textureName)
        {
            bool success = imageCache.TryGetValue(textureName, out Bitmap? result);
            if (!success || result is null)
            {
                try
                {
                    if (textureName.Equals("empty"))
                    {
                        Bitmap bitmap = new(trackWidth, trackHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                        return bitmap;
                    }

                    result = new(textureName);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"File {textureName} is not found: {e}", "Missing data file", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new Exception($"Missing file {textureName} because of {e}");
                }
            }

            return result;
        }

        public static void FlushCache()
        {
            imageCache.Clear();
        }

        public static (int, int) CalculateTrackDimensions(Track track)
        {
            Visualisation.Orientation orientation = EAST;

            int x    = 0,
                y    = 0,
                minX = 0,
                maxX = 0,
                minY = 0,
                maxY = 0;

            foreach (Section section in track.Sections)
            {
                x += orientation == EAST ? 1 : (orientation == WEST ? -1 : 0);
                y += orientation == SOUTH ? 1 : (orientation == NORTH ? -1 : 0);

                if (x > maxX) { maxX = x; }
                if (x < minX) { minX = x; }

                if (y > maxY) { maxY = y; }
                if (y < minY) { minY = y; }

                switch (section.SectionType)
                {
                    case LeftCorner:
                    {
                        Dictionary<Visualisation.Orientation, Visualisation.Orientation> orientationMappings = new()
                        {
                            { NORTH, WEST },
                            { WEST, SOUTH },
                            { SOUTH, EAST },
                            { EAST, NORTH },
                        };

                        orientation = orientationMappings[orientation];
                        break;
                    }

                    case RightCorner:
                    {
                        Dictionary<Visualisation.Orientation, Visualisation.Orientation> orientationMappings = new()
                        {
                            { NORTH, EAST },
                            { EAST, SOUTH },
                            { SOUTH, WEST },
                            { WEST, NORTH },
                        };

                        orientation = orientationMappings[orientation];
                        break;
                    }

                    default:
                    {
                        break;
                    }
                }
            }

            return (Math.Abs(maxX - minX) + 1, Math.Abs(maxY - minY) + 1);
        }

        public static (int, int) CalculateTrackPixelDimensions(Track track)
        {
            (int w, int h) = CalculateTrackDimensions(track);
            return (w * IMAGE_SIZE, h * IMAGE_SIZE);
        }

        public static void CreateEmptyImage(int width, int height)
        {
            imageCache.Add("empty", new(width, height));
        }

        public static Bitmap CreateImage(int width, int height)
        {
            return new(width, height);
        }

        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using MemoryStream memory = new();

            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
