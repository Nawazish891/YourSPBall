﻿using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourSPBall.Interface;
using YourSPBall.Models;
using YourSPBall.Resources;
using YourSPBall.SpinPaint;

namespace YourSPBall.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpinPaintScreen : ContentPage
    {
        public SpinPaintScreen(SPBall spBall)
        {
            InitializeComponent();
            SPBall = spBall;
            Init();
        }

        #region ---- Properties ----
        private SPBall _SPBall;
        public SPBall SPBall
        {
            get
            {
                return _SPBall;
            }
            set
            {
                _SPBall = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ----Spin Paint Implementation ----

        void Init()
        {
            SKGLView canvasView = new SKGLView();
            bitmap = SKBitmap.Decode(SPBall.ImageURL);
            bitmap = bitmap.Resize(new SKSizeI(544,544), SKFilterQuality.High);
            bitmapCanvas = new SKCanvas(bitmap);
            bitmapCanvas.DrawBitmap(bitmap, 0,0);
            bitmapSize = Math.Min(bitmap.Height, bitmap.Width);
            canvasView.PaintSurface += OnGLViewPaintSurface;
            this.canvasView = canvasView;
            canvasViewLayout.Children.Add(canvasView);

            // Start animation
            stopwatch.Start();
            Device.StartTimer(TimeSpan.FromMilliseconds(16), OnTimerTick);
        }
        // These should be contrasting colors
        static readonly SKColor backgroundColor = SKColors.Transparent;
        static readonly SKColor crossHairColor = SKColors.Transparent;

        // Either SKCanvasView or SKGLView
        View canvasView;

        // Current bitmap being drawn upon by user
        SKBitmap bitmap;
        SKCanvas bitmapCanvas;
        int bitmapSize;                 // bitmaps used here are always square

        // SKPaint for user drawings on bitmap
        SKPaint fingerPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
        };

        // SKPaint for crosshairs on bitmap
        SKPaint thinLinePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1,
            Color = crossHairColor
        };

        // SKPath for clipping drawings to circle
        SKPath clipPath = new SKPath();

        // Animation helpers
        Stopwatch stopwatch = new Stopwatch();
        float angle;

        // Item to store in touch-tracking dictionary
        class FingerInfo
        {
            public Point ThisPosition;
            public SKPoint LastPosition;
        }

        // Touch-tracking dictionary for tracking multiple fingers
        Dictionary<long, FingerInfo> idDictionary = new Dictionary<long, FingerInfo>();

        // For each touch event, simply store information in idDictionary.
        // Do not draw at this time!
        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (args.IsInContact)
                    {
                        idDictionary.Add(args.Id, new FingerInfo
                        {
                            ThisPosition = args.Location,
                            LastPosition = new SKPoint(float.PositiveInfinity, float.PositiveInfinity)
                        });
                    }
                    break;

                case TouchActionType.Moved:
                    if (idDictionary.ContainsKey(args.Id))
                    {
                        idDictionary[args.Id].ThisPosition = args.Location;
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (idDictionary.ContainsKey(args.Id))
                    {
                        idDictionary.Remove(args.Id);
                    }
                    break;
            }
        }

        // Every 1/60th second, update bitmap with user drawings
        bool OnTimerTick()
        {
            if (bitmap == null)
            {
                return true;
            }

            // Determine the current color.
            float tColor = stopwatch.ElapsedMilliseconds % 10000 / 10000f;
            fingerPaint.Color = SKColor.FromHsl(360 * tColor, 100, 50);
            titleLabel.TextColor = fingerPaint.Color.ToFormsColor();

            // Determine the rotation angle.
            float tAngle = stopwatch.ElapsedMilliseconds % 5000 / 5000f;
            angle = 360 * tAngle;
            SKMatrix matrix = SKMatrix.MakeRotationDegrees(-angle, bitmap.Width / 2, bitmap.Height / 2);

            // Loop trough the fingers touching the screen.
            foreach (long id in idDictionary.Keys)
            {
                FingerInfo fingerInfo = idDictionary[id];

                // Get the canvas size in pixels. It's square so it's only one number.
                float canvasSize = 0;

                if (canvasView is SKCanvasView)
                {
                    canvasSize = (canvasView as SKCanvasView).CanvasSize.Width;
                }
                else
                {
                    canvasSize = (canvasView as SKGLView).CanvasSize.Width;
                }

                // Convert Xamarin.Forms coordinates to pixels for drawing on the bitmap.
                // Also, make an offset factor if there's been resizing and the bitmap
                //      is now larger than the canvas. (It's never smaller.)
                float factor = canvasSize / (float)canvasView.Width;    // scaling factor
                float offset = (bitmapSize - canvasSize) / 2;           // bitmap always >= canvas

                SKPoint convertedPoint = new SKPoint(factor * (float)fingerInfo.ThisPosition.X + offset,
                                                     factor * (float)fingerInfo.ThisPosition.Y + offset);

                // Now rotate the point based on the rotation angle
                SKPoint pt0 = matrix.MapPoint(convertedPoint);

                if (!float.IsPositiveInfinity(fingerInfo.LastPosition.X))
                {
                    // Draw four lines in four quadrants.
                    SKPoint pt1 = fingerInfo.LastPosition;
                    bitmapCanvas.DrawLine(pt0.X, pt0.Y, pt1.X, pt1.Y, fingerPaint);

                    float x0Flip = bitmap.Width - pt0.X;
                    float y0Flip = bitmap.Height - pt0.Y;
                    float x1Flip = bitmap.Width - pt1.X;
                    float y1Flip = bitmap.Height - pt1.Y;

                    bitmapCanvas.DrawLine(x0Flip, pt0.Y, x1Flip, pt1.Y, fingerPaint);
                    bitmapCanvas.DrawLine(pt0.X, y0Flip, pt1.X, y1Flip, fingerPaint);
                    bitmapCanvas.DrawLine(x0Flip, y0Flip, x1Flip, y1Flip, fingerPaint);
                }

                // Save the current point for next time through.
                fingerInfo.LastPosition = pt0;
            }

            // Redraw the canvas.
            if (canvasView is SKCanvasView)
            {
                (canvasView as SKCanvasView).InvalidateSurface();
            }
            else
            {
                (canvasView as SKGLView).InvalidateSurface();
            }
            return true;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            OnPaintSurface(args.Surface, args.Info.Width, args.Info.Height);
        }

        void OnGLViewPaintSurface(object sender, SKPaintGLSurfaceEventArgs args)
        {
            OnPaintSurface(args.Surface, args.RenderTarget.Width, args.RenderTarget.Height);
        }

        void OnPaintSurface(SKSurface surface, int width, int height)
        {
            // Get the canvas
            SKCanvas canvas = surface.Canvas;

            // These two dimensions should be the same.
            int canvasSize = Math.Min(width, height);

            // If the canvas has become larger, make a new bitmap of that size.
            if (bitmapSize < canvasSize)
            {
                // New versions of the three fields
                int newBitmapSize = canvasSize * 2;
                SKBitmap newBitmap = new SKBitmap(newBitmapSize, newBitmapSize);
                SKCanvas newBitmapCanvas = new SKCanvas(newBitmap);

                // New circular clipping and background
                PrepBitmap(newBitmapCanvas, newBitmapSize);

                // Copy old bitmap to new bitmap
                float diff = (newBitmapSize - bitmapSize) / 2f;
                newBitmapCanvas.DrawBitmap(bitmap, diff, diff);

                // Dispose old bitmap and its canvas
                //bitmapCanvas.Dispose();
                bitmap.Dispose();

                // Set fields to new values
                bitmap = newBitmap;
                bitmapCanvas = newBitmapCanvas;
                bitmapSize = newBitmapSize;
            }

            // Clear the canvas
            canvas.Clear(SKColors.Transparent);

            // Set the rotate transform
            float radius = canvasSize / 2;
            canvas.RotateDegrees(angle, radius, radius);

            // Set a circular clipping area
            clipPath.Reset();
            clipPath.AddCircle(radius, radius, radius);
            canvas.ClipPath(clipPath);

            // Draw the bitmap
            float offset = (canvasSize - bitmapSize) / 2f;
            canvas.DrawBitmap(bitmap, offset, offset);

            // Draw the cross hairs
            canvas.DrawLine(radius, 0, radius, canvasSize, thinLinePaint);
            canvas.DrawLine(0, radius, canvasSize, radius, thinLinePaint);
        }

        static void PrepBitmap(SKCanvas bitmapCanvas, int bitmapSize)
        {
            // Set clipping path based on bitmap size
            using (SKPath bitmapClipPath = new SKPath())
            {
                bitmapClipPath.AddCircle(bitmapSize / 2, bitmapSize / 2, bitmapSize / 2);
                bitmapCanvas.ClipPath(bitmapClipPath);
            }

            // Color the bitmap background
            bitmapCanvas.Clear(backgroundColor);
        }

        // Clear the bitmap of all user drawings.
        void OnClearButtonClicked(object sender, EventArgs args)
        {
            // Color the bitmap background, erasing all user drawing
            bitmapCanvas.Clear(backgroundColor);
        }

        // Save the bitmap to the phone's shared photo library
        async void OnSaveButtonClicked(object sender, EventArgs args)
        {
            // Get this SKImage data. This is basically an array of bytes in PNG format.
            SKData data = SKImage.FromBitmap(bitmap).Encode();

            // Fabricate a filename based on data and time.
            // NOTE: This filename is not used by iOS!
            DateTime dt = DateTime.Now;
            string filename = String.Format("SpinPaint-{0:D4}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}{6:D3}.png",
                                            dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

            //// Get the dependency service implemented in the particular platform.
            //ISpinPaintDependencyService dependencyService = DependencyService.Get<ISpinPaintDependencyService>();

            //// Save the bitmap and get a boolean indicating success.
            //bool result = await dependencyService.SaveBitmap(data.ToArray(), filename);

            //// Display an alert if an error occurred.
            //if (!result)
            //{
            //    await DisplayAlert("SpinPaint", "Artwork could not be saved. Sorry!", "OK");
            //}
        }
        #endregion

        #region ----Commands----
        public Command ClearPaintCommand
        {
            get
            {
                return new Command(async () =>
                {
                    App.IconClicked();

                    string action = await DisplayActionSheet(AppResources.ClearPaintMsg, AppResources.Cancel, null, new string[] { AppResources.No, AppResources.Yes });

                    if (action != AppResources.Yes)
                        return;

                    this.Navigation.PopAsync();
                });
            }
        }

        public Command SavePaintCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.IconClicked();
                    SKData data = SKImage.FromBitmap(bitmap).Encode();
                    SPBall.ImageURL = DependencyService.Get<IFileService>().SavePicture(SPBall.ImageURL,new MemoryStream(data.ToArray()));
                    this.Navigation.PopAsync();
                });
            }
        }
        #endregion
    }
}