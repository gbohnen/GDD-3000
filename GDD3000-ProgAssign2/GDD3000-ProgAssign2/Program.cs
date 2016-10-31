using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDD3000_ProgAssign2
{
    enum StatTypes { HEALTH, STAMINA, MANA, }

    public class Choices
    {
        // choice array dimensions
        public const int WIDTH = 6;                     // x dimension. finds cell to grab next 'room' from
        public const int HEIGHT = 6;                    // y dimension. same as x  
        public const int DEPTH = 7;                     // z dimension. depth into the array represents each choice for that node. 1st column follows 'x:y' format
        public const int ANA = 7;                       // w dimension. different data at each depth. 0-depth, holds text for options. 
                                                        // going down, holds stat changes for each choice. always 1-based lists, as zero is occupied by the choice pointer

        // choice arrays
        string[,,,] gameChoices = new string[Choices.WIDTH, Choices.HEIGHT, Choices.DEPTH, Choices.ANA];

        // stats
        Dictionary<StatTypes, int> stats = new Dictionary<StatTypes, int>();

        /// <summary>
        /// adds all strings to the array
        /// node primary line
        /// each choice line
        /// </summary>
        /// <param name="array"></param>
        public void LoadStrings()
        {
            // load prompts

            // load choices

            // load pointers

            // load base stats

            // load stat changes
        }

        /// <summary>
        /// draws the next node 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawNode(int x, int y)
        {
            // blank line
            Console.WriteLine();

            // display stats
            foreach (StatTypes stat in stats.Keys)
            {
                Console.Write(stat.ToString() + ": " + stats[stat].ToString() + "\t");
            }
            Console.Write(Environment.NewLine);

            // display current cell prompt
            Console.WriteLine(gameChoices[x, y, 0, 0]);

            // display choices
            for (int i = 1; i < ANA; i++)
            {
                Console.WriteLine(i + ": " + gameChoices[x, y, 0, i]);
            }
        }

        /// <summary>
        /// set the next node based on a given choice
        /// </summary>
        /// <param name="str"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UpdateGraph(int input, ref int x, ref int y)
        {
            string str = gameChoices[x, y, input, 0];
            int pos = str.IndexOf(":");

            UpdateStats(x, y, input);

            if (pos > 0)
            {
                x = Int32.Parse(str.Substring(0, pos));
                y = Int32.Parse(str.Substring(pos + 1));
            }
        }

        /// <summary>
        /// updates all stats based on a given choice coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private void UpdateStats(int x, int y, int z)
        {
            // update each stats
            for (int i = 0; i < ANA; i++)
            {
                stats[(StatTypes)i] += Int32.Parse(gameChoices[x, y, z, i + 1]);
            }

            // clamp
            foreach (StatTypes stat in stats.Keys)
            {
                if (stats[stat] < 0)
                    stats[stat] = 0;
                if (stats[stat] > 5)
                    stats[stat] = 5;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Choices c = new Choices();

            // current cell
            int x = 0;
            int y = 0;

            // load data
            c.LoadStrings();

            int input = -1;

            // main game loop
            do
            {
                // reset input
                input = -1;

                c.DrawNode(x, y);

                // grab input
                input = Int32.Parse(Console.ReadLine());

                // validate input
                while (input > Choices.DEPTH || input < 0)
                {
                    
                    Console.WriteLine("Please input a valid choice. " + input);
                    input = Int32.Parse(Console.ReadLine());
                }

                // parse input
                c.UpdateGraph(input, ref x, ref y);

            } while (true);

            // endgame logic

        }
    }
}
