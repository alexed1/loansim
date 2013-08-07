using LoanSim.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace LoanSim
{


    class Glob
    {
        public static int LOAN_PERIODS = 12;
        public static bool USE_DEBUG = true;

    }


    class Program
    {
        //$PROFILES = init_profiles;
        //actors, simulations = init_simulations
        //simulations[0].print(actors)

        public static void Main(string[] args)
        {
            init_profiles();
        }


        public async static void init_profiles()
        {


            //load all the profiles
            Dictionary<string, string> profiles = new Dictionary<string, string>();

            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles("../../profiles");
            StreamReader input; //= new StreamReader();
            foreach (string fileName in fileEntries)
            {
                var yaml = new YamlStream();
                using (input = File.OpenText( fileName))
                {
                    yaml.Load(input);
                }
                
                // Examine the stream
			    var mapping =
				(YamlMappingNode)yaml.Documents[0].RootNode;
			    foreach (var entry in mapping.Children)
			    {
				    Console.WriteLine(((YamlScalarNode)entry.Key));
                    Console.WriteLine(((YamlScalarNode)entry.Value));
			    }

			   
		
            }




        }




        //     #the validation process has a side effect of returning a composed array of the pay period percentages
        //   pay_period_percentages = validate_profile(profile, item)

        // #extract the name from the yaml and use it as the key for this profile
        //    main_key = profile['name']
        //      profiles[main_key] = profile

        //  #insert the pay_period percentages array into the profile
        //    profiles[main_key]['pay_period_percentages'] = pay_period_percentages

        //   end
        //return profiles;

    }

}

