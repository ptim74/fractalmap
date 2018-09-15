using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FractalMap.Logic;
using Microsoft.AspNetCore.Mvc;

namespace FractalMap.Controllers
{
    //[ResponseCache(Duration = 86400)]
    [Route("api/[controller]")]
    [ApiController]
    public class TilesController : ControllerBase
    {
        [HttpGet("mandel1/{z}/{x}/{y}.png")]
        public IActionResult Mandel1(int z, long x, long y)
        {
            return Mandel(z, x, y, 25);
        }

        [HttpGet("mandel2/{z}/{x}/{y}.png")]
        public IActionResult Mandel2(int z, long x, long y)
        {
            return Mandel(z, x, y, 50);
        }

        [HttpGet("mandel3/{z}/{x}/{y}.png")]
        public IActionResult Mandel3(int z, long x, long y)
        {
            return Mandel(z, x, y, 100);
        }

        [HttpGet("mandel4/{z}/{x}/{y}.png")]
        public IActionResult Mandel4(int z, long x, long y)
        {
            return Mandel(z, x, y, 200);
        }

        [HttpGet("julia1/{z}/{x}/{y}.png")]
        public IActionResult Julia1(int z, long x, long y)
        {
            return Julia(z, x, y, -0.4, 0.6, 25);
        }

        [HttpGet("julia2/{z}/{x}/{y}.png")]
        public IActionResult Julia2(int z, long x, long y)
        {
            return Julia(z, x, y, 0.285, 0.01, 50);
        }

        [HttpGet("julia3/{z}/{x}/{y}.png")]
        public IActionResult Julia3(int z, long x, long y)
        {
            return Julia(z, x, y, -0.70176, -0.3842, 25);
        }

        [HttpGet("julia4/{z}/{x}/{y}.png")]
        public IActionResult Julia4(int z, long x, long y)
        {
            return Julia(z, x, y, -0.8, 0.156, 200);
        }

        [HttpGet("julia5/{z}/{x}/{y}.png")]
        public IActionResult Julia5(int z, long x, long y)
        {
            return Julia(z, x, y, -0.7269, 0.1889, 200);
        }

        protected IActionResult Mandel(int z, long x, long y, int shades)
        {
            return FractalImage(new Mandelbrot(1000, true, 16.0), z, x, y, shades);
        }

        protected IActionResult Julia(int z, long x, long y, double cx, double cy, int shades)
        {
            return FractalImage(new Julia(cx, cy, 1000, true, 16.0), z, x, y, shades);
        }

        protected IActionResult FractalImage(IFractal fractal, int z, long x, long y, int shades)
        {
            var generator = new TileGenerator(fractal, shades, z, x, y);
            generator.Palette = DefaultPalette();
            return File(generator.GetImageStream(), "image/png");
        }

        protected List<Color> DefaultPalette()
        {
            return new List<Color>
            {
                Color.Navy,
                Color.White,
                Color.Maroon
            };
        }
    }
}
