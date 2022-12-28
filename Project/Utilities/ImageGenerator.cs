using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SixLabors.ImageSharp.PixelFormats;

namespace ProjectOrigin
{
    public static class ImageGenerator
    {
        static ImageGenerator()
        {

        }

        public static void ImageTest()
        {

        }

        public static Bitmap UrlToBitmap(string url)
        {
            Image img = Image.FromFile(url);

            if (img == null)
            {
                throw new ArgumentNullException("img");
            }

            Bitmap outputImage = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(img, new Rectangle(new Point(), img.Size),
                    new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }

        public static List<Bitmap> DirectoryToBitmaps(string url)
        {
            string[] images = Directory.GetFiles(url);
            List<Bitmap> bitmaps = new List<Bitmap>();

            foreach (string img in images)
            {
                bitmaps.Add(UrlToBitmap(img));
            }

            return bitmaps;
        }

        public static Bitmap MergeTwoImages()
        {
            Image firstImage = Image.FromFile("Project\\Assets\\background.jpg");
            Image secondImage = Image.FromFile("Project\\Assets\\Mon Art\\suki.png");

            secondImage.RotateFlip(RotateFlipType.RotateNoneFlipX);

            if (firstImage == null)
            {
                throw new ArgumentNullException("firstImage");
            }

            if (secondImage == null)
            {
                throw new ArgumentNullException("secondImage");
            }

            int outputImageWidth = firstImage.Width > secondImage.Width ? firstImage.Width : secondImage.Width;
            int outputImageHeight = firstImage.Height > secondImage.Height ? firstImage.Height : secondImage.Height;

            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size),
                    new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(secondImage, new Rectangle(new Point(0, firstImage.Height - secondImage.Height), secondImage.Size),
                    new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }

        public static Bitmap MergeTwoImages(string firstImg, string secImg)
        {
            Image firstImage = Image.FromFile(firstImg);
            Image secondImage = Image.FromFile(secImg);

            //secondImage.RotateFlip(RotateFlipType.RotateNoneFlipX); 

            if (firstImage == null)
            {
                throw new ArgumentNullException("firstImage");
            }

            if (secondImage == null)
            {
                throw new ArgumentNullException("secondImage");
            }

            int outputImageWidth = firstImage.Width > secondImage.Width ? firstImage.Width : secondImage.Width;
            int outputImageHeight = firstImage.Height > secondImage.Height ? firstImage.Height : secondImage.Height;

            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size),
                    new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(secondImage, new Rectangle(new Point(0, firstImage.Height - secondImage.Height), secondImage.Size),
                    new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }

        public static Bitmap MergeTwoImages(ImageCriteria criteria)
        {
            //Get the background image
            Image backImage;
            if (criteria.IsBitmapBack)
                backImage = criteria.BImageBack;
            else
                backImage = Image.FromFile(criteria.ImageBack);

            //Get the overlay image
            Image overlayImage;
            if (criteria.IsBitmap)
                overlayImage = criteria.BImage;
            else
                overlayImage = Image.FromFile(criteria.Image);

            if (criteria.Flip)
                overlayImage.RotateFlip(RotateFlipType.RotateNoneFlipX);

            if (backImage == null)
                throw new ArgumentNullException("backImage");
            if (overlayImage == null)
                throw new ArgumentNullException("overlayImage");

            Bitmap outputImage = new Bitmap(backImage.Width, backImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            if (criteria.XWidth == 0)
                criteria.XWidth = overlayImage.Width;
            if (criteria.YHeight == 0)
                criteria.YHeight = overlayImage.Height;

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(backImage, new Rectangle(new Point(), backImage.Size),
                    new Rectangle(new Point(), backImage.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(overlayImage, new Rectangle(criteria.GetXLoc(), criteria.GetYLoc(), criteria.GetXSize(), criteria.GetYSize()),
                    new Rectangle(new Point(), overlayImage.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }

        public static Bitmap MergeTwoImages(Bitmap firstImage, string secImg)
        {
            //Image firstImage = Image.FromHbitmap(firstImg);
            Image secondImage = Image.FromFile(secImg);

            //secondImage.RotateFlip(RotateFlipType.RotateNoneFlipX);

            if (firstImage == null)
            {
                throw new ArgumentNullException("firstImage");
            }

            if (secondImage == null)
            {
                throw new ArgumentNullException("secondImage");
            }

            int outputImageWidth = firstImage.Width > secondImage.Width ? firstImage.Width : secondImage.Width;
            int outputImageHeight = firstImage.Height > secondImage.Height ? firstImage.Height : secondImage.Height;

            Bitmap outputImage = firstImage;

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(secondImage, new Rectangle(new Point(0, firstImage.Height - secondImage.Height), secondImage.Size),
                    new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }

        public static Bitmap MergeTwoImages(Bitmap firstImage, string secImg, int x, int y, bool flipped, bool centralize, double scale)
        {
            //Image firstImage = Image.FromHbitmap(firstImg);
            Image secondImage = Image.FromFile(secImg);

            if (flipped)
                secondImage.RotateFlip(RotateFlipType.RotateNoneFlipX);

            if (firstImage == null)
            {
                throw new ArgumentNullException("firstImage");
            }

            if (secondImage == null)
            {
                throw new ArgumentNullException("secondImage");
            }

            int outputImageWidth = firstImage.Width > secondImage.Width ? firstImage.Width : secondImage.Width;
            int outputImageHeight = firstImage.Height > secondImage.Height ? firstImage.Height : secondImage.Height;

            Bitmap outputImage = firstImage;

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(secondImage, new Rectangle(x, y, (int)(320 * scale), (int)(320 * scale)),
                    new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }

        public static List<Bitmap> GifOntoBackground(string background, string gif)
        {
            List<Bitmap> frames = new List<Bitmap>();
            foreach (string dir in Directory.GetFiles(gif))
            {
                frames.Add(MergeTwoImages(background, dir));
            }

            return frames;
        }

        public static List<Bitmap> GifOntoGif(List<Bitmap> mainFrames, string gif)
        {
            List<Bitmap> frames = new List<Bitmap>();
            string[] gifFrames = Directory.GetFiles(gif);
            if (mainFrames.Count >= gifFrames.Length)
            {
                for (int i = 0; i < gifFrames.Length; i++)
                {
                    frames.Add(MergeTwoImages(mainFrames[i], gifFrames[i]));
                    Console.WriteLine($"whereFRAMES {i}");
                }
            }
            else if (gifFrames.Length > mainFrames.Count)
            {
                for (int i = 0; i < mainFrames.Count; i++)
                {
                    frames.Add(MergeTwoImages(mainFrames[i], gifFrames[i]));
                    Console.WriteLine($"whereFRAMES2 {i}");
                }
            }

            return frames;
        }

        public static Bitmap PartyMenu(List<BasicMon> mons)
        {
            Image back = Image.FromFile("Project\\Assets\\UI Assets\\background.png");

            if (back == null)
                throw new ArgumentNullException("background");

            List<Bitmap> pBoxes = new List<Bitmap>();

            foreach (BasicMon m in mons)
            {
                Image monImage = Image.FromFile($"Project\\Assets\\Mon Art\\{m.Species.ToLower()}.png");
                Image box = Image.FromFile("Project\\Assets\\UI Assets\\bluebox.png");
                Image border = Image.FromFile("Project\\Assets\\UI Assets\\healthborder.png");
                Image borderBack = Image.FromFile("Project\\Assets\\UI Assets\\healthbackground.png");

                Image bar;
                if (m.HealthPercentage() > 0.5)
                    bar = Image.FromFile("Project\\Assets\\UI Assets\\healthbarG.png");
                else if (m.HealthPercentage() > 0.2)
                    bar = Image.FromFile("Project\\Assets\\UI Assets\\healthbarY.png");
                else
                    bar = Image.FromFile("Project\\Assets\\UI Assets\\healthbarR.png");

                if (monImage == null)
                    throw new ArgumentNullException("monImage");
                if (box == null)
                    throw new ArgumentNullException("partyBox");
                if (border == null)
                    throw new ArgumentNullException("HPBorder");
                if (borderBack == null)
                    throw new ArgumentNullException("HPBorderBack");
                if (bar == null)
                    throw new ArgumentNullException("HPBar");

                int outputImageWidth = box.Width;
                int outputImageHeight = box.Height;
                Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                using (Graphics graphics = Graphics.FromImage(outputImage))
                {
                    graphics.DrawImage(box, new Rectangle(new Point(), box.Size), new Rectangle(new Point(), box.Size), GraphicsUnit.Pixel);
                    graphics.DrawImage(monImage, new Rectangle(50, 40, 120, 120), new Rectangle(new Point(), monImage.Size), GraphicsUnit.Pixel);
                    graphics.DrawImage(borderBack, new Rectangle(261, 136, 400, 48), new Rectangle(new Point(), borderBack.Size), GraphicsUnit.Pixel);
                    graphics.DrawImage(bar, new Rectangle(265, 140, (int)(390 * m.HealthPercentage()), 42), new Rectangle(new Point(), bar.Size), GraphicsUnit.Pixel);
                    graphics.DrawImage(border, new Rectangle(261, 136, 400, 48), new Rectangle(new Point(), border.Size), GraphicsUnit.Pixel);

                    Font electrolizeSmall = new Font("Electrolize", 26);
                    Font electrolizeLarge = new Font("Electrolize", 36);
                    graphics.DrawString($"Lv. {m.Level}", electrolizeSmall, Brushes.Black, new PointF(55, 165));
                    graphics.DrawString($"{m.Species} {m.GenderSymbol}", electrolizeLarge, Brushes.Black, new PointF(365, 65));
                    graphics.DrawString($"HP", electrolizeSmall, Brushes.Black, new PointF(195, 140));
                    graphics.DrawString($"{m.CurrentHP} / {m.TotalHP}", electrolizeSmall, Brushes.Black, new PointF(390, 190));
                }

                pBoxes.Add(outputImage);
            }

            while (pBoxes.Count != 6)
            {
                Image box = Image.FromFile("Project\\Assets\\UI Assets\\bluebox.png");
                int extraBoxWidth = box.Width;
                int extraBoxHeight = box.Height;
                Bitmap extraBox = new Bitmap(extraBoxWidth, extraBoxHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(extraBox))
                {
                    graphics.DrawImage(box, new Rectangle(new Point(), box.Size),
                        new Rectangle(new Point(), box.Size), GraphicsUnit.Pixel);
                }
                pBoxes.Add(extraBox);
            }

            int finalImageWidth = back.Width;
            int finalImageHeight = back.Height;
            Bitmap finalImage = new Bitmap(finalImageWidth, finalImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(finalImage))
            {
                graphics.DrawImage(back, new Rectangle(new Point(), back.Size),
                        new Rectangle(new Point(), back.Size), GraphicsUnit.Pixel);
                bool right = false;
                int level = 1;
                foreach (Bitmap b in pBoxes)
                {
                    int leftCoord = 71 * level + (b.Height * (level - 1));
                    int rightCoord;
                    if (right)
                    {
                        rightCoord = 310 + b.Width;
                        right = false;
                        level++;
                    }
                    else
                    {
                        rightCoord = 155;
                        right = true;
                    }
                    graphics.DrawImage(b, new Rectangle(new Point(rightCoord, leftCoord), b.Size),
                        new Rectangle(new Point(), b.Size), GraphicsUnit.Pixel);
                }
            }

            return finalImage;
        }

        public static Bitmap PvPSoloLobby(CombatCreationTool tool)
        {
            Image back = Image.FromFile("Project\\Assets\\UI Assets\\combatlobby.png");
            if (back == null)
                throw new ArgumentNullException("background");

            Image leader = Image.FromFile("Project\\Assets\\UI Assets\\leader.png");
            if (leader == null)
                throw new ArgumentNullException("leaderImage");

            int leaderWidth = (int)(leader.Width * 0.2);
            int leaderHeight = (int)(leader.Height * 0.2);
            Bitmap leaderImage = new Bitmap(leaderWidth, leaderHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int finalImageWidth = back.Width;
            int finalImageHeight = back.Height;
            Bitmap finalImage = new Bitmap(finalImageWidth, finalImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(finalImage))
            {
                graphics.DrawImage(back, new Rectangle(new Point(), back.Size),
                        new Rectangle(new Point(), back.Size), GraphicsUnit.Pixel);

                Font electrolizeSmall = new Font("Electrolize", 56);
                Font electrolizeLarge = new Font("Electrolize", 66);

                int crownVal = 70;
                int xVal = 180;
                foreach (Team t in tool.Teams)
                {
                    if (t.MemberIDs.Count != 0)
                    {
                        graphics.DrawString($"{UserHandler.GetUser(t.MemberIDs[0]).Char.Name}", electrolizeLarge, Brushes.White, new PointF(xVal, 260));
                        if (t.MemberIDs[0] == tool.PartyLeader)
                            graphics.DrawImage(leader, new Rectangle(crownVal, 250, 100, 100), new Rectangle(new Point(), leader.Size), GraphicsUnit.Pixel);
                    }
                    else
                        graphics.DrawString($"- - - -", electrolizeLarge, Brushes.White, new PointF(xVal, 260));

                    xVal += 1170;
                    crownVal += 1170;
                }

                Brush selected = new SolidBrush(Color.FromArgb(43, 121, 195));
                Brush nonselected = Brushes.Gray;

                Brush NatBrush = selected;
                Brush CapBrush = selected;
                Brush ItemsOnBrush = selected;
                Brush ItemsOffBrush = selected;

                Brush OneBrush = nonselected;
                Brush TwoBrush = nonselected;
                Brush ThreeBrush = nonselected;
                Brush FourBrush = nonselected;
                Brush FiveBrush = nonselected;
                Brush SixBrush = nonselected;

                if (tool.NaturalLevels)
                    CapBrush = nonselected;
                else
                    NatBrush = nonselected;

                if (tool.ItemsOn)
                    ItemsOffBrush = nonselected;
                else
                    ItemsOnBrush = nonselected;

                switch (tool.MonsPerTeam)
                {
                    case 1:
                        OneBrush = selected;
                        break;
                    case 2:
                        TwoBrush = selected;
                        break;
                    case 3:
                        ThreeBrush = selected;
                        break;
                    case 4:
                        FourBrush = selected;
                        break;
                    case 5:
                        FiveBrush = selected;
                        break;
                    case 6:
                        SixBrush = selected;
                        break;
                }

                graphics.DrawString($"Natural", electrolizeSmall, NatBrush, new PointF(415, 710));
                graphics.DrawString($"/", electrolizeSmall, Brushes.White, new PointF(700, 710));
                graphics.DrawString($"Capped", electrolizeSmall, CapBrush, new PointF(770, 710));

                graphics.DrawString($"On", electrolizeSmall, ItemsOnBrush, new PointF(415, 840));
                graphics.DrawString($"/", electrolizeSmall, Brushes.White, new PointF(540, 840));
                graphics.DrawString($"Off", electrolizeSmall, ItemsOffBrush, new PointF(600, 840));

                graphics.DrawString($"6", electrolizeSmall, SixBrush, new PointF(415, 970));
                graphics.DrawString($"/", electrolizeSmall, Brushes.White, new PointF(495, 970));
                graphics.DrawString($"5", electrolizeSmall, FiveBrush, new PointF(575, 970));
                graphics.DrawString($"/", electrolizeSmall, Brushes.White, new PointF(655, 970));
                graphics.DrawString($"4", electrolizeSmall, FourBrush, new PointF(735, 970));
                graphics.DrawString($"/", electrolizeSmall, Brushes.White, new PointF(815, 970));
                graphics.DrawString($"3", electrolizeSmall, ThreeBrush, new PointF(895, 970));
                graphics.DrawString($"/", electrolizeSmall, Brushes.White, new PointF(975, 970));
                graphics.DrawString($"2", electrolizeSmall, TwoBrush, new PointF(1055, 970));
                graphics.DrawString($"/", electrolizeSmall, Brushes.White, new PointF(1135, 970));
                graphics.DrawString($"1", electrolizeSmall, OneBrush, new PointF(1215, 970));
            }

            return finalImage;
        }

        //C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\placeholder cat.png
        public static SixLabors.ImageSharp.Image<Rgba32> GifTest()
        {
            // Images that will be included in the gif. The duration is in milliseconds
            List<(string path, int duration)> images = new List<(string, int)>()
            {
                //100 => 1 second
                ("Project\\Assets\\placeholder cat.png", 150),
                ("Project\\Assets\\placeholder catflip.png", 50)
            };

            int width = 0;
            int height = 0;
            using (var image = SixLabors.ImageSharp.Image.Load(images[0].path))
            {
                width = image.Width;
                height = image.Height;
            }

            // Create a blank canvas for the gif
            var gif = new SixLabors.ImageSharp.Image<Rgba32>(width, height);
            for (int i = 0; i < images.Count; i++)
            {
                // Load image that will be added
                using (var image = SixLabors.ImageSharp.Image.Load(images[i].path))
                {
                    // Resize the image to the output dimensions
                    //image.Mutate(ctx => ctx.Resize(width, height));

                    // Set the duration of the image
                    image.Frames.RootFrame.Metadata.GetFormatMetadata(SixLabors.ImageSharp.Formats.Gif.GifFormat.Instance).FrameDelay = images[i].duration;

                    // Add the image to the gif
                    gif.Frames.InsertFrame(i, image.Frames.RootFrame);
                }
            }

            gif.Metadata.GetFormatMetadata(SixLabors.ImageSharp.Formats.Gif.GifFormat.Instance).RepeatCount = 0;
            return gif;
        }

        public static SixLabors.ImageSharp.Image<Rgba32> BattlefieldTest()
        {
            Console.WriteLine("whereA");
            List<Bitmap> monSpawnLeft = new List<Bitmap>();

            ImageCriteria criteria = new ImageCriteria();
            criteria.AddBack("Project\\Assets\\UI Assets\\battlefield.jpg")
            .AddImage("Project\\Assets\\Mon Art\\grasipup.png")
            .SetX(385)
            .SetY(420)
            .SetXWidth(420)
            .SetYHeight(420)
            .SetFlip(true)
            .SetCentralize(true);

            for (int i = 0; i < 10; i++)
            {
                if (i < 5)
                {
                    criteria.SetScale(0.2 * (i + 1));
                    monSpawnLeft.Add(MergeTwoImages(criteria));
                }
                else
                {
                    criteria.SetScale(1.0);
                    monSpawnLeft.Add(MergeTwoImages(criteria));
                }
            }

            List<Bitmap> monSpawnRight = new List<Bitmap>();
            criteria.ClearBack()
            .ClearImage()
            .AddImage("Project\\Assets\\Mon Art\\psygoat.png")
            .SetX(1520)
            .SetFlip(false);
            for (int i = 0; i < 10; i++)
            {
                criteria.AddBack(monSpawnLeft[i]);
                if (i < 5)
                {
                    criteria.SetScale(0.2 * (i + 1));
                    monSpawnRight.Add(MergeTwoImages(criteria));
                }
                else
                {
                    criteria.SetScale(1.0);
                    monSpawnRight.Add(MergeTwoImages(criteria));
                }
            }

            List<Bitmap> leftSpawnAnim = GifOntoGif(monSpawnRight, "Project\\Assets\\Animation Experiments\\LightBurst");
            Console.WriteLine("whereB");
            List<Bitmap> finalFrames = GifOntoGif(leftSpawnAnim, "Project\\Assets\\Animation Experiments\\LightBurstFlipped");
            Console.WriteLine($"whereC {finalFrames.Count}");
            List<(byte[] img, int duration)> images = new List<(byte[], int)>();
            Console.WriteLine("whereD");
            int framesCount = finalFrames.Count;
            Console.WriteLine("whereE");

            foreach (Bitmap frame in finalFrames)
            {
                using (var stream = new MemoryStream())
                {
                    frame.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    images.Add((stream.ToArray(), 10));
                }
            }

            Console.WriteLine("whereF");
            int width = 0;
            int height = 0;
            using (var image = SixLabors.ImageSharp.Image.Load(images[0].img))
            {
                width = image.Width;
                height = image.Height;
            }
            Console.WriteLine("whereG");

            // Create a blank canvas for the gif
            var gif = new SixLabors.ImageSharp.Image<Rgba32>(width, height);
            for (int i = 0; i < images.Count; i++)
            {
                Console.WriteLine("whereH");
                // Load image that will be added
                using (var image = SixLabors.ImageSharp.Image.Load(images[i].img))
                {
                    // Resize the image to the output dimensions
                    //image.Mutate(ctx => ctx.Resize(width, height));

                    // Set the duration of the image
                    image.Frames.RootFrame.Metadata.GetFormatMetadata(SixLabors.ImageSharp.Formats.Gif.GifFormat.Instance).FrameDelay = images[i].duration;

                    // Add the image to the gif
                    gif.Frames.InsertFrame(i, image.Frames.RootFrame);
                    Console.WriteLine($"Frame #{i} added");
                }
            }
            Console.WriteLine("whereI");

            gif.Metadata.GetFormatMetadata(SixLabors.ImageSharp.Formats.Gif.GifFormat.Instance).RepeatCount = 1;
            Console.WriteLine("whereJ");
            if (gif.Frames.Count > framesCount)
                gif.Frames.RemoveFrame(gif.Frames.Count - 1);
            Console.WriteLine("whereK");

            return gif;
        }

    }
}