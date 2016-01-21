using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SecondApp
{
    /*
     * !!!При первом запуске файл входных данных генерируется автоматически!!!
     * 
     * В файле хранятся координаты вершин четырехугольника в порядке обхода фигуры по часовой стрелке в виде:
     * <координата x1> <координата y1>
     * <координата x2> <координата y2>
     * <координата x3> <координата y3>
     * <координата x4> <координата y4>
     * Считаем, что полученные из файла вершины гарантированно образуют выпуклый четырехугольник. 
     * Написать программу, которая считывает координаты из файла. При запуске ждет от пользователя ввода 
     * координат некой точки и выводит один из четырех возможных результатов: 
     * точка внутри четырехугольника
     * точка лежит на сторонах четырехугольника
     * точка - вершина четырехугольника
     * точка снаружи четырехугольника
     */
    class Program
    {
        static void Main(string[] args)
        {
            int[,] coords = GetRect();
            int[] point = new int[2];
            Console.Write("Enter x: ");
            point[0] = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter y: ");
            point[1] = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("-------------------");

            if (isSamePoint(coords, point)) {
                Console.WriteLine("Точка - вершина четырехугольника.");
                Console.ReadKey();
                return;
            }

            bool inRectangle = false;
            for (int i = 0; i < coords.GetLength(0); i++) {
                
                int[,] tmp = TrimArray(i, coords);
                if (isInTriangle(tmp, point))
                {
                    inRectangle = true;
                    break;
                }
            }

            bool onSide = false;
            if (inRectangle) {
                List<int[,]> sides = getSides(coords);
                foreach (int[,] side in sides) {
                    if (isOnSide(side, point))
                    { 
                        onSide = true;
                        break;
                    }
                }
            }

            if (onSide) {
                Console.WriteLine("Точка лежит на сторонах четырехугольника.");
            }
            else if (inRectangle)
            {
                Console.WriteLine("Точка внутри четырехугольника.");
            }
            else 
            {
                Console.WriteLine("Точка снаружи четырехугольника.");
            }

            Console.ReadKey();
        }

        //являются ли точки одинаковыми
        static bool isSamePoint(int[,] coords, int[] newPoint)
        {
            bool isPresent = false;
            for (int i = 0; i < coords.GetLength(0); i++)
            {
                int[] existPoint = new int[2];
                for (int j = 0; j < coords.GetLength(1); j++)
                {
                    existPoint[j] = coords[i, j];
                }
                if (existPoint.SequenceEqual(newPoint)) 
                {
                    isPresent = true;
                    break;
                }
            }

            return isPresent;
        }

        static bool isOnSide(int[,] sides, int[] point)
        {
            return ((sides[1, 1] - sides[0, 1]) * (point[0] - sides[0, 0]) ==
                (sides[1, 0] - sides[0, 0]) * (point[1] - sides[0, 1]));
        }

        static bool isInTriangle(int[,] cd, int[] point)
        {
            int a = (cd[0, 0] - point[0]) * (cd[1, 1] - cd[0, 1]) - (cd[1, 0] - cd[0, 0]) * (cd[0, 1] - point[1]);
            int b = (cd[1, 0] - point[0]) * (cd[2, 1] - cd[1, 1]) - (cd[2, 0] - cd[1, 0]) * (cd[1, 1] - point[1]);
            int c = (cd[2, 0] - point[0]) * (cd[0, 1] - cd[2, 1]) - (cd[0, 0] - cd[2, 0]) * (cd[2, 1] - point[1]);

            if ((a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool isIntersect(int[,] A, int[,] B)
        {
            int v1 = (B[1, 0] - B[0, 0]) * (A[0, 1] - B[0, 1])
                - (B[1, 1] - B[0, 1]) * (A[0, 0] - B[0, 0]);
            int v2 = (B[1, 0] - B[0, 0]) * (A[1, 1] - B[0, 1])
                - (B[1, 1] - B[0, 1]) * (A[1, 0] - B[0, 0]);
            int v3 = (A[1, 0] - A[0, 0]) * (B[0, 1] - A[0, 1])
                - (A[1, 1] - A[0, 1]) * (B[0, 0] - A[0, 0]);
            int v4 = (A[1, 0] - A[0, 0]) * (B[1, 1] - A[0, 1])
                - (A[1, 1] - A[0, 1]) * (B[1, 0] - A[0, 0]);

            if ((v1 * v2 < 0) && (v3 * v4 < 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static int[,] swapPoints(int[,] coords, int p1, int p2) {
            int x = coords[p1, 0];
            int y = coords[p1, 1];
            coords[p1, 0] = coords[p2, 0];
            coords[p1, 1] = coords[p2, 1];
            coords[p2, 0] = x;
            coords[p2, 1] = y;

            return coords;
        }

        static void SetRect()
        {
            Random r = new Random();
            int[,] coords = new int[4, 2];
            for (int i = 0; i < coords.GetLength(0); i++)
            {
                for (int j = 0; j < coords.GetLength(1); j++)
                {
                    coords[i, j] = r.Next(0, 10);
                }
            }

            for (int i = 1; i < coords.GetLength(0); i++)
            {
                int[,] tmp = TrimArray(i, coords);
                while (isSamePoint(tmp, new int[] { coords[i, 0], coords[i, 1] }))
                {
                    coords[i, 0] = r.Next(0, 10);
                    coords[i, 1] = r.Next(0, 10);
                }
            }

            //являются ли точки на одной прямой
            while (isOnSide(coords, new int[] { coords[2,0], coords[2,1]}))
            {
                coords[2, 0] = r.Next(0, 10);
                coords[2, 1] = r.Next(0, 10);
            }

            /*
             * Проверяем каждую точку на принадлежность остальным трём.
             * Если совершилась перегенерация, то необходимо проверить
             * все точки заново.
             */
            for (int i = 0; i < coords.GetLength(0); ++i)
            {
                bool inTriangle = false;
                
                int[,] tmp = TrimArray(i, coords);
                while (isInTriangle(tmp, new int[] { coords[i, 0], coords[i, 1] }))
                {
                    coords[i, 0] = r.Next(0, 10);
                    coords[i, 1] = r.Next(0, 10);
                    inTriangle = true;
                }
                
                if (inTriangle) i = 0;
            }

            //0 ab; 1 bc; 2 cd;3 ad
            List<int[,]> sides = getSides(coords);

            bool isect12n34 = isIntersect(sides[0], sides[2]);
            bool isect23n14 = isIntersect(sides[1], sides[3]);

            if (isect12n34) coords = swapPoints(coords, 1, 2);

            if (isect23n14) coords = swapPoints(coords, 0, 1);

            try
            {
                using (StreamWriter generate = new StreamWriter("rectangle.txt"))
                {
                    for (int i = 0; i < coords.GetLength(0); i++)
                    {
                        for (int j = 0; j < coords.GetLength(1); j++)
                        {
                            generate.Write(coords[i, j] + ";");
                            Console.Write(coords[i, j] + ";");
                        }
                        generate.Write(System.Environment.NewLine);
                        Console.Write(System.Environment.NewLine);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Houston, we have a problem: " + e.Message);
            }
        }

        static List<int[,]> getSides(int[,] coords)
        {
            List<int[,]> sides = new List<int[,]>();

            int[,] tmp1 = TrimArray(3, coords);
            sides.Add(TrimArray(2, tmp1));
            int[,] tmp2 = TrimArray(0, coords);
            sides.Add(TrimArray(2, tmp2));
            int[,] tmp3 = TrimArray(0, coords);
            sides.Add(TrimArray(0, tmp3));
            int[,] tmp4 = TrimArray(1, coords);
            sides.Add(TrimArray(1, tmp4));

            return sides;
        }

        static int[,] GetRect()
        {
            int[,] coords = new int[4, 2];
            bool isInit = false;

            while (!isInit)
            {
                try
                {
                    using (StreamReader input = new StreamReader("rectangle.txt"))
                    {
                        string line;
                        isInit = true;
                        char[] separator = { ';' };

                        for (int i = 0; i < coords.GetLength(0); i++)
                        {
                            line = input.ReadLine();
                            string[] words = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            coords[i, 0] = Convert.ToInt32(words[0]);
                            coords[i, 1] = Convert.ToInt32(words[1]);
                        }
                    }
                }
                catch (FileNotFoundException e)
                {
                    SetRect();
                    Console.WriteLine("Четырехугольник сгенерирован.");
                }
            }

            return coords;
        }

        public static int[,] TrimArray(int pointToRemove, int[,] originalArray)
        {
            int[,] result = new int[originalArray.GetLength(0) - 1, originalArray.GetLength(1)];

            for (int i = 0, k = 0; i < originalArray.GetLength(0); i++)
            {
                if (pointToRemove == i)
                    continue;

                for (int j = 0; j < originalArray.GetLength(1); j++)
                {
                    result[k, j] = originalArray[i, j];
                }
                k++;
            }

            return result;
        }
    }
}
