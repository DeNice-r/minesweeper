using System;
using System.Collections.Generic;
using System.Threading;

namespace saper
{
    class Program
    {
        static Thread kevtmgr = new Thread(keyManager);
        static public char isgameon = 'n';
        static List<List<Slot>> map = new List<List<Slot>>();
        static UInt16 curx = 0, cury = 0, sx = 20, sy = 15;
        public static Random rnd = new Random();
        public static bool cheat = false;
        public static char emptysymb = '·';
        public static UInt16[] counter = new UInt16[2] { 0, 0 };
        private static DateTime estime = DateTime.Now;

        static void Main(string[] args)
        {
            curx = Convert.ToUInt16(rnd.Next(0, sx));
            cury = Convert.ToUInt16(rnd.Next(0, sy));

            UInt16 diff = 30;
            counter[0] = diff;

            mapGen(diff);

            isgameon = 'y';
            kevtmgr.Start();
            redraw();
        }

        static public void mapGen(UInt16 diff)
        {
            map = new List<List<Slot>>();

            for(int x = 0; x < sx; x++)
            {
                map.Add(new List<Slot>());
                for (int y = 0; y < sy; y++)
                {
                    map[x].Add(new Slot());
                }
            }

            while(diff > 0)
            {
                var x = new int[]{ rnd.Next(0, sx), rnd.Next(0, sy) };
                if (map[x[0]][x[1]].bimg == emptysymb)
                {
                    map[x[0]][x[1]].bimg = '&';
                    diff--;
                }
            }

            for (int x = 0; x < sx; x++)
            {
                map.Add(new List<Slot>());
                for (int y = 0; y < sy; y++)
                {
                    if (map[x][y].bimg == '&')
                        continue;
                    var t = 0;
                    if(x > 0 && map[x - 1][y].bimg == '&')
                        t++;
                    if (y > 0 && map[x][y - 1].bimg == '&')
                        t++;
                    if (x > 0 && y > 0 && map[x - 1][y - 1].bimg == '&')
                        t++;
                    if (x < sx - 1 && map[x + 1][y].bimg == '&')
                        t++;
                    if (y < sy - 1 && map[x][y + 1].bimg == '&')
                        t++;
                    if (x < sx - 1 && y < sy - 1 && map[x + 1][y + 1].bimg == '&')
                        t++;
                    if (x > 0 && y < sy - 1 && map[x - 1][y + 1].bimg == '&')
                        t++;
                    if (x < sx - 1 && y > 0 && map[x + 1][y - 1].bimg == '&')
                        t++;
                    if(t != 0)
                        map[x][y].bimg = Convert.ToChar('0' + t);
                }
            }
        }

        static private void keyManager()
        {
            while (true)
            {
                if (isgameon == 'y')
                {
                    var key = Console.ReadKey(true);

                    switch (key.Key)
                    {
                        case ConsoleKey.W:
                            if (cury > 0)
                                cury--;
                            break;
                        case ConsoleKey.S:
                            if (cury < sy - 1)
                                cury++;
                            break;
                        case ConsoleKey.A:
                            if (curx > 0)
                                curx--;
                            break;
                        case ConsoleKey.D:
                            if (curx < sx - 1)
                                curx++;
                            break;
                        case ConsoleKey.Q:
                            map[curx][cury].qpress();
                            if(counter[0] - counter[1] == 0)
                            {

                            }
                            break;
                        case ConsoleKey.E:
                            recursivewipe(curx, cury);
                            break;
                        case ConsoleKey.F5:
                            cheat = !cheat;
                            break;
                        case ConsoleKey.R:
                            if (map[curx][cury].fimg == '█')
                                map[curx][cury].epress();
                            if (curx > 0 && map[curx - 1][cury].fimg == '█')
                                map[curx - 1][cury].epress();
                            if (cury > 0 && map[curx][cury - 1].fimg == '█')
                                map[curx][cury - 1].epress();
                            if (curx > 0 && cury > 0 && map[curx - 1][cury - 1].fimg == '█')
                                map[curx - 1][cury - 1].epress();
                            if (curx < sx - 1 && map[curx + 1][cury].fimg == '█')
                                map[curx + 1][cury].epress();
                            if (cury < sy - 1 && map[curx][cury + 1].fimg == '█')
                                map[curx][cury + 1].epress();
                            if (curx < sx - 1 && cury < sy - 1 && map[curx + 1][cury + 1].fimg == '█')
                                map[curx + 1][cury + 1].epress();
                            if (curx > 0 && cury < sy - 1 && map[curx - 1][cury + 1].fimg == '█')
                                map[curx - 1][cury + 1].epress();
                            if (curx < sx - 1 && cury > 0 && map[curx + 1][cury - 1].fimg == '█')
                                map[curx + 1][cury - 1].epress();
                            break;
                    }
                    redraw();
                }
                else
                    Thread.Sleep(250);
            }

        }

        public static void recursivewipe(UInt16 startx, UInt16 starty)
        {
            map[startx][starty].epress();
            if(map[startx][starty].fimg == emptysymb)
            {
                if (startx > 0 && map[startx - 1][starty].fimg != emptysymb)
                    recursivewipe(Convert.ToUInt16(startx - 1), starty);
                if (startx < sx - 1 && map[startx + 1][starty].fimg != emptysymb)
                    recursivewipe(Convert.ToUInt16(startx + 1), starty);
                if (starty > 0 && map[startx][starty - 1].fimg != emptysymb)
                    recursivewipe(startx, Convert.ToUInt16(starty - 1));
                if (starty < sy - 1 && map[startx][starty + 1].fimg != emptysymb)
                    recursivewipe(startx, Convert.ToUInt16(starty + 1));
            }
        }

        static private void redraw()
        {
            Console.Clear();
            if (isgameon == 'l')
            {
                Console.WriteLine("You lost :(");
                for (int y = 0; y < sy; y++)
                {
                    for (int x = 0; x < sx; x++)
                    {
                        if (map[x][y].fimg == 'f' && map[x][y].bimg == '&')
                            Console.ForegroundColor = ConsoleColor.Green;
                        else if(map[x][y].bimg == '&' || map[x][y].bimg == 'f')
                            Console.ForegroundColor = ConsoleColor.Red;
                        if (map[x][y].fimg == 'f' && map[x][y].bimg != '&')
                            ;
                        else
                            map[x][y].fimg = map[x][y].bimg;
                        Console.Write(map[x][y].fimg + " ");
                        Console.ResetColor();
                    }
                    Console.Write("\n");
                }
            }
            else if (isgameon == 'w')
            {
                for (int y = 0; y < sy; y++)
                {
                    for (int x = 0; x < sx; x++)
                    {
                        if (map[x][y].bimg == '&')
                            Console.ForegroundColor = ConsoleColor.Green;
                        map[x][y].fimg = map[x][y].bimg;
                        Console.Write(map[x][y].fimg + " ");
                        Console.ResetColor();
                    }
                    Console.Write("\n");
                }
                Console.WriteLine("You won :)");
            }
            else
            {
                var c = counter[0];
                for (int y = 0; y < sy; y++)
                {
                    for (int x = 0; x < sx; x++)
                    {
                        if (counter[0] == counter[1] && map[x][y].fimg == 'f' && map[x][y].bimg == '&')
                            c--;
                        if (x == curx || y == cury)
                            Console.ForegroundColor = ConsoleColor.Blue;
                        if (x == curx && y == cury)
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(map[x][y].fimg + " ");
                        Console.ResetColor();
                    }
                    Console.Write("\n");
                }
                if (c == 0)
                {
                    isgameon = 'w';
                    redraw();

                }
                Console.ResetColor();
                Console.Write("Mines left: " + (counter[0] - counter[1]) + "\tTime passed: " + (DateTime.Now.Minute * 60 + DateTime.Now.Second - estime.Minute * 60 - estime.Second));
                if (cheat == true)
                {
                    for (int y = 0; y < sy; y++)
                    {
                        for (int x = 0; x < sx; x++)
                        {
                            if (x == curx || y == cury)
                                Console.ForegroundColor = ConsoleColor.Blue;
                            if (x == curx && y == cury)
                                Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(map[x][y].bimg + " ");
                            Console.ResetColor();
                        }
                        Console.Write("\n");
                    }
                    Console.ResetColor();
                }
            }
        }
    }

    class Slot
    {
        public char fimg = '█';
        public char bimg = Program.emptysymb;

        public void qpress()
        {
            switch (fimg)
            {
                case '█': fimg = 'f'; Program.counter[1]++; break;
                case 'f': fimg = '?'; Program.counter[1]--; break;
                case '?': fimg = '█'; break;
            }
        }

        public void epress()
        {
            if(fimg == '█')
            {
                if (bimg == '&')
                    Program.isgameon = 'l';
                fimg = bimg;
            }
        }
    }
}