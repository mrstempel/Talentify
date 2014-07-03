using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwIt.Project.Pattern.Utils
{
	public class ImageSize
	{
		public int Width { get; set; }
		public int Height { get; set; }
	}

	public static class Image
	{
		public static bool IsPictureLandscape(string imagePath)
		{
			System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
			var isLandscape = (image.Width > image.Height);
			image.Dispose();
			return isLandscape;
		}

		public static void SaveResize(string oldPath, string newPath, int size)
		{
			System.Drawing.Image image = System.Drawing.Image.FromFile(oldPath);
			image.FixOrientation();
			int width = 0;
			int height = 0;
			
			if (image.Width < image.Height)
			{
				width = size;
				var aspectRatio = (float)image.Size.Height / (float)image.Size.Width;
				height = Convert.ToInt32(aspectRatio*size);
			}
			else
			{
				height = size;
				var aspectRatio = (float)image.Size.Width / (float)image.Size.Height;
				width = Convert.ToInt32(aspectRatio * size);
			}

			Bitmap thumbnailBitmap = new Bitmap(width, height);
			Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
			//thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
			//thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
			//thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
			var imageRectangle = new Rectangle(0, 0, width, height);
			thumbnailGraph.DrawImage(image, imageRectangle);
			var format = ImageFormat.Png;
			//if (oldPath.ToLower().EndsWith(".gif"))
			//	format = ImageFormat.Gif;
			//if (oldPath.ToLower().EndsWith(".png"))
			//	format = ImageFormat.Png;


			thumbnailBitmap.Save(newPath, format);
			thumbnailGraph.Dispose();
			thumbnailBitmap.Dispose();
			image.Dispose();
		}

		public static void FixOrientation(this System.Drawing.Image image)
		{
			// 0x0112 is the EXIF byte address for the orientation tag
			if (!image.PropertyIdList.Contains(0x0112))
			{
				return;
			}

			// get the first byte from the orientation tag and convert it to an integer
			var orientationNumber = image.GetPropertyItem(0x0112).Value[0];

			switch (orientationNumber)
			{
				// up is pointing to the right
				case 8:
					image.RotateFlip(RotateFlipType.Rotate270FlipNone);
					break;
				// up is pointing to the bottom (image is upside-down)
				case 3:
					image.RotateFlip(RotateFlipType.Rotate180FlipNone);
					break;
				// up is pointing to the left
				case 6:
					image.RotateFlip(RotateFlipType.Rotate90FlipNone);
					break;
				// up is pointing up (correct orientation)
				case 1:
					break;
			}
		}
	}
}
