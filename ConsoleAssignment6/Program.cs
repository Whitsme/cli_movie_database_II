// Aaron Whitaker
// Winter 2022
// CIS 207
// Console Assignment 6: JSON Movies Collection Program Advanced

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace ConsoleAssignment6
{
    /*
     * Class 'Program' calls class 'MovieCollection' as an object, creates custom constructor myMovies and passes "jsonFolder" into 'MovieCollection', requests user input for what method to call, 
     *  calls said method via object 'MovieCollection', and finally calls method 'SaveAllJSON' (while passing var 'jsonFolder') to save all edits to JSON files 
     *  required inputs: JSON folder, class 'MovieCollection', and user input for method selection 
     *  expected outputs: var'jsonFolder' to constructor 'MovieCollection', user selected method, and finally method 'SaveAllJSON' with var 'jsonFolder' passed to said method 
     *  no returned values  
    */
    internal class Program
    {
        /*
         * method 'Main' is called when the program is initialized 
         * it imports the JSON folder location, creates custom constructor myMovies and passes "jsonFolder" into 'MovieCollection', requests user input for what method to call, calls said method via object 'MovieCollection', 
         *  and finally calls method 'SaveAllJSON' (while passing var 'jsonFolder') to save all edits to JSON files 
         * required inputs: JSON folder, class 'MovieCollection', and user input for method selection 
         * expected outputs: var'jsonFolder' to constructor 'MovieCollection', user selected method, and finally method 'SaveAllJSON' with var 'jsonFolder' passed to said method 
         * no returned values 
        */
        static void Main(string[] args)
        {
            // comment below is for hard coding JSON folder name and why there is and if statement pertaining to it 
            string jsonFolder = "";// = "JSON";
            if (jsonFolder == "")
            {
                Console.WriteLine("Welcome, please enter the folder name containing your JSON files, or enter only to exit:");
                jsonFolder = Console.ReadLine();
                Console.WriteLine("");
                bool validFolder = false;
                // exits program if nothing input // checks user input JSON folder to see if it contains any files and re-requests folder name if not
                if (jsonFolder == "") { Environment.Exit(0); }
                else
                {
                    jsonFolder = Convert.ToString(jsonFolder);
                    string[] testFiles = Directory.GetFiles(jsonFolder);
                    if (testFiles.Length == 0)
                    {
                        Console.WriteLine("\nThe folder " + jsonFolder + " does not contain any files.");
                        validFolder = false;
                    }
                    else { validFolder = true; }
                }
                while (!validFolder) ;
            }
            MovieCollection myMovies = new MovieCollection(jsonFolder) { };
            Console.WriteLine("\nPlease choose from the below (you may also hit enter to exit):");
            Console.WriteLine("1: I want to add or update the movie ratings");
            Console.WriteLine("2: I want to add a movie");
            Console.WriteLine("3: I want to delete a movie");
            Console.WriteLine("4: Exit program\n");
            bool validMenu = false;
            while (!validMenu)
            {

                var menuChoice = Console.ReadLine();
                // if statement checks for valid user input, prints error if invalid, and re-queries user
                if (Convert.ToString(menuChoice) == "1") { validMenu = true; myMovies.UpdateRating(); }
                else if (Convert.ToString(menuChoice) == "2") { validMenu = true; myMovies.AddMovie(); }
                else if (Convert.ToString(menuChoice) == "3") { validMenu = true; myMovies.DeleteMovies(); }
                else if (Convert.ToString(menuChoice) == "4") { Environment.Exit(0); }
                else if (Convert.ToString(menuChoice) == "") { Environment.Exit(0); }
                else { Console.WriteLine("Invalid selection. Please enter 1-5 only or press enter to exit.\n"); }
            }
            myMovies.SaveAllJSON(jsonFolder);
        }
    }
    /*
     * Class 'MovieCollection' creates list object using class 'Movies', constructor 'MovieCollection' is called from class 'Program' and is passed 'jsonDirectory' (see string 'jsonFolder' in class 'Program), 
     *  user selected method below (except 'InportJSON' or 'SaveAllJSON') is called from class 'Program', method 'SaveAllJSON' is automatically called from class 'Program'  
     * required inputs: JSON folder , class 'Movies', and user input for method selection along with any input required by chosen method 
     * expected outputs: edits to JSON movie files are saved either back to modified file, a new file if adding a movie, or the deletion of a file if deleting a movie 
     * no returned values other than adding items to list object using class 'Movies'  
    */
    class MovieCollection
    {
        List<Movies> theMovies = new List<Movies> { };
        string jsonPath;
        /*
         * called from method 'Main' class 'Program'
         * this constructor method's function is to read the JSON files in JSON folder and add them to list 'theMovies'
         * requires user input of JSON folder name (unless user opts to uncomment hard codded path in method 'Main')
         * expected output is the contents of the JSON files in JSON folder are added to 'theMovies'
         * returned values are the items added to 'theMovies'
         */
        public MovieCollection(string jsonDirectory)
        {
            jsonPath = jsonDirectory; 
            string[] jsonFiles = Directory.GetFiles(jsonPath); 
            Movies oneMovie = new Movies(); 
            string jsonData = ""; 
            foreach (var jsonFile in jsonFiles)
            {
                // Checks to see if JSON files in JSON folder are able to be read, dematerialized, and added to List 'theMovies // exception prints warning of file not added and said file name
                try
                {
                    jsonData = File.ReadAllText(jsonFile);
                    oneMovie = JsonSerializer.Deserialize<Movies>(jsonData);
                    theMovies.Add(oneMovie);
                }
                catch (Exception) 
                {
                    Console.WriteLine("'{0}' file format is invalid and was not added to theMovies List.", Path.GetFileName(jsonFile));
                    continue; 
                }
            }
            if (theMovies.Count == 0) 
            {
                bool validFolder = false;
                while (!validFolder)
                {

                    Console.WriteLine("\nNo movies were added from folder '" + jsonPath + "'. Please re-enter folder name or press enter to edit:");
                    jsonPath = Console.ReadLine();
                    Console.WriteLine();
                    if (jsonPath == "") { Environment.Exit(0); }
                    else
                    {
                        jsonPath = Convert.ToString(jsonPath);
                        string[] testFiles = Directory.GetFiles(jsonPath);
                        // checks user input folder to see if it contains files, prints error if not, and re-queries user
                        if (testFiles.Length == 0)
                        {
                            Console.WriteLine("The folder " + jsonPath + " does not contain any files.");
                            validFolder = false;
                        }
                        else
                        {
                            string[] altFiles = Directory.GetFiles(jsonPath);
                            foreach (var jsonFile in altFiles)
                            {
                                // Checks to see if JSON files in JSON folder are able to be read, dematerialized, and added to List 'theMovies // exception prints warning of file not added and said file name
                                try
                                {
                                    jsonData = File.ReadAllText(jsonFile);
                                    oneMovie = JsonSerializer.Deserialize<Movies>(jsonData);
                                    theMovies.Add(oneMovie);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("'{0}' file format is invalid and was not added to theMovies List.", Path.GetFileName(jsonFile));
                                    continue;
                                }
                            }
                            // check to re-query user if files in folder did not contain any valid data
                            if (theMovies.Count != 0) { validFolder = true; }
                        }
                    }
                }
            } 
        }
        /*
         * called from method 'Main' in class 'Program' by user input '2' 
         * this method's function is to query the user for movies ratings per movie in 'theMovies' 
         * required inputs are user input '2' in method 'Main' and 'theMovies' 
         * expected out is: any valid ratings the user inputs are added to 'aMovie.rating' in 'theMovies' 
         * no returned values other than addition of 'aMovie.rating'
        */
        public void UpdateRating()
        {
            Console.WriteLine("Please enter the movie rating (*-*****) to add or replace rating. Press enter to exit or enter 0 to skip.");
            bool validRating = false;
            while (!validRating)
            {
                foreach (var aMovie in theMovies)
                {
                    Console.WriteLine(aMovie.title + " " + aMovie.year + " Rating: " + aMovie.rating);
                    var movieRating = Console.ReadLine();
                    if (movieRating == "") { validRating = true; break; }
                    // else if to next movie if the user does not wish to update current selection
                    else if (movieRating == "0") { Console.WriteLine(""); continue; }
                    // checks for valid user input, prints error if not and re-queries
                    else if (movieRating == "*" || movieRating == "**" || movieRating == "***" || movieRating == "****" || movieRating == "*****") { validRating = true; aMovie.rating = movieRating; }
                    else { Console.WriteLine("The above input is invalid. Please enter a valid rating, * - *****, only.\n"); validRating = false; }
                }
            }
            Console.WriteLine("\nPlease reopen program to make another selection.");
        }
        /*
         * called from method 'Main' in class 'Program' 
         * this method's function is to add a movie to 'theMovies'
         * required inputs are user input '3' in method 'Main', 'theMovies', additional user input for aMovie.(title, year, runtime, genre, and rating)
         * expected output is the user input movie is added to 'theMovies' and consequently written to a JSON file in the JSON folder
         */
        public void AddMovie()
        {
            Movies AdditionalMovie = new Movies();
            Console.WriteLine("\nHit enter only to return to exit program.\n");
            Random id = new Random();
            // tries to generate movie id similar to id formatting // prints error if not
            try { AdditionalMovie.id = ($"tt{id.Next(0, 1000000).ToString("D7")}"); } // short of writing a web scrapper to pull IMDB movie ID based on title/year, I'm not sure what to use other than this. sorry.
            catch (Exception idGenerator) { Console.WriteLine("\nEncountered and error when auto generating a movie ID.\n" + idGenerator.Message); }

            bool validTitle = false;
            Console.WriteLine("Please enter the movie title:");
            // checks for valid user input, prints error if not and re-queries
            try
            {
                var addTitle = Console.ReadLine();
                if (addTitle == "") { Environment.Exit(0); }
                else { AdditionalMovie.title = addTitle; }
                validTitle = true;
            }
            catch (Exception input) { Console.WriteLine("\nPlease enter letters or numbers only."); Console.WriteLine(input.Message); }
            while (!validTitle) ;

            bool validYear = false;
            Console.WriteLine("Please enter the movie year:");
            // checks for valid user input, prints error if not and re-queries
            try
            {
                int year = DateTime.Now.Year;
                var addYear = Console.ReadLine();
                if (addYear == "") { Environment.Exit(0); }
                else if (Convert.ToInt32(addYear) <= year & Convert.ToInt32(addYear) >= 1895) { AdditionalMovie.year = addYear; }
                else { Console.WriteLine("\nInvalid year input. Please enter a valid year ~ >= 1895 next time\n"); }
                validYear = true;
            }
            catch (Exception input)
            { Console.WriteLine("\nInvalid year input. Please enter a valid year ~ >= 1895 next time\n" + input.Message); }
            while (!validYear) ;

            bool validRuntime = false;
            Console.WriteLine("Please enter the movie runtime:");
            // checks for valid user input, prints error if not and re-queries
            try
            {
                var addRuntime = Console.ReadLine();
                if (addRuntime == "") { Environment.Exit(0); }
                else if (Convert.ToInt32(addRuntime) <= 2147483647 & (Convert.ToInt32(addRuntime) >= 1)) { AdditionalMovie.runtime = addRuntime; }
                else { Console.WriteLine("\nInvalid runtime input. Please enter a valid runtime in minuets next time"); }
                validRuntime = true;
            }
            catch (Exception input)
            { Console.WriteLine("\nInvalid runtime input. Please enter a valid runtime in minuets next time.\n" + input.Message); }
            while (!validRuntime) ;

            bool validGenre = false;
            Console.WriteLine("Please enter the movie genre:");
            // checks for valid user input, prints error if not and re-queries
            try
            {
                var addGenre = Console.ReadLine();
                if (addGenre == "") { Environment.Exit(0); }
                else { AdditionalMovie.genre = addGenre; }
                validGenre = true;
            }
            catch (Exception input)
            { Console.WriteLine("\nPlease enter letters or numbers only."); Console.WriteLine(input.Message); }
            while (!validGenre) ;

            bool validRating = false;
            Console.WriteLine("Please enter the movie rating (*-*****):");
            // checks for valid user input, prints error if not and re-queries
            try
            {
                var addRating = Console.ReadLine();
                if (addRating == "") { Environment.Exit(0); }
                else if (addRating == "*" || addRating == "**" || addRating == "***" || addRating == "****" || addRating == "*****") { AdditionalMovie.rating = addRating; }
                validRating = true;
            }
            catch (Exception input)
            { Console.WriteLine("\nPlease enter a valid rating, either * - *****.\n"); Console.WriteLine(input.Message); }
            while (!validRating) ;

            theMovies.Add(AdditionalMovie);
            Console.WriteLine("\nPlease reopen program to make another selection.");
        }
        /*
         * called from method 'Main' in class 'Program'
         * this method's function is to query user input for aMovie.id to delete, validate the user input, and call method 'DeleteJSON'
         * required inputs are user input '4' in method 'Main', 'theMovies' and user input for 'idChosen'
         * expected output is the user input 'idChosen' is passed to method 'DeleteJSON'
         * returned value is passed 'idChosen' passed to 'DeleteJSON'
         */
        public void DeleteMovies()
        {
            List<Movies> SortedList = new List<Movies> { };
            SortedList.AddRange(theMovies);
            SortedList.Sort();
            Console.WriteLine("id".PadLeft(6) + "".PadLeft(3) + "|rating".PadRight(1) + "|year|".PadRight(1) + "title".PadRight(1));
            Console.WriteLine("--------- ------ ---- ------");

            foreach (var aMovie in SortedList)
            {
                // check to adjust spacing based on if there is a user rating, and if so, what the rating is
                if (aMovie.rating != null)
                {
                    int ratingSpace = aMovie.rating.Length;
                    Console.Write(aMovie.id.PadLeft(1) + " " + aMovie.rating.PadRight(1) + "".PadLeft(7 - ratingSpace) + aMovie.year.PadLeft(1) + " " + aMovie.title.PadRight(1));
                }
                else
                {
                    Console.Write(aMovie.id.PadLeft(1) + "".PadLeft(8) + aMovie.year.PadLeft(1) + " " + aMovie.title.PadRight(1));
                }
                Console.WriteLine("");
            }

            Console.Write("\nPlease enter the movie id below to delete. Press enter to exit or enter 0 to skip through available movies.\n");
            bool validDelete = false;
            while (!validDelete)
            {
                string idChosen = Console.ReadLine();
                int indexToDelete = -1;
                if (idChosen == "") { Environment.Exit(0); }
                else if (idChosen == "0")
                {
                    Console.WriteLine("Enter 0 to skip, 1 to delete, or hit enter to exit:");
                    int idIndex = 0;
                    bool deletePicked = false;
                    while (!deletePicked)
                    {
                        if (idIndex <= theMovies.Count)
                        {
                            string deleteOrNot = theMovies[idIndex].id + "  " + theMovies[idIndex].rating + "  " + theMovies[idIndex].year + "  " + theMovies[idIndex].title + ":  ";
                            Console.Write(deleteOrNot);
                            string deleteOrSkip = Console.ReadLine();
                            // tries to delete user input JSON file name based on user input id // prints error and re-queries if not 
                            try
                            {
                                if (deleteOrSkip == "1") { deletePicked = true; DeleteJSON(theMovies[idIndex].id); }
                                else if (deleteOrSkip == "0") { idIndex += 1; }
                                else if (deleteOrSkip == "") { Environment.Exit(0); }
                            }
                            catch
                            {
                                Console.WriteLine("\nDue to an error, the below movie was not deleted:");
                                deletePicked = false;
                                validDelete = false;
                            }
                        }
                        else { idIndex = 0; }
                    }
                }
                else
                {
                    for (int i = 0; i < theMovies.Count; i++)
                    {
                        if (theMovies[i].id == idChosen)
                        {
                            indexToDelete = i;
                            break;
                        }
                    }
                    // checks to verify user input is in list 'theMovies.id' // prints error and re-queries if not
                    if (indexToDelete != -1)
                    {
                        // tries to delete user input JSON file name based on user input id // prints error and re-queries if not 
                        try
                        {
                            validDelete = true;
                            DeleteJSON(idChosen);
                        }
                        catch { Console.WriteLine("\nDue to an error, the following movie was not deleted: " + theMovies[indexToDelete].id + "  " + theMovies[indexToDelete].rating + "  " + theMovies[indexToDelete].year + "  " + theMovies[indexToDelete].title); }
                    }
                    else { Console.WriteLine("Invalid movie ID or ID not found.\n\nPlease enter a valid movie ID listed above:"); }
                }
            }
        }
        /*
         * called from method 'DeleteMovies'
         * this method's function is to delete the JSON file per user input of aMovie.id
         * required inputs are 'theMovies' and 'idChosen' passed from method 'DeleteMovies'
         * expected output is the deletion of JSON file in JSON folder that has the file name matching user input 'idChosen'
         * no returned values, just prints confirmation
        */
        private void DeleteJSON(string idToDelete)
        {
            int indexToDelete = 0;
            for (int i = 0; i < theMovies.Count; i++)
            {
                if (theMovies[i].id == idToDelete)
                {
                    indexToDelete = i;
                    break;
                }
            }
            File.Delete(jsonPath + "/" + idToDelete + ".json");
            string deleted = "\nThe following movie has been deleted: " + theMovies[indexToDelete].id + "  " + theMovies[indexToDelete].rating + "  " + theMovies[indexToDelete].year + "  " + theMovies[indexToDelete].title;
            theMovies.RemoveAt(indexToDelete);
            Console.WriteLine(deleted);
            Environment.Exit(0);
        }
        /* 
         * called from method 'Main' in class 'Program' 
         * this method's function is to save all items in list 'theMovies' to the JSON files in user specified JSON folder updating any current files in JSON folder and creating new files for new items added to 'theMovies' 
         * required inputs are 'jsonFolder' and 'theMovies' 
         * expected output is writing the items in 'theMovies' to JSON files in JSON folder 
         * no returned values 
        */
        public void SaveAllJSON(string jsonFolder)
        {
            // tries to write items in 'theMovies' to JSON files in JSON folder // prints error if not
            try
            {
                foreach (var aMovie in theMovies)
                {
                    var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                    string jsonData = JsonSerializer.Serialize(aMovie, jsonOptions);
                    StreamWriter writer = new StreamWriter(jsonFolder + "/" + aMovie.id + ".json");
                    writer.Write(jsonData);
                    writer.Close();
                }
            }
            catch (Exception ex) { Console.Write("Program experienced an error when trying to write files to '" + jsonFolder + "'. "); Console.WriteLine(ex.Message); }
        }
    }
    /*
     * class 'Movies' is used to organize, edit, and save movies and modifications to movie data to and from the JSON files in JSON folder 
     * No inputs are required, but it accepts input from class 'MovieCollection' 
     * no expected outputs 
     * no returned values
    */
    class Movies : IComparable
    {
        public string id { get; set; }
        public string title { get; set; }
        public string year { get; set; }
        public string runtime { get; set; }
        public string genre { get; set; }
        public string rating { get; set; }

        int IComparable.CompareTo(object obj)
        {
            Movies temp = (Movies)obj;
            return String.Compare(this.title, temp.title);
        }
    }
}
