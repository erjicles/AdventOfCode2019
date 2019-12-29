using AdventOfCode2019.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Challenges.Day08
{
    /// <summary>
    /// Solution to the Day 8 challenge:
    /// https://adventofcode.com/2019/day/8
    /// </summary>
    public class Day08
    {
        public const string FILE_NAME = "Day08Input.txt";

        public static int GetDay8Part1Answer()
        {
            // To make sure the image wasn't corrupted during transmission, 
            // the Elves would like you to find the layer that contains the 
            // fewest 0 digits. On that layer, what is the number of 1 digits 
            // multiplied by the number of 2 digits?
            // Answer: 2375
            var encodedImage = GetDay8Input();
            var layers = GetImageLayers(encodedImage, 25, 6);
            var layerWithFewestDigitsIndex = GetIndexOfLayerWithFewestDigits(layers, 0);
            var layerWithFewestDigits = layers[layerWithFewestDigitsIndex];
            var numberOfDigits1 = layerWithFewestDigits
                .Where(d => d == 1)
                .Count();
            var numberOfDigits2 = layerWithFewestDigits
                .Where(d => d == 2)
                .Count();
            var result = numberOfDigits1 * numberOfDigits2;
            return result;
        }

        public static IList<int> RunDay8Part2()
        {
            // What message is produced after decoding your image?
            // Answer: RKHRY
            var imageWidth = 25;
            var imageHeight = 6;
            var encodedImage = GetDay8Input();
            var layers = GetImageLayers(encodedImage, imageWidth, imageHeight);
            var decodedImage = DecodeLayersIntoFinalImage(layers);
            Console.WriteLine();
            for (int rowIndex = 0; rowIndex < imageHeight; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < imageWidth; columnIndex++)
                {
                    var pixelIndex = (imageWidth * rowIndex) + columnIndex;
                    var pixel = decodedImage[pixelIndex];
                    Console.Write(pixel);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            return decodedImage;
        }

        public static IList<int> DecodeLayersIntoFinalImage(IList<IList<int>> layers)
        {
            // Now you're ready to decode the image. The image is rendered by 
            // stacking the layers and aligning the pixels with the same 
            // positions in each layer. The digits indicate the color of the 
            // corresponding pixel: 0 is black, 1 is white, and 2 is transparent.
            // The layers are rendered with the first layer in front and the 
            // last layer in back.So, if a given position has a transparent 
            // pixel in the first and second layers, a black pixel in the third 
            // layer, and a white pixel in the fourth layer, the final image 
            // would have a black pixel at that position.
            if (layers == null || layers.Count == 0)
                throw new Exception("No layers provided");
            var result = new List<int>();
            var layerLength = layers[0].Count;
            for (int pixelIndex = 0; pixelIndex < layerLength; pixelIndex++)
            {
                int pixelValue = -1;
                for (int layerIndex = 0; layerIndex < layers.Count; layerIndex++)
                {
                    pixelValue = layers[layerIndex][pixelIndex];
                    if (pixelValue == 0
                        || pixelValue == 1)
                        break;
                }
                if (pixelValue == -1)
                    throw new Exception("Pixel value unassigned");
                result.Add(pixelValue);
            }
            return result;
        }

        public static int GetIndexOfLayerWithFewestDigits(IList<IList<int>> imageLayers, int digit)
        {
            int minDigitCount = int.MaxValue;
            int layerWithFewestDigitsIndex = -1;
            for (int i = 0; i < imageLayers.Count; i++)
            {
                var layer = imageLayers[i];
                var digitCount = layer.Where(d => d == digit).Count();
                if (digitCount < minDigitCount)
                {
                    minDigitCount = digitCount;
                    layerWithFewestDigitsIndex = i;
                }
            }
            return layerWithFewestDigitsIndex;
        }

        public static IList<IList<int>> GetImageLayers(string encodedImage, int width, int height)
        {
            var pixelsPerLayer = width * height;
            if (encodedImage.Length % pixelsPerLayer != 0)
                throw new Exception("Encoded image size does not meet height/width requirements");
            var layers = new List<IList<int>>();
            for (int i = 0; i < encodedImage.Length; i += pixelsPerLayer)
            {
                var layer = encodedImage.Substring(i, pixelsPerLayer)
                    .ToCharArray()
                    .Select(c => int.Parse(c.ToString()))
                    .ToList();
                layers.Add(layer);
            }
            return layers;
        }

        public static string GetDay8Input()
        {
            return FileHelper.ReadInputFileAsString(FILE_NAME);
        }
    }
}
