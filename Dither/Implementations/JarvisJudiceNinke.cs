﻿using LlamaCorp.ImageProcessing.Attributes;
using LlamaCorp.ImageProcessing.Dither.Entities;
using LlamaCorp.ImageProcessing.Dither.Enums;
using LlamaCorp.ImageProcessing.Dither.Interfaces;

namespace LlamaCorp.ImageProcessing.Dither.Implementations
{
  [EnumLink(typeof(DitherAlgorithm), (int)DitherAlgorithm.JarvisJudiceNinke)]
  public class JarvisJudiceNinke : IDither
  {
    public void PerformWork(Image image, Parameters parameters = null)
    {
      if (parameters == null)
      {
        parameters = new Parameters();
      }

      for (var x = 0; x < image.Width; x++)
      {
        for (var y = 0; y < image.Height; y++)
        {
          var pixel = image.GetPixel(x, y);
          var avg = (pixel.R * parameters.R_Multiplier + pixel.G * parameters.G_Multiplier + pixel.B * parameters.B_Multiplier);

          avg += pixel.Error;

          int error = 0;

          if (avg < parameters.Threshold)
          {
            error = (int)(avg);
            avg = 0;
          }
          else
          {
            error = (int)(avg - 255);
            avg = 255;
          }

          var errorMultiplier = ((double)error / 48.0);
          
          // x + 1, y
          image.SetPixelError(x + 1, y, errorMultiplier * 7);

          // x + 2, y
          image.SetPixelError(x + 2, y, errorMultiplier * 5);

          // x - 2, y + 1
          image.SetPixelError(x - 2, y + 1, errorMultiplier * 3);

          // x - 1, y + 1
          image.SetPixelError(x - 1, y + 1, errorMultiplier * 5);

          // x, y + 1
          image.SetPixelError(x, y + 1, errorMultiplier * 7);

          // x + 1, y + 1
          image.SetPixelError(x + 1, y + 1, errorMultiplier * 5);

          // x + 2, y + 1
          image.SetPixelError(x + 2, y + 1, errorMultiplier * 3);

          // x - 2, y + 2
          image.SetPixelError(x - 2, y + 2, errorMultiplier * 1);

          // x - 1, y + 2
          image.SetPixelError(x - 1, y + 2, errorMultiplier * 3);

          // x, y + 2
          image.SetPixelError(x, y + 2, errorMultiplier * 5);

          // x + 1, y + 2
          image.SetPixelError(x + 1, y + 2, errorMultiplier * 3);

          // x + 2, y + 2
          image.SetPixelError(x + 2, y + 2, errorMultiplier * 1);

          image.SetPixel(x, y, new Pixel()
          {
            R = (byte)avg,
            G = (byte)avg,
            B = (byte)avg,
            A = 255,
          });
        }
      }
    }
  }
}