using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inspection
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Camera : ContentPage
	{
		public Camera(bool view, string imgUrl)
		{
			InitializeComponent();
			if (!view)
				BtnCamera_Clicked();
			else
				Photo.Source = imgUrl;
		}

		async void BtnCamera_Clicked()
		{
			var cameraPage = new CameraPage();
			cameraPage.OnPhotoResult += CameraPage_OnPhotoResult;
			await Navigation.PushModalAsync(cameraPage);
		}

		async void CameraPage_OnPhotoResult(Inspection.PhotoResultEventArgs result)
		{
			await Navigation.PopModalAsync();
			if (!result.Success)
				return;
			Photo.Source = result.ImagePath;
		}

		private void Btn_Click_Back(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new AuditQuestionAnswers((int)App.Current.Properties["AuditID"], (bool)App.Current.Properties["IsNewAudit"]));
		}		
	}
}