using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Inspection
{
	public class CameraPage : ContentPage
	{
		public delegate void PhotoResultEventHandler(PhotoResultEventArgs result);

		public event PhotoResultEventHandler OnPhotoResult;

		public void SetPhotoResult(byte[] image, int width = -1, int height = -1, string imagePath = "")
		{
			OnPhotoResult?.Invoke(new PhotoResultEventArgs(image, width, height, imagePath));
		}

		public void Cancel()
		{
			OnPhotoResult?.Invoke(new PhotoResultEventArgs());
		}
	}

	public class PhotoResultEventArgs : EventArgs
	{

		public PhotoResultEventArgs()
		{
			Success = false;
		}

		public PhotoResultEventArgs(byte[] image, int width, int height, string imagePath)
		{
			Success = true;
			Image = image;
			Width = width;
			Height = height;
			ImagePath = imagePath;
		}

		public byte[] Image { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public bool Success { get; private set; }
		public string ImagePath { get; private set; }
	}
}
