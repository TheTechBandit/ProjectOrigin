using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SixLabors.ImageSharp.PixelFormats;

namespace ProjectOrigin
{
    public class BattlefieldAnimator : BasicAnimator
    {
        public override Bitmap Background { get; set; }

        public BattlefieldAnimator()
        {

        }

        public BattlefieldAnimator(bool newanim) : base(newanim)
        {
            Background = ImageGenerator.UrlToBitmap("Project\\Assets\\UI Assets\\battlefieldhalf.jpg");
        }

        public SixLabors.ImageSharp.Image<Rgba32> ConstructAnimation()
        {
            CurrentFrame = 0;
            /* STAGE 1 
            Beginning frames
            Count: 6 */
            Frames.Add(Background);
            Frames.Add(Background);
            Frames.Add(Background);
            Frames.Add(Background);
            Frames.Add(Background);
            CurrentFrame = Frames.Count;

            /* STAGE 2
            Mon spawn left & right side
            Count: 9 */
            // Set the static criteria for the left mon
            Keyframes.Add((CurrentFrame, "Spawning"));
            Criteria.AddImage("Project\\Assets\\Mon Art\\grasipup.png")
            .SetX(192)
            .SetY(210)
            .SetXWidth(210)
            .SetYHeight(210)
            .SetFlip(true)
            .SetCentralize(true);

            // Calculate and add each left frame
            for (int i = 0; i < 12; i++)
            {
                if (i < 5)
                {
                    Criteria.SetScale(0.2 * (i + 1));
                    AddImageToFrame(Criteria, CurrentFrame);
                }
                else
                {
                    Criteria.SetScale(1.0);
                    AddImageToFrame(Criteria, CurrentFrame);
                }
                CurrentFrame++;
            }

            ReturnToLastKeyframe();

            // Set the static criteria for the right mon
            Criteria.ClearImage()
            .AddImage("Project\\Assets\\Mon Art\\psygoat.png")
            .SetX(760)
            .SetFlip(false);

            // Calculate and add each right frame
            for (int i = 0; i < 12; i++)
            {
                if (i < 5)
                {
                    Criteria.SetScale(0.2 * (i + 1));
                    AddImageToFrame(Criteria, CurrentFrame);
                }
                else
                {
                    Criteria.SetScale(1.0);
                    AddImageToFrame(Criteria, CurrentFrame);
                }
                CurrentFrame++;
            }

            ReturnToLastKeyframe();
            CurrentFrame -= 2;

            Criteria.DefaultReset()
            .SetXWidth(960)
            .SetYHeight(540);
            AddAllImagesToFrames(Criteria, ImageGenerator.DirectoryToBitmaps("Project\\Assets\\Animation Experiments\\LightBurst"), CurrentFrame);

            ReturnToLastKeyframe();
            CurrentFrame -= 2;

            AddAllImagesToFrames(Criteria, ImageGenerator.DirectoryToBitmaps("Project\\Assets\\Animation Experiments\\LightBurstFlipped"), CurrentFrame);

            /* STAGE 3
            Mon idle left & right side
            (squish x stretch y then back to normal)
            Count: */

            /* GIF COMPLIATION
            Converts a List<Bitmap> into a byte[] array to be used in the gif compiler.
            Each image is also given a duration.
            The number of frames is saved in framesCount and checked against the number of frames in the final compiled gif for error correction purposes. */
            List<(byte[] img, int duration)> images = new List<(byte[], int)>();
            int framesCount = Frames.Count;
            foreach (Bitmap frame in Frames)
            {
                using (var stream = new MemoryStream())
                {
                    frame.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    images.Add((stream.ToArray(), Speed));
                }
            }

            // Determine the width and height of the gif
            int width = Frames[0].Width;
            int height = Frames[0].Height;

            // Create a blank canvas for the gif
            var gif = new SixLabors.ImageSharp.Image<Rgba32>(width, height);
            for (int i = 0; i < images.Count; i++)
            {
                // Load image that will be added
                using (var image = SixLabors.ImageSharp.Image.Load(images[i].img))
                {
                    // Set the duration of the image
                    image.Frames.RootFrame.Metadata.GetFormatMetadata(SixLabors.ImageSharp.Formats.Gif.GifFormat.Instance).FrameDelay = images[i].duration;

                    // Add the image to the current Frame
                    gif.Frames.InsertFrame(i, image.Frames.RootFrame);
                }
            }

            // Set the gif to 1 loop (no repeat)
            gif.Metadata.GetFormatMetadata(SixLabors.ImageSharp.Formats.Gif.GifFormat.Instance).RepeatCount = 1;

            // Remove the final frame if the original frame count and current frame count aren't the same (sometimes an extra black frame is added- not sure why)
            if (gif.Frames.Count > framesCount)
                gif.Frames.RemoveFrame(gif.Frames.Count - 1);

            return gif;
        }

    }
}