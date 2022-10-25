using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using Controller;

namespace Grafische
{
    public static class ImageManager
    {
        public const int IMAGE_SIZE = 128;
        public const int HALF_IMAGE = 64;

        public const int SKATER_SIZE = 48;
        public const int HALF_SKATER = 24;

        public static int trackWidth;
        public static int trackHeight;

        public static Dictionary<string, Bitmap> imageCache = new();
        public static List<Section> tunnelSections = new();

        public static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public static Bitmap GetEmptyTrackBitmap(Track track, Race race)
        {
            bool success = imageCache.TryGetValue("emptytrack", out Bitmap? result);
            if (!success || result is null)
            {
                Bitmap bitmap = Visualisation.DrawEmptyTrack(track, race);
                imageCache.Add("emptytrack", bitmap);
                                
                return bitmap;
            }

            return result;
        }

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
                    imageCache.Add(textureName, result);
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
            int x,
                y,
                minX = 0,
                maxX = 0,
                minY = 0,
                maxY = 0;

            tunnelSections = new();
            Dictionary<(int x, int y), Section> posToSection = new();
            Visualisation.TrackIterator iter = new(track, Data.CurrentRace);
            foreach (var el in iter.Iterator())
            {
                x = el.point.X;
                y = el.point.Y;

                bool success = posToSection.TryAdd((x, y), el.section);
                if (!success)
                {
                    tunnelSections.Add(posToSection[(x, y)]);
                }

                maxX = Math.Max(x, maxX);
                minX = Math.Min(x, minX);

                maxY = Math.Max(y, maxY);
                minX = Math.Min(y, minY);

                Console.WriteLine($"current piece point {el.section.SectionType}: {el.dir},({x},{y})");
            }

            return (Math.Abs(maxX - minX) + 1, Math.Abs(maxY - minY) + 1);
        }

        public static (int w, int h) CalculateTrackPixelDimensions(Track track)
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

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        /* Bitmap -> BitmapSource met native code
         * https://stackoverflow.com/questions/6484357/converting-bitmapimage-to-bitmap-and-vice-versa */

        public static BitmapSource ToBitmapSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource retval;

            try
            {
                retval = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()
                );
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }
    }
}
