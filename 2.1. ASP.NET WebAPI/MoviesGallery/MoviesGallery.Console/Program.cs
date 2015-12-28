using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviesGallery.Test;
using MoviesGallery.Models;

namespace MoviesGallery.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new MoviesGalleryContext();
            System.Console.WriteLine(context.Actors.Count());
        }
    }
}
