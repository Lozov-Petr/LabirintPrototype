using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{

    enum TypePrimitives
    {
        LINE,
        RECTANGLE,
        CIRCLE,
        TRIANGLE,
        FILL_RECTRANGLE,
        FILL_CIRCLE,
        FILL_TRIANGLE,
        CIRCLE_WITH_WIDTH
    }
    
    class Primitive
    {
        public int startIndex;
        public int count;
        public TypePrimitives typePrimitives;

        public Primitive()
        {
            //int startIndex, int count, int typePrimitives
        }
    }

    static class Primitives
    {

        private static float ZBuffer = 0;

        private static List<Primitive> primitives = new List<Primitive>();
        private static VertexPositionColor[] vertices = new VertexPositionColor[196605];
        private static int index = 0;

        private static int Virtual_width = 400;
        private static int Virtual_height = 300;
        private static Vector3 vect1;
        private static Vector3 vect2;
        private static Vector3 vect3;
        private static Vector3 vect4;

        /// <summary>
        /// Кол-во отрисованных "вершин"
        /// </summary>
        /// <remarks> Максимальное кол-во: 65535 </remarks>
        public static int count_index=0;

        /// <summary>
        /// Кол-во отрисованных "линий"
        /// </summary>
        /// <remarks> Максимальное кол-во: 65535 </remarks>
        public static int count_line=0;
        private static int temp_line=0;

        /// <summary>
        /// Кол-во отрисованных "прямоугольников"
        /// </summary>
        /// <remarks> Максимальное кол-во: 16383 </remarks>
        public static int count_rectangle=0;
        private static int temp_rectangle=0;

        /// <summary>
        /// Кол-во отрисованных "кругов"
        /// </summary>
        /// <remarks> Максимальное кол-во: 1310 </remarks>
        public static int count_circle=0;
        private static int temp_circle=0;

        /// <summary>
        /// Кол-во отрисованных "треугольников"
        /// </summary>
        /// <remarks> Максимальное кол-во: 21845 </remarks>
        public static int count_triangle=0;
        private static int temp_triangle=0;

        /// <summary>
        /// Кол-во отрисованных "закрашенных прямоугольников"
        /// </summary>
        /// <remarks> Максимальное кол-во: 21845 </remarks>
        public static int count_rectanglefill=0;
        private static int temp_rectanglefill=0;

        /// <summary>
        /// Кол-во отрисованных "закрашенных кругов"
        /// </summary>
        /// <remarks> Максимальное кол-во: 1872 </remarks>
        public static int count_circlefill=0;
        private static int temp_circlefill=0;

        /// <summary>
        /// Кол-во отрисованных "закрашенных кругов с изменяемой шириной"
        /// </summary>
        /// <remarks> Максимальное кол-во: 1872 </remarks>
        public static int count_circlefillwidth=0;
        private static int temp_circlefillwidth=0;

        /// <summary>
        /// Кол-во отрисованных "закрашенных треугольников"
        /// </summary>
        /// <remarks> Максимальное кол-во: 65535 </remarks>
        public static int count_trianglefill=0;
        private static int temp_trianglefill=0;

        private static BasicEffect effect;
        /// <summary>
        /// Инициализация класса примитивов
        /// </summary>
        /// <param name="gd">Графическое устройство</param>
        /// <param name="width">Ширина виртуального окна</param>
        /// <param name="height">Высота виртуального окна</param>
        public static void Init(GraphicsDevice gd, int width, int height)
        {
            Virtual_width = width / 2;
            Virtual_height = height / 2;
            effect = new BasicEffect(gd);
            effect.VertexColorEnabled = true;
        }

        /// <summary>
        /// Преобразование экранных координат к "текстурным"
        /// </summary>
        /// <param name="vector">Экранные координаты</param>
        /// <returns>Возвращет текстурные координаты</returns>
        private static Vector3 convertCoordinates3(Vector3 vector)
        {
            return new Vector3((vector.X - Virtual_width) / Virtual_width, -(vector.Y - Virtual_height) / Virtual_height, vector.Z);
        }

        /// <summary>
        /// Поворот вектора относительно оси вращения
        /// </summary>
        /// <param name="vector">Вектор для вращения</param>
        /// <param name="orign">Ось вращения</param>
        /// <param name="rotation">Угол поворота в радианах</param>
        /// <returns>Возвращет повернутый вектор</returns>
        private static Vector3 rotateVector3(Vector3 vector, Vector2 orign, float rotation)
        {
            Vector3 t_orign = new Vector3(orign.X, orign.Y, 0);
            Vector3 v1_temp = vector - t_orign; // временное значение для наглядности
            Matrix matrixRotation = Matrix.CreateRotationZ(rotation);
            return Vector3.Transform(v1_temp, matrixRotation) + t_orign;// итоговый вектор относительно позиции базового спрайта
        }

        /// <summary>
        /// Добавить для отрисовки примитив "линия"
        /// </summary>
        /// <param name="point1">Первая точка линии (задается экранными координатами)</param>
        /// <param name="point2">Вторая точка линии (задается экранными координатами)</param>
        /// <param name="color">Цвет линии</param>
        public static void AddLine(Vector2 point1, Vector2 point2, Color color)
        {
            
            
            if (index <= 131068)
            {
                ZBuffer += 0.001f;

                vertices[index    ] = new VertexPositionColor(convertCoordinates3(new Vector3(point1.X, point1.Y, ZBuffer)), color);
                vertices[index + 1] = new VertexPositionColor(convertCoordinates3(new Vector3(point2.X, point2.Y, ZBuffer)), color);
               
                Primitive primitive = new Primitive();
                primitive.typePrimitives = TypePrimitives.LINE;
                primitive.startIndex = index;
                primitive.count = 1;

                index += 2;

                primitives.Add(primitive);
                temp_line++;
            }
        }

        /// <summary>
        /// Добавить для отрисовки примитив "прямоугольник"
        /// </summary>
        /// <param name="rectangle">Этим параметром задается положение и размер прямоугольника</param>
        /// <param name="color">Цвет прямоугольника</param>
        public static void AddRectangle(Rectangle rectangle, Color color)
        {
            AddRectangle(rectangle, 0f, color);
        }

        /// <summary>
        /// Добавить для отрисовки примитив "прямоугольник"
        /// </summary>
        /// <param name="rectangle">Этим параметром задается положение и размер прямоугольника</param>
        /// <param name="rotate">Угол поворота в радианах (вращение производится вокруг центра фигуры)</param>
        /// <param name="color">Цвет прямоугольника</param>
        public static void AddRectangle(Rectangle rectangle, float rotate, Color color)
        {
            if (index <= 131062)
            {
                ZBuffer += 0.001f;
                
                Vector2 orign = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);

                vect1 = rotateVector3(new Vector3(rectangle.X, rectangle.Y, ZBuffer), orign, rotate);
                vect2 = rotateVector3(new Vector3(rectangle.X + rectangle.Width, rectangle.Y, ZBuffer), orign, rotate);
                vect3 = rotateVector3(new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, ZBuffer), orign, rotate);
                vect4 = rotateVector3(new Vector3(rectangle.X, rectangle.Y + rectangle.Height, ZBuffer), orign, rotate);

                vertices[index    ] = new VertexPositionColor(convertCoordinates3(vect1), color);
                vertices[index + 1] = new VertexPositionColor(convertCoordinates3(vect2), color);
                vertices[index + 2] = new VertexPositionColor(convertCoordinates3(vect3), color);
                vertices[index + 3] = new VertexPositionColor(convertCoordinates3(vect4), color);
                vertices[index + 4] = new VertexPositionColor(convertCoordinates3(vect1), color);

                Primitive primitive = new Primitive();
                primitive.typePrimitives = TypePrimitives.RECTANGLE;
                primitive.startIndex = index;
                primitive.count = 4;
                primitives.Add(primitive);

                index += 5;
                temp_rectangle++;
            }
        }

        /// <summary>
        /// Добавить для отрисовки примитив "закрашенный прямоугольник"
        /// </summary>
        /// <param name="rectangle">Этим параметром задается положение и размер прямоугольника</param>
        /// <param name="color">Цвет прямоугольника</param>
        public static void AddRectangleFill(Rectangle rectangle, Color color)
        {
            AddRectangleFill(rectangle, 0f, color);
        }

        /// <summary>
        /// Добавить для отрисовки примитив "закрашенный прямоугольник"
        /// </summary>
        /// <param name="rectangle">Этим параметром задается положение и размер прямоугольника</param>
        /// <param name="rotate">Угол поворота в радианах (вращение производится вокруг центра фигуры)</param>
        /// <param name="color">Цвет прямоугольника</param>
        public static void AddRectangleFill(Rectangle rectangle, float rotate, Color color)
        {
            if (index <= 131064)
            {
                ZBuffer += 0.001f;

                Vector2 orign = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);

                vect1 = rotateVector3(new Vector3(rectangle.X, rectangle.Y, ZBuffer), orign, rotate);
                vect2 = rotateVector3(new Vector3(rectangle.X + rectangle.Width, rectangle.Y, ZBuffer), orign, rotate);
                vect3 = rotateVector3(new Vector3(rectangle.X, rectangle.Y + rectangle.Height, ZBuffer), orign, rotate);
                vect4 = rotateVector3(new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, ZBuffer), orign, rotate);

                vertices[index    ] = new VertexPositionColor(convertCoordinates3(vect1), color);
                vertices[index + 1] = new VertexPositionColor(convertCoordinates3(vect2), color);
                vertices[index + 2] = new VertexPositionColor(convertCoordinates3(vect3), color);
                vertices[index + 3] = new VertexPositionColor(convertCoordinates3(vect4), color);

                Primitive primitive = new Primitive();
                primitive.typePrimitives = TypePrimitives.FILL_RECTRANGLE;
                primitive.startIndex = index;
                primitive.count = 2;
                primitives.Add(primitive);

                index += 4;
                temp_rectanglefill++;
            }
        }

        /// <summary>
        /// Добавить для отрисовки примитив "круг"
        /// </summary>
        /// <param name="point">Положение круга (центр окружности)</param>
        /// <param name="radius">Радиус окружности</param>
        /// <param name="color">Цвет окружности</param>
        public static void AddCircle(Vector2 point, int radius, Color color)
        {

            if (index <= 130970)
            {
                int countStep = radius;
                if (radius < 10) countStep = 10;
 
                ZBuffer += 0.001f;

                Vector3 temp_point = convertCoordinates3(new Vector3(point.X, point.Y, 0));
                Vector2 rad = new Vector2(radius / (float)Virtual_width, radius / (float)Virtual_height);
                
                for (int i = 0; i <= countStep; ++i)
                {
                    float angle = (float)(i * Math.PI * 2 / (float)countStep);
                    vertices[index + i] = new VertexPositionColor(new Vector3(temp_point.X + (float)Math.Cos(angle) * rad.X, temp_point.Y + (float)Math.Sin(angle) * rad.Y, ZBuffer), color);
                }

                Primitive primitive = new Primitive();
                primitive.typePrimitives = TypePrimitives.CIRCLE;
                primitive.startIndex = index;
                primitive.count = countStep;
                primitives.Add(primitive);

                index += countStep + 1;
                temp_circle++;
            }
        }

        /// <summary>
        /// Добавить для отрисовки примитив "круг"
        /// </summary>
        /// <param name="point">Положение круга (центр окружности)</param>
        /// <param name="radius">Радиус окружности</param>
        /// <param name="color">Цвет окружности</param>
        public static void AddCircle(Vector2 point, int radius, int width, Color color)
        {
            AddCircle(point, radius, width, color, color);
        }

        /// <summary>
        /// Добавить для отрисовки примитив "круг"
        /// </summary>
        /// <param name="point">Положение круга (центр окружности)</param>
        /// <param name="radius">Радиус окружности</param>
        /// <param name="color_border">Цвет края окружности</param>
        /// <param name="color_center">Цвет центра окружности</param>
        public static void AddCircle(Vector2 point, int radius, int width, Color color_border, Color color_center)
        {
            if (index <= 196500)
            {

                int countStep = radius;
                if (radius < 10) countStep = 10;

                if (width > radius) width = radius;
                
                ZBuffer += 0.001f;

                Vector3 temp_point = convertCoordinates3(new Vector3(point.X, point.Y, 0));

                Vector2 rad1 = new Vector2(radius / (float)Virtual_width, radius / (float)Virtual_height);
                Vector2 rad2 = new Vector2((radius - width) / (float)Virtual_width, (radius - width) / (float)Virtual_height);
                
                
                for (int i = 0; i <= countStep; ++i)
                {
                    float angle = (float)(i * Math.PI * 2 / (float)countStep);

                    float cos = (float)Math.Cos(angle);
                    float sin = (float)Math.Sin(angle);

                    vertices[index + 2 * i    ] = new VertexPositionColor(new Vector3(temp_point.X + cos * rad1.X, temp_point.Y + sin * rad1.Y, ZBuffer), color_border);
                    vertices[index + 2 * i + 1] = new VertexPositionColor(new Vector3(temp_point.X + cos * rad2.X, temp_point.Y + sin * rad2.Y, ZBuffer), color_center);                                            
                }
                
                Primitive primitive = new Primitive();
                primitive.typePrimitives = TypePrimitives.CIRCLE_WITH_WIDTH;
                primitive.startIndex = index;
                primitive.count = 2 * countStep;
                primitives.Add(primitive);

                index += 2 * countStep + 2;
                temp_circlefillwidth++;
            }
        }

        /// <summary>
        /// Добавить для отрисовки примитив "закрашенный круг"
        /// </summary>
        /// <param name="point">Положение круга (центр окружности)</param>
        /// <param name="radius">Радиус окружности</param>
        /// <param name="color">Цвет окружности</param>
        public static void AddCircleFill(Vector2 point, int radius, Color color)
        {
            AddCircleFill(point, radius, color, color);
        }

        /// <summary>
        /// Добавить для отрисовки примитив "закрашенный круг"
        /// </summary>
        /// <param name="point">Положение круга (центр окружности)</param>
        /// <param name="radius">Радиус окружности</param>
        /// <param name="color_border">Цвет края окружности</param>
        /// <param name="color_center">Цвет центра окружности</param>
        public static void AddCircleFill(Vector2 point, int radius, Color color_border, Color color_center)
        {
            AddCircle(point, radius, radius, color_border, color_center);
        }

        /// <summary>
        /// Добавить для отрисовки примитив "треугольник"
        /// </summary>
        /// <param name="point1">Первый угол треугольника</param>
        /// <param name="point2">Второй угол треугольника</param>
        /// <param name="point3">Третий угол треугольника</param>
        /// <param name="color">Цвет треугольника</param>
        public static void AddTriangle(Vector2 point1, Vector2 point2, Vector2 point3, Color color)
        {
            if (index <= 131064)
            {   
                ZBuffer += 0.001f;

                vect1 = convertCoordinates3(new Vector3(point1.X, point1.Y, ZBuffer));
                vect2 = convertCoordinates3(new Vector3(point2.X, point2.Y, ZBuffer));
                vect3 = convertCoordinates3(new Vector3(point3.X, point3.Y, ZBuffer));

                vertices[index    ] = new VertexPositionColor(vect1, color);
                vertices[index + 1] = new VertexPositionColor(vect2, color);
                vertices[index + 2] = new VertexPositionColor(vect3, color);
                vertices[index + 3] = new VertexPositionColor(vect1, color);

                Primitive primitive = new Primitive();
                primitive.typePrimitives = TypePrimitives.TRIANGLE;
                primitive.startIndex = index;
                primitive.count = 3;
                primitives.Add(primitive);

                index += 4;
                temp_triangle++;
            }
        }

        /// <summary>
        /// Добавить для отрисовки примитив "закрашенный треугольник"
        /// </summary>
        /// <param name="point1">Первый угол треугольника</param>
        /// <param name="point2">Второй угол треугольника</param>
        /// <param name="point3">Третий угол треугольника</param>
        /// <param name="color">Цвет треугольника</param>
        public static void AddTriangleFill(Vector2 point1, Vector2 point2, Vector2 point3, Color color)
        {
            if (index <= 196602)
            {             
                ZBuffer += 0.001f;

                vect1 = convertCoordinates3(new Vector3(point1.X, point1.Y, ZBuffer));
                vect2 = convertCoordinates3(new Vector3(point2.X, point2.Y, ZBuffer));
                vect3 = convertCoordinates3(new Vector3(point3.X, point3.Y, ZBuffer));

                vertices[index    ] = new VertexPositionColor(vect1, color);
                vertices[index + 1] = new VertexPositionColor(vect2, color);
                vertices[index + 2] = new VertexPositionColor(vect3, color);

                Primitive primitive = new Primitive();
                primitive.typePrimitives = TypePrimitives.FILL_TRIANGLE;
                primitive.startIndex = index;
                primitive.count = 1;
                primitives.Add(primitive);

                index += 3;
                temp_trianglefill++;
            }
        }

        /// <summary>
        /// Отрисовка приготовленных примитивов
        /// </summary>
        /// <param name="batch">ссылка на рабочий spriteBatch</param>
        public static void Draw(ref GraphicsDeviceManager graphics, ref SpriteBatch batch, bool CullModeNone = true, bool WireFrame = false)
        {
            effect.CurrentTechnique.Passes[0].Apply();

            RasterizerState rasterizerState = new RasterizerState();
            if (CullModeNone) rasterizerState.CullMode = CullMode.None; else rasterizerState.CullMode = CullMode.CullClockwiseFace;
            if (WireFrame) rasterizerState.FillMode = FillMode.WireFrame; else rasterizerState.FillMode = FillMode.Solid;
            graphics.GraphicsDevice.RasterizerState = rasterizerState;

            foreach (Primitive primitive in primitives)
            {

                if (primitive.typePrimitives == TypePrimitives.LINE) //Линия
                {
                    batch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, primitive.startIndex, primitive.count);
                }
                if (primitive.typePrimitives == TypePrimitives.RECTANGLE) //Прямоугольник
                {
                    batch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, primitive.startIndex, primitive.count);
                }
                if (primitive.typePrimitives == TypePrimitives.CIRCLE) //Круг
                {
                    batch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, primitive.startIndex, primitive.count);
                }
                if (primitive.typePrimitives == TypePrimitives.TRIANGLE) //Треугольник
                {
                    batch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, primitive.startIndex, primitive.count);
                }
                if (primitive.typePrimitives == TypePrimitives.FILL_RECTRANGLE) //Прямоугольник с заливкой
                {
                    batch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, primitive.startIndex, primitive.count);
                }
                if (primitive.typePrimitives == TypePrimitives.FILL_CIRCLE) //Круг с заливкой
                {
                    batch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, primitive.startIndex, primitive.count);
                }
                if (primitive.typePrimitives == TypePrimitives.CIRCLE_WITH_WIDTH) //Круг с толщиной
                {
                    batch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, primitive.startIndex, primitive.count);
                }
                if (primitive.typePrimitives == TypePrimitives.FILL_TRIANGLE) //Треугольник с заливкой
                {
                    batch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, primitive.startIndex, primitive.count);
                }
            }

            count_line = temp_line;
            count_rectangle = temp_rectangle;
            count_circle = temp_circle;
            count_triangle = temp_triangle;
            count_rectanglefill = temp_rectanglefill;
            count_circlefill = temp_circlefill;
            count_circlefillwidth = temp_circlefillwidth;
            count_trianglefill = temp_trianglefill;

            temp_circle = 0;
            temp_circlefill = 0;
            temp_circlefillwidth = 0;
            temp_line = 0;
            temp_rectangle = 0;
            temp_rectanglefill = 0;
            temp_triangle = 0;
            temp_trianglefill = 0;

            count_index = index;
            ZBuffer = 0;
            index = 0;

            primitives.Clear();

        }

    }
}
