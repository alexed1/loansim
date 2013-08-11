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


    //this allows us to create a dictionary where one of the values is a dictionary. Seems messy, but other approaches were problematic.
    class ProfileDictionary : Dictionary<string, string>
    {
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



       

        //loads actor profiles from yaml
        public  static void init_profiles()
        {

            //The profiles dictionary is a hash where the key is the name of an individual profile, and the value is a dictionary containing that profile's data
            Dictionary<string, ProfileDictionary> profiles = new Dictionary<string,ProfileDictionary>();
            
            //Load the yaml profiles found in the directory. 
            string[] fileEntries = Directory.GetFiles("../../profiles");
            StreamReader input; //= new StreamReader();
            foreach (string fileName in fileEntries)
            {

                //yaml data for an individual profile will be loaded into a dictionary
                ProfileDictionary profile = new ProfileDictionary();

                //yaml library processes the yaml files
                var yaml = new YamlStream();        
                input = File.OpenText( fileName);
                yaml.Load(input);       
			    var node = (YamlMappingNode)yaml.Documents[0].RootNode;

                // convert the yaml input into a dictionary object
			    foreach (var entry in node.Children)
			    {
                    //string foo = ((YamlScalarNode)entry.Key).Value;
                    profile.Add(((YamlScalarNode)entry.Key).Value,((YamlScalarNode)entry.Value).Value);
				    
			    }

                //     #the validation process has a side effect of returning a composed array of the pay period percentages
                string[] pay_period_percentages = validate_profile(profile, fileName);

                // this profile will be stored in a dictionary of profiles.
                // extract the profile's name from the yaml and use it as the key for this profile
                string profile_key = profile["name"];
                profiles.Add(profile_key, profile);
                var i = 0;

            }
        }

        public static string[] validate_profile(Dictionary<string, string> profile, string fileName) {

            string[] foo = new string[1];
            
          Console.WriteLine( "validating profile: "  + profile["name"]);

           
          return foo;

        }
        
          


       



        //  #insert the pay_period percentages array into the profile
        //    profiles[main_key]['pay_period_percentages'] = pay_period_percentages

        //   end
        //return profiles;

    }

}

