using LoanSim.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.RepresentationModel;
using System.Runtime.Serialization.Json;

namespace LoanSim
{


    class Glob
    {
        public static int LOAN_PERIODS = 12;
        public static bool USE_DEBUG = true;
        public static string path = "../../profiles";
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
            string[] fileEntries = Directory.GetFiles(Glob.path);
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
                int[] pay_period_percentages = validate_profile(profile, fileName);

                // this profile will be stored in a dictionary of profiles.
                // extract the profile's name from the yaml and use it as the key for this profile
                string profile_key = profile["name"];
                profiles.Add(profile_key, profile);
           
                //insert the pay_period percentages array into the profile
                //PAUSE this won't work because it needs to be serialized
                MemoryStream stream1 = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(int[]));
                ser.WriteObject(stream1, pay_period_percentages);
                profiles[profile_key]["pay_period_percentages"] = stream1.ToString();

            }
        }

        public static int[] validate_profile(Dictionary<string, string> profile, string fileName) {

          string[] foo = new string[1];
            
          Console.WriteLine( "validating profile: "  + profile["name"]);
          if (profile["config_type"] != "profile")
              throw new ApplicationException( "all entries must be config_type = profile");
          if (Convert.ToInt32(profile["pct_theft"]) > 100 || Convert.ToInt32(profile["pct_theft"])< 0)
              throw new ApplicationException( "theft percentage must be between 0 and 100");
          if (Convert.ToInt32(profile["average_item_price"])> 2000 || Convert.ToInt32(profile["average_item_price"]) < 25)
              throw new ApplicationException("item price must be between 25 and 2000");
          if (fileName != Glob.path + "\\"+ profile["name"] + ".yml")
              throw new ApplicationException("we don't allow the filename and the 'name' key to have different values, because misery will result");
           
          //make sure the percentages add up to 1, and store them all in a convenient array while we're at it
          int total = 0;
          int[] pay_period_probabilities = new int[12];
          for (int i = 1; i< Glob.LOAN_PERIODS; i++) {
              //all array operations ignore index 0 to match things up nicely with the periods
               string key = "pct_complete_" + Convert.ToString(i) + "_payment";
               total += Convert.ToInt32(profile[key]);
               pay_period_probabilities[i] = total;
          };

          return pay_period_probabilities;

        }
        
          


       




    }

}

